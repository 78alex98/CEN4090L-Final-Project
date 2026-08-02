using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bartering.Core.Common.Dtos;
using Bartering.Core.Features.Authentication.Dtos;
using Bartering.Data.Database;
using Bartering.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bartering.Core.Features.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    private readonly BarteringDbContext _dbContext;

    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        BarteringDbContext dbContext,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<ApplicationUserDto?> RegisterUser(AuthenticationDto request)
    {
        var applicationUser = new ApplicationUser { UserName = request.UserName };

        var result = await _userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
            return null;

        var refreshToken = await GenerateRefreshToken(applicationUser, DateTime.UtcNow.AddDays(1));
        var accessToken = GenerateAccessToken(applicationUser, DateTime.UtcNow.AddMinutes(30));
        var tokens = new TokenDto(refreshToken, accessToken);

        return new ApplicationUserDto(applicationUser.UserName, tokens);
    }

    public async Task<ApplicationUserDto?> LoginUser(AuthenticationDto request)
    {
        var applicationUser = await _userManager.FindByNameAsync(request.UserName);

        if (applicationUser is null)
            return null;

        var passwordValidity = await _userManager.CheckPasswordAsync(applicationUser, request.Password);

        if (!passwordValidity)
            return null;

        var refreshToken = await GenerateRefreshToken(applicationUser, DateTime.UtcNow.AddDays(1));
        var accessToken = GenerateAccessToken(applicationUser, DateTime.UtcNow.AddMinutes(30));
        var tokens = new TokenDto(refreshToken, accessToken);

        return new ApplicationUserDto(applicationUser.UserName, tokens);
    }

    public async Task<bool> LogoutUser(RefreshTokenDto refreshToken)
    {
        var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken.Token);

        if (token is null)
            return false;

        _dbContext.RefreshTokens.Remove(token);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<ApplicationUserDto?> Refresh(RefreshTokenDto request)
    {
        var refreshToken = await _dbContext
            .RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.Token);

        if (refreshToken is null)
            return null;

        if (refreshToken.ExpiresOn <= DateTime.UtcNow)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
            return null;
        }

        var accessToken = GenerateAccessToken(refreshToken.User, DateTime.UtcNow.AddMinutes(30));
        var tokens = new TokenDto(new RefreshTokenDto(refreshToken), accessToken);

        return new ApplicationUserDto(refreshToken.User.UserName, tokens);
    }

    private async Task<RefreshTokenDto> GenerateRefreshToken(ApplicationUser applicationUser, DateTime expiresOn)
    {
        var randomNumber = new byte[32];

        // Using statement because RandomNumberGenerator implements IDisposable.
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        var refreshTokenString = Convert.ToBase64String(randomNumber);

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            ExpiresOn = expiresOn,
            UserId = applicationUser.Id,
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new RefreshTokenDto(refreshToken);
    }

    private AccessTokenDto GenerateAccessToken(ApplicationUser request, DateTime expiresOn)
    {
        IEnumerable<Claim> claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, request.Id) };

        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = expiresOn,
            SigningCredentials = signingCredentials,
        };

        var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

        return new AccessTokenDto(token, expiresOn);
    }
}
