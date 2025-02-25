using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddRefreshTokenAsync(AppUser user, string resfreshToken);
        Task<bool> ValidateRefreshTokenAsync(AppUser user);
        Task<string> GetRefreshTokenAsync(AppUser user);
    }
}
