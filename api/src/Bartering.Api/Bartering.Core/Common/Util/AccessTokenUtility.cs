using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bartering.Core.Common.Util;

internal static class AccessTokenUtility
{
    /// <summary>
    /// Reads a JSON Web Token and returns the subject.
    /// </summary>
    /// <param name="token">A JSON Web Token.</param>
    /// <returns>The 'sub' claim of the token, which should include the user ID. If the 'sub' claim is not found, an empty string is returned.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="token"/> is null or empty.</exception>
    public static string GetUserId(string token)
    {
        return new JsonWebTokenHandler().ReadJsonWebToken(token).Subject;
    }

    /// <summary>
    /// Reads a JSON Web Token and returns the claims.
    /// </summary>
    /// <param name="token">A JSON Web Token.</param>
    /// <returns>A <see cref="System.Collections.Generic.IEnumerable{Claim}"/> of claims, where each claim is a <see cref="System.Security.Claims.Claim"/></returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="token"/> is null or empty.</exception>
    public static IEnumerable<Claim> GetClaims(string token)
    {
        return new JsonWebTokenHandler().ReadJsonWebToken(token).Claims;
    }
}
