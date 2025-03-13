using API.Dtos;
using API.Errors;
using API.Extensions;
using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static API.Enums.TimeUnits;

/* DOC:
 * https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-auth-code-flow
 * Apply the OAuth 2.0 authorization
 */
namespace API.Controllers
{
    public class TokenController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;

        public TokenController(IAuthenticationService authenticationService, IConfiguration configuration, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> GenerateToken(LoginDto loginDto)
        {
            var userResult = await _authenticationService.GetUserByEmailAsync(loginDto.Email);
            if (!userResult.IsSuccess) return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, userResult.Error));

            var user = userResult.Value;

            var refreshTokenResult = await _authenticationService.CreateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess) return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, refreshTokenResult.Error));

            var refreshToken = refreshTokenResult.Value;

            var userRoleResult = await _authenticationService.GetUserRoleAsync(user);
            if (!userRoleResult.IsSuccess) return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, userRoleResult.Error));

            var accessToken = _authenticationService.GenerateAccessToken(user, userRoleResult.Value);

            return Ok(new TokenDto { AccessToken = accessToken });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("refresh")]
        public async Task<ActionResult<TokenDto>> Refresh()
        
        {
            // Try to get the refresh token from the cookie
            //if (Request.Cookies.TryGetValue(_configuration["Cookie:Name"], out var refreshToken))
            //{
            
            var user = await _signInManager.UserManager.GetUserAsync(User);

                // Generate a new refresh token
                var newRefreshTokenResult = await _authenticationService.UpdateRefreshTokenAsync(user);
                if (!newRefreshTokenResult.IsSuccess)
                {
                    return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Failed to generate refresh token"));
                }
                var newRefreshToken = newRefreshTokenResult.Value;

                //var userRoleResult = await _authenticationService.GetUserRoleAsync(user);
                //if (!userRoleResult.IsSuccess)
                //{
                //    // Clear the invalid cookie
                //    Response.Cookies.Delete(_configuration["Cookie:Name"]);
                //    return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Failed to get user role"));
                //}
                //var userRole = userRoleResult.Value;
                // Generate a new access token
                var accessToken = _authenticationService.GenerateAccessToken(user, "Customer");

                // Return the new access token
                return Ok(new TokenDto { AccessToken = accessToken });
            //}

            // Clear cookie if exist
            Response.Cookies.Delete(_configuration["Cookie:Name"]);
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Time out! 🙋"));
        }

        [Authorize(AuthenticationSchemes = "bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            // Try to get the refresh token from the cookie
            //if (!Request.Cookies.TryGetValue(_configuration["Cookie:Name"], out var refreshToken))
            //{
            // Validate the refresh token and get the user
            var user = await _signInManager.UserManager.GetUserAsync(User);
            await _authenticationService.RevokeRefreshTokenAsync(user.Id);
            //}

            // Clear the cookie
            Response.Cookies.Delete(_configuration["Cookie:Name"]);
            return NoContent();
        }
    }
}
