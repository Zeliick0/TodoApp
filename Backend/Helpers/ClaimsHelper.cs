using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Model;

namespace TodoApp.Helpers;

public static class ClaimsHelper
{
    public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            return null;
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            return null;
        }
        
        return userId;
    }
}