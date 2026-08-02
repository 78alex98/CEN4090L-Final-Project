namespace Bartering.Core.Features.Authentication.Dtos;

public record TokenDto(RefreshTokenDto RefreshToken, AccessTokenDto AccessToken);
