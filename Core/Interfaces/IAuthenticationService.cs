using Core.Enitities;
using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isPersistent = false);
        Task<Result<string>> GetUserRoleAsync(AppUser user);
        Task<Result<bool>> RegisterByPassAsync(AppUser user, string password, string userRole, bool isPersistent = false);
        Task<Result<bool>> ValidateRefreshTokenAsync(AppUser user);
        Task<Result<bool>> ClearRefreshTokenAsync(AppUser user);
    }
}
