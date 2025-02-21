using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Insfrastructure.Data
{
    public class RefreshTokenRepository(ITokenService tokenService, UserManager<AppUser> userManager) : IRefreshTokenRepository
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private const string LOGIN_PROVIDER = "Identity";
        private const string TOKEN_NAME = "RefreshToken";

        /// <summary>
        /// Generate a refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        public async Task SetRefreshTokenAsync(AppUser user, string refreshToken)
        {
            await _userManager.SetAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME, refreshToken);
        }

        /// <summary>
        /// Revoke the refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        public async Task RevokeRefreshTokenAsync(AppUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME);
        }

        /// <summary>
        /// Validate the refresh token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        public async Task<bool> ValidateRefreshTokenAsync(AppUser user, string refreshToken)
        {
            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME);
            return storedToken == refreshToken && !string.IsNullOrEmpty(storedToken);
        }
    }
}
