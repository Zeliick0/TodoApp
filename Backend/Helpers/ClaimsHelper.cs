using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Model;

namespace TodoApp.Helpers;

public static class ClaimsHelper
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            throw new Exception("Claim not found");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new Exception("Invalid user id");
        }

        return userId;
    }
}