using Bartering.Core.Common.Dtos;
using Bartering.Core.Features.Authentication.Dtos;

namespace Bartering.Core.Features.Authentication;

public interface IAuthenticationService
{
    public Task<ApplicationUserDto?> RegisterUser(AuthenticationDto request);

    public Task<ApplicationUserDto?> Refresh(RefreshTokenDto request);

    public Task<ApplicationUserDto?> LoginUser(AuthenticationDto request);

    public Task<bool> LogoutUser(RefreshTokenDto refreshToken);
}
