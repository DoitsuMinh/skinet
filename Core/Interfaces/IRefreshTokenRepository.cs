using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SetRefreshTokenAsync(AppUser user, string resfreshToken);
        Task RevokeRefreshTokenAsync(AppUser user);
        Task<bool> ValidateRefreshTokenAsync(AppUser user, string refreshToken);

    }
}
