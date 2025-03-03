using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Errors;
using API.Dtos;
using API.Extensions;
using static API.Enums.TimeUnits;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "bearer")]
    public class TokenController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;

        public TokenController(ITokenService tokenService, IAuthenticationService authenticationService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("refresh")]
        public async Task<ActionResult<UserDto>> Refresh()
        {
            // Try to get the refresh token from the cookie
            if (!Request.Cookies.TryGetValue("UserCookie", out var refreshToken))
            {
                return Unauthorized(new ApiResponse(401, "Refresh token not found"));
            }

            // Validate the refresh token and get the user
            var userResult = await _authenticationService.ValidateRefreshTokenAsync(refreshToken);
            if (!userResult.IsSuccess)
            {
                // Clear the invalid cookie
                Response.Cookies.Delete("UserCookie");
                return Unauthorized(new ApiResponse(401, userResult.Error ?? "Invalid refresh token"));
            }

            var user = userResult.Value;

            // Get the user's role
            var userRoleResult = await _authenticationService.GetUserRoleAsync(userResult.Value);
            if (!userRoleResult.IsSuccess)
            {
                return Unauthorized(new ApiResponse(401, userRoleResult.Error ?? "Failed to get user role"));
            }
            var userRole = userRoleResult.Value;
            // Generate a new refresh token
            var newRefreshTokenResult = await _authenticationService.CreateRefreshTokenAsync(user);
            if (!newRefreshTokenResult.IsSuccess)
            {
                return StatusCode(500, new ApiResponse(500, "Failed to generate refresh token"));
            }
            var newRefreshToken = newRefreshTokenResult.Value;

            // Generate a new access token
            var userAccessToken = _tokenService.CreateAccessToken(user, userRole);

            // Set the new refresh token cookie
            Response.AppendCookie("UserCookie", newRefreshToken, TimeUnit.Minutes, 1);

            // Return the new access token
            return Ok(new UserDto
            {
                Email = user.Email,
                Token = userAccessToken,
                DisplayName = user.DisplayName,
                Role = userRole
            });
        }

        [HttpPost]
        public async Task<IActionResult> Revoke()
        {
            var email = User.Identity.Name;

            var userResult = await _authenticationService.GetUserByEmailAsync(email);
            if (!userResult.IsSuccess) return BadRequest(new ApiResponse(400, userResult.Error ?? null));

            await _authenticationService.ClearRefreshTokenAsync(userResult.Value);

            return NoContent();
        }

    }
}
