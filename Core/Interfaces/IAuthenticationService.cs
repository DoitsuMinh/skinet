using Core.Enitities;
using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isPersistent = false);
        Task<Result<string>> GetUserRoleAsync(AppUser user);
        Task<Result<(string, string)>> RegisterByPassAsync(AppUser user, string password, string userRole, bool isPersistent = false);
        Task<Result<bool>> ValidateRefreshTokenAsync(AppUser user);
        Task<Result<bool>> ClearRefreshTokenAsync(AppUser user);
        Task<Result<string>> GetRefreshTokenAsync(AppUser user);
        Task<Result<string>> CreateRefreshTokenAsync(AppUser user);
        Task<Result<AppUser>> GetUserByEmailAsync(string email);
    }
}
