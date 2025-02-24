using Core.Enitities.Identity;
using Core.Interfaces;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.Data
{
    public class RefreshTokenRepository(UserManager<AppUser> userManager, AppIdentityDbContext identityDbContext) : IRefreshTokenRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly AppIdentityDbContext _identityDbContext = identityDbContext;
        private const string LOGIN_PROVIDER = "Identity";
        private const string TOKEN_NAME = "RefreshToken";

        /// <summary>
        /// Generate a refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        public async Task<bool> AddRefreshTokenAsync(AppUser user, string refreshToken)
        {
            if (user.Id == null) return false;

            var result = await _identityDbContext.Set<AppUserToken>().AddAsync(new AppUserToken
            {
                UserId = user.Id,
                LoginProvider = LOGIN_PROVIDER,
                Name = TOKEN_NAME,
                Value = refreshToken,
                ExpiredDate = DateTime.UtcNow.AddDays(1)
            });
            return await _identityDbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Validate the refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        public async Task<bool> ValidateRefreshTokenAsync(AppUser user)
        {
            var storedToken = await _identityDbContext.Set<AppUserToken>().FirstOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == LOGIN_PROVIDER && x.Name == TOKEN_NAME);
            return storedToken != null && !string.IsNullOrEmpty(storedToken.Value);
        }
    }
}
