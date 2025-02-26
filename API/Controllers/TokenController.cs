using Core.Enitities.Identity;
using Core.Enitities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Errors;
using Microsoft.AspNetCore.Identity;
using API.Dtos;

namespace API.Controllers
{
    [ApiController]
    public class TokenController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;

        public TokenController(ITokenService tokenService, IAuthenticationService authenticationService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<UserDto>> Refresh(UserDto userDto)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(userDto.Token);
            var email = principal.Identity.Name; //this is mapped to the Name claim by default

            var userResult = await _authenticationService.GetUserByEmailAsync(email);
            //var userRoleResult = await _authenticationService.GetUserRoleAsync(userResult.Value);
            var refreshTokenResult = await _authenticationService.CreateRefreshTokenAsync(userResult.Value);

            return Ok(new UserDto
            {
                Email = userDto.Email,
                DisplayName = userDto.DisplayName,
                Role = userDto.Role,
                RefreshToken = refreshTokenResult.Value,
                Token = _tokenService.CreateAccessToken(userResult.Value, userDto.Role)
            });
        }

        [Authorize]
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
