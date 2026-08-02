using System.Text.Json;
using Bartering.Core.Common.Dtos;
using Bartering.Core.Features.Authentication.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bartering.Core.Features.Authentication;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;

    private readonly IAuthenticationService _service;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService service)
    {
        _logger = logger;
        _service = service;
    }

    [EndpointSummary("Create account")]
    [EndpointDescription(
        "Creates a new user account. Returns a refresh token cookie, a JWT access token cookie, "
            + "and a cookie with basic user data in the response."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] AuthenticationDto request)
    {
        var result = await _service.RegisterUser(request);

        if (result is null)
            return BadRequest();

        AddTokensToCookies(result.Tokens?.RefreshToken, result.Tokens?.AccessToken);
        AddUserDataToCookies(result);
        return Created();
    }

    [EndpointSummary("Refresh access token")]
    [EndpointDescription(
        "Generates a new access token if the refresh token is valid. Generally this is used for when the access token expires."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (refreshToken is null)
            return Unauthorized();

        var result = await _service.Refresh(new RefreshTokenDto(refreshToken));

        if (result is null)
            return Unauthorized();

        AddTokensToCookies(result.Tokens?.RefreshToken, result.Tokens?.AccessToken);
        AddUserDataToCookies(result);
        return Ok();
    }

    [EndpointSummary("Login")]
    [EndpointDescription(
        "Authenticates an existing user with a username and password. "
            + "Returns a refresh token cookie, a JWT access token cookie, and a cookie with basic user data in the response."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] AuthenticationDto request)
    {
        var result = await _service.LoginUser(request);

        if (result is null)
            return Unauthorized();

        AddTokensToCookies(result.Tokens?.RefreshToken, result.Tokens?.AccessToken);
        AddUserDataToCookies(result);
        return Ok();
    }

    [EndpointSummary("Log out")]
    [EndpointDescription("Logs a user out by invalidating cookies and the refresh token.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (refreshToken is null)
            return Unauthorized();

        var result = await _service.LogoutUser(new RefreshTokenDto(refreshToken));

        if (!result)
            return BadRequest();

        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            }
        );

        Response.Cookies.Delete(
            "accessToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            }
        );

        Response.Cookies.Delete(
            "userData",
            new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            }
        );

        return Ok();
    }

    private void AddTokensToCookies(RefreshTokenDto? refreshToken, AccessTokenDto? accessToken)
    {
        if (refreshToken is not null)
        {
            Response.Cookies.Append(
                "refreshToken",
                refreshToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpiresOn,
                }
            );
        }

        if (accessToken is not null)
        {
            Response.Cookies.Append(
                "accessToken",
                accessToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = accessToken.ExpiresOn,
                }
            );
        }
    }

    private void AddUserDataToCookies(ApplicationUserDto user)
    {
        var userJson = JsonSerializer.Serialize(user);

        Response.Cookies.Append(
            "userData",
            userJson,
            new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1),
            }
        );
    }
}
