using Core.Enitities;
using Core.Enitities.Identity;
using Core.Interfaces;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Insfrastructure.Services
{
    public class AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IRefreshTokenRepository refreshTokenRepo, AppIdentityDbContext identityDbContext) : IAuthenticationService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IRefreshTokenRepository _refreshTokenRepo = refreshTokenRepo;
        private readonly AppIdentityDbContext _identityDbContext = identityDbContext;
        private const string LOGIN_PROVIDER = "Identity";
        private const string TOKEN_NAME = "RefreshToken";

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


        private async Task<Result<bool>> CreateRefreshTokenAsync(AppUser user)
        {
            var refreshToken = _tokenService.CreateRefreshToken();
            var result = await _refreshTokenRepo.AddRefreshTokenAsync(user, refreshToken);
            if (!result) return Result<bool>.Failure("Failed to create refresh token");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> ValidateRefreshTokenAsync(AppUser user)
        {

            var result = await _refreshTokenRepo.ValidateRefreshTokenAsync(user);
            if (!result) return Result<bool>.Failure("Refresh token not exists");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> ClearRefreshTokenAsync(AppUser user)
        {
            var removeResult = await _userManager.RemoveAuthenticationTokenAsync(user, LOGIN_PROVIDER, TOKEN_NAME);
            if (removeResult.Errors.Any())
            {
                return Result<bool>.Failure(string.Join(", ", removeResult.Errors.Select(x => x.Description)));
            }
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RegisterByPassAsync(AppUser user, string password, string userRole, bool isPersistent = false)
        {
            using var transaction = await _identityDbContext.Database.BeginTransactionAsync();

            try
            {
                var createUserResult = await _userManager.CreateAsync(user, password);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception(string.Join(", ",createUserResult.Errors.Select(x => x.Description)));
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, userRole);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", addRoleResult.Errors.Select(x => x.Description)));
                }

                var createRefreshTokenResult = await CreateRefreshTokenAsync(user);
                if (!createRefreshTokenResult.Value)
                {
                    throw new Exception(createRefreshTokenResult.Error);
                }
                var token = _tokenService.CreateAccessToken(user, userRole);

                await _signInManager.SignInAsync(user, isPersistent: true);

                await transaction.CommitAsync();
                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<bool>.Failure(ex.Message);
            }
        }
    }
}
