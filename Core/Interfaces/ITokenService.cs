using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, string userRole);
        string CreateAccessToken(AppUser user,string userRole);
        string CreateRefreshToken();
    }
}