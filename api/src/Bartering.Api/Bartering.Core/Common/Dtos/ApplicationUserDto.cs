using System.Text.Json.Serialization;
using Bartering.Core.Features.Authentication.Dtos;
using Bartering.Data.Models;

namespace Bartering.Core.Common.Dtos;

[method: JsonConstructor]
public record ApplicationUserDto(string? UserName, [property: JsonIgnore] TokenDto? Tokens)
{
    public ApplicationUserDto(ApplicationUser applicationUser)
        : this(applicationUser.UserName, null) { }
}
