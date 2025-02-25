using Core.Enitities.Identity;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, string userRole);
        string CreateAccessToken(AppUser user,string userRole);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}