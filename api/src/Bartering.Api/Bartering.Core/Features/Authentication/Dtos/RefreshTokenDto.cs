using Bartering.Data.Models;

namespace Bartering.Core.Features.Authentication.Dtos;

public record RefreshTokenDto(string Token, DateTime? ExpiresOn = null)
{
    public RefreshTokenDto(RefreshToken refreshToken)
        : this(refreshToken.Token, refreshToken.ExpiresOn) { }
}
