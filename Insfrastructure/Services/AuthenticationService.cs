using Core.Enitities;
using Core.Enitities.Identity;
using Core.Interfaces;
using Insfrastructure.Identity;
using Insfrastructure.Infrastructures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Insfrastructure.Services
{
    public class AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, IRefreshTokenRepository refreshTokenRepo
        //,SignInManager<AppUser> signInManager
        //, AppIdentityDbContext identityDbContext
        ) : IAuthenticationService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        //private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IRefreshTokenRepository _refreshTokenRepo = refreshTokenRepo;
        //private readonly AppIdentityDbContext _identityDbContext = identityDbContext;

        public async Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isRememberPass)
        {
            throw new NotImplementedException();
            //var user = await _userManager.FindByEmailAsync(email);
            //if (user == null) return Result<AppUser>.Failure("Incorrect email");

            //var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            //if (!result.Succeeded) return Result<AppUser>.Failure("Incorrect password");

            //return Result<AppUser>.Success(user);
        }

        public async Task<Result<AppUser>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Result<AppUser>.Failure("Incorrect email");
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


        public async Task<Result<string>> CreateRefreshTokenAsync(AppUser user)
        {
            throw new NotImplementedException();
            //var refreshToken = _tokenService.GenerateRefreshToken();
            //var refreshTokenObject = new RefreshToken
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserId = user.Id,
            //    Token = refreshToken,
            //    ExpiresOnUtc = DateTime.UtcNow.AddDays(1)
            //};
            //_identityDbContext.RefreshTokens.Add(refreshTokenObject);

            //var result = await _identityDbContext.SaveChangesAsync() > 0;
            //if (!result) return Result<string>.Failure("Failed to create refresh token");

            //return Result<string>.Success(refreshToken);
        }

        public async Task<Result<bool>> ValidateRefreshTokenAsync(AppUser user)
        {
            var result = await _refreshTokenRepo.ValidateRefreshTokenAsync(user);
            if (!result) return Result<bool>.Failure("Refresh token not exists");
            return Result<bool>.Success(true);
        }

        public async Task<Result<string>> GetRefreshTokenAsync(AppUser user)
        {
            throw new NotImplementedException();
            //var tokenResult = await _identityDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);
            //if (tokenResult is null) return Result<string>.Failure("Refresh token not exists");
            //return Result<string>.Success(tokenResult.Token);
        }

        public async Task<Result<bool>> ClearRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
            //var refreshTokenRes = await _identityDbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
            //if (refreshTokenRes is not null)
            //{
            //    refreshTokenRes.Token = string.Empty;
            //}
            //await _identityDbContext.SaveChangesAsync();
            //return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RegisterByPassAsync(AppUser user, string password, string userRole)
        {
            throw new NotImplementedException();
            //using var transaction = await _identityDbContext.Database.BeginTransactionAsync();

            //try
            //{
            //    var createUserResult = await _userManager.CreateAsync(user, password);
            //    if (!createUserResult.Succeeded)
            //    {
            //        throw new Exception(string.Join(", ", createUserResult.Errors.Select(x => x.Description)));
            //    }

            //    var addRoleResult = await _userManager.AddToRoleAsync(user, "Customer");
            //    if (!createUserResult.Succeeded)
            //    {
            //        throw new Exception(string.Join(", ", addRoleResult.Errors.Select(x => x.Description)));
            //    }

            //    await transaction.CommitAsync();
            //    return Result<bool>.Success(true);
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    return Result<bool>.Failure(ex.Message);
            //}
        }


        public async Task<Result<Response>> Test(string token)
        {
            throw new NotImplementedException();
            //RefreshToken? refreshToken = await _identityDbContext.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);
            //if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            //{
            //    return Result<Response>.Failure("The refresh token has expired");
            //}

            //return Result<Response>.Success(new Response("", ""));
        }

        public async Task<Result<AppUser>> ValidateRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
            //try
            //{
            //    RefreshToken? refreshToken = await _identityDbContext.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);

            //    if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            //    {
            //        return Result<AppUser>.Failure("The refresh token is expired");
            //    }

            //    var user = await _identityDbContext.Users.FirstOrDefaultAsync(r => r.Id == refreshToken.UserId);
            //    return Result<AppUser>.Success(user);
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception
            //    //_logger.LogError(ex, "Error validating refresh token");
            //    return Result<AppUser>.Failure("Error validating refresh token");
            //}
        }

        public async Task<Result<bool>> LoginUserAsync(string email, string password)
        {
            throw new NotImplementedException();
            ////ValidateLoginByPass
            //var user = await _userManager.FindByEmailAsync(email);
            //if (user == null) return Result<bool>.Failure("User note existed");

            //var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            //if (!result.Succeeded) return Result<bool>.Failure("Incorrect password");

            //return Result<bool>.Success(true);
        }

        public string GenerateAccessToken(AppUser user, string userRole)
        {
            return _tokenService.CreateAccessToken(user, userRole);
        }

        public async Task<Result<AppUser>> GetUserByRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
            //var tokenResult = await _identityDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            //if (tokenResult is null) return Result<AppUser>.Failure("Invalid token");

            //var user = await _identityDbContext.Users.FirstOrDefaultAsync(x => x.Id == tokenResult.UserId);
            //if (user is null) return Result<AppUser>.Failure("user not found");
            //return Result<AppUser>.Success(user);

        }
    }
}
