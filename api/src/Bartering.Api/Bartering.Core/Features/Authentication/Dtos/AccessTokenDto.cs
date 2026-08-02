namespace Bartering.Core.Features.Authentication.Dtos;

public record AccessTokenDto(string Token, DateTime? ExpiresOn = null);
