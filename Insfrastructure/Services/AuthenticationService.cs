using Core.Enitities;
using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Insfrastructure.Services
{
    public class AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IRefreshTokenRepository refreshTokenRepo) : IAuthenticationService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IRefreshTokenRepository _refreshTokenRepo = refreshTokenRepo;

        public async Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isRememberPass)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Result<AppUser>.Failure("incorrect email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return Result<AppUser>.Failure("Incorrect password");

            return Result<AppUser>.Success(user);
        }

        public async Task<Result<string>> GetUserRoleAsync(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                return Result<string>.Failure("Missing user role. WHY ?");
            }

            return Result<string>.Success(userRoles.FirstOrDefault());
        }
    }
}
