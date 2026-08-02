using Bartering.Core.Features.Authentication;
using Bartering.Core.Features.Authentication.Dtos;
using Bartering.Data.Database;
using Bartering.Data.Models;
using Bartering.Tests.Integration.Fixtures;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Bartering.Tests.Integration;

public sealed class AuthenticationServiceTests(BarteringDbFixture dbFixture)
    : IClassFixture<BarteringDbFixture>,
        IAsyncLifetime
{
    private readonly IServiceProvider _serviceProvider = dbFixture.Services;

    private readonly Func<Task> _resetDatabase = dbFixture.ResetDatabaseAsync;

    [Fact]
    public async Task When_given_valid_credentials_registration_is_successful()
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var credentials = new AuthenticationDto(UserName: "johndoe", Password: "Pa$sword1");

        var result = await service.RegisterUser(credentials);

        result
            .Should()
            .NotBeNull("because it should return user information if the username and password meet requirements");
    }

    [Fact]
    public async Task When_given_invalid_credentials_registration_fails()
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var credentials = new AuthenticationDto(UserName: string.Empty, Password: "pass");

        var result = await service.RegisterUser(credentials);

        result
            .Should()
            .BeNull(
                "because it shouldn't return user information if the username and password are invalid or do not meet requirements "
            );
    }

    [Fact]
    public async Task When_given_valid_credentials_login_is_successful()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var newUser = new Faker<ApplicationUser>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .UseSeed(1)
            .Generate();
        var credentials = new AuthenticationDto(UserName: newUser.UserName!, Password: "Pa$sword1");

        await userManager.CreateAsync(newUser, credentials.Password);

        // Act
        var result = await service.LoginUser(credentials);

        // Assert
        using (new AssertionScope("NullAssertions"))
        {
            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Tokens.AccessToken.Should().NotBeNull();
        }

        using (new AssertionScope("TokenAssertion"))
        {
            result.Tokens.AccessToken.Token.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task When_given_invalid_credentials_login_fails()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var newUser = new Faker<ApplicationUser>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .UseSeed(1)
            .Generate();
        await userManager.CreateAsync(newUser, "Pa$sword1");

        var credentials = new AuthenticationDto(UserName: newUser.UserName!, Password: "pA!sword2");

        // Act
        var result = await service.LoginUser(credentials);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_user_does_not_exist_login_fails()
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var credentials = new AuthenticationDto(UserName: new Faker().Internet.UserName(), Password: "Pa$sword1");

        var result = await service.LoginUser(credentials);

        result.Should().BeNull();
    }

    [Fact]
    public async Task When_given_a_valid_refresh_token_a_new_access_token_is_returned()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<BarteringDbContext>();

        var newUser = new Faker<ApplicationUser>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .UseSeed(1)
            .Generate();
        var refreshToken = new RefreshToken
        {
            UserId = newUser.Id,
            ExpiresOn = DateTime.UtcNow.AddDays(1),
            Token = Convert.ToBase64String(new Faker().Random.Bytes(32)),
        };
        var refreshTokenDto = new RefreshTokenDto(refreshToken);

        await userManager.CreateAsync(newUser, "Pa$sword1");
        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.Refresh(refreshTokenDto);

        // Assert
        using (new AssertionScope("NullAssertions"))
        {
            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Tokens.AccessToken.Should().NotBeNull();
        }

        using (new AssertionScope("TokenAssertion"))
        {
            result.Tokens.AccessToken.Token.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task When_given_an_expired_refresh_token_refresh_fails()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<BarteringDbContext>();

        var newUser = new Faker<ApplicationUser>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .UseSeed(1)
            .Generate();
        var refreshToken = new RefreshToken
        {
            UserId = newUser.Id,
            ExpiresOn = DateTime.UtcNow,
            Token = Convert.ToBase64String(new Faker().Random.Bytes(32)),
        };
        var refreshTokenDto = new RefreshTokenDto(refreshToken);

        await userManager.CreateAsync(newUser, "Pa$sword1");
        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.Refresh(refreshTokenDto);

        // Assert
        result.Should().BeNull();
    }

    // Don't really need to do anything here, so just return completed.
    public Task InitializeAsync() => Task.CompletedTask;

    // Reset the database between tests.
    public Task DisposeAsync() => _resetDatabase();
}
