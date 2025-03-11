using Core.Enitities.Identity;
using Core.Interfaces;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.Data
{
    public class RefreshTokenRepository(UserManager<AppUser> userManager
        //,AppIdentityDbContext identityDbContext
        ) : IRefreshTokenRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        //private readonly AppIdentityDbContext _identityDbContext = identityDbContext;
        private const string LOGIN_PROVIDER = "Identity";
        private const string TOKEN_NAME = "RefreshToken";

        /// <summary>
        /// Generate a refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        public async Task<bool> AddRefreshTokenAsync(AppUser user, string refreshToken)
        {
            var setTokenResult = await _userManager.SetAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME, refreshToken);
            
            return setTokenResult.Succeeded;
        }

        /// <summary>
        /// Validate the refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        public async Task<bool> ValidateRefreshTokenAsync(AppUser user)
        {
            //var storedToken = await _identityDbContext.Set<AppUserToken>().FirstOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == LOGIN_PROVIDER && x.Name == TOKEN_NAME);
            //return storedToken != null && !string.IsNullOrEmpty(storedToken.Value);
            throw new NotImplementedException();
        }

        public async Task<string> GetRefreshTokenAsync(AppUser user)
        {
            //var storedToken = await _identityDbContext.Set<AppUserToken>().FirstOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == LOGIN_PROVIDER && x.Name == TOKEN_NAME);
            //return storedToken?.Value;
            throw new NotImplementedException();
        }
    }
}
