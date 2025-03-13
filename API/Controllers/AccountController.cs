using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ITokenService tokenService, IMapper mapper, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        //[Authorize(AuthenticationSchemes ="bearer")]
        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetCurrentUser()
        //{
        //    var (user, userRole) = await _userManager.FindByEmailFromClaimsPrinciple(User);

        //    return Ok(new UserDto
        //    {
        //        Email = user.Email,
        //        DisplayName = user.DisplayName,
        //        Role = userRole
        //    });
        //}

        //[HttpGet("emailexists")]
        //public async Task<ActionResult<bool>> CheckEmailExistAsync([FromQuery] string email)
        //{
        //    return await _userManager.FindByEmailAsync(email) != null;
        //}

        //[Authorize(AuthenticationSchemes = "bearer")]
        //[HttpGet("address")]
        //public async Task<ActionResult<AddressDto>> GetUserAddress()
        //{
        //    var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User);

        //    return _mapper.Map<Address, AddressDto>(user.Address);
        //}

        //[Authorize]
        //[HttpPut("address")]
        //public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        //{
        //    var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

        //    user.Address = _mapper.Map<AddressDto, Address>(address);

        //    var result = await _userManager.UpdateAsync(user);
        //    if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        //    return BadRequest("Proplem updating the user");
        //}

        /// <summary>
        /// Authenticates a user and returns their details with an access token.
        /// </summary>
        /// <param name="loginDto">The user's login credentials (email and password).</param>
        /// <response code="200">Login successful, returns user details.</response>
        /// <response code="401">Invalid credentials or missing user role.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return BadRequest(new ApiResponse(400));

            var result = await _signInManager.UserManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return BadRequest(new ApiResponse(400));

            var token = _authenticationService.GenerateAccessToken(user, "Customer");

            return Ok(new TokenDto { AccessToken = token });
        }

        /// <summary>
        /// Registers a new customer and returns their details with an access token.
        /// </summary>
        /// <param name="registerDto"></param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> CustomerRegister([FromBody] RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _signInManager.UserManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            var tokenResult = await _authenticationService.CreateRefreshTokenAsync(user);

            if (!tokenResult.IsSuccess)
            {
                return BadRequest(tokenResult.Error);
            }

            return Created();
        }

        [Authorize(AuthenticationSchemes = "bearer")]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var result = await _signInManager.UserManager.RemoveAuthenticationTokenAsync(user, "WebBrowser", "RefreshToken");
            if (!result.Succeeded)
            {
                
            }

            await _signInManager.SignOutAsync();            

            return NoContent();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();

            var user = await _signInManager.UserManager.GetUserByEmailWithAddressAsync(User);

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address = _mapper.Map<AddressDto>(user.Address)
            });
        }

        [HttpGet]
        public ActionResult GetAuthState()
        {
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<AddressDto>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await _signInManager.UserManager.GetUserByEmailWithAddressAsync(User);

            if (user.Address is null)
            {
                user.Address = _mapper.Map<Address>(addressDto);
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result = await _signInManager.UserManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Updating address failed"));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
    }
}