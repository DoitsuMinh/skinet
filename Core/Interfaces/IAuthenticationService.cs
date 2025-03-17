using Core.Enitities;
using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isPersistent = false);
        Task<Result<string>> GetUserRoleAsync(AppUser user);
        Task<Result<bool>> RegisterByPassAsync(AppUser user, string password, string userRole);
        //Task<Result<bool>> ValidateRefreshTokenAsync(AppUser user);
        Task<Result<AppUser>> GetUserByRefreshTokenAsync(string refreshToken);
        Task<Result<bool>> RevokeRefreshTokenAsync(string token);
        Task<Result<string>> GetRefreshTokenAsync(AppUser user);
        Task<Result<string>> CreateRefreshTokenAsync(AppUser user);
        Task<Result<(string, string)>> GenerateTokenAsync(AppUser user);
        //Task<Result<string>> UpdateRefreshTokenAsync(AppUser user);
        Task<Result<AppUser>> GetUserByEmailAsync(string email);
        Task<Result<bool>> LoginUserAsync(string email, string password);
        string GenerateAccessToken(AppUser user,string userRole);
        Task<Result<AppUser>> GetUserByRefreshTokenAsync(AppUser user);
    }
}
