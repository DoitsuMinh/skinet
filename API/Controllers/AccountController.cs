using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(AuthenticationSchemes ="bearer")]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var (user, userRole) = await _userManager.FindByEmailFromClaimsPrinciple(User);

            return Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Role = userRole
            });
        }

        //[HttpGet("emailexists")]
        //public async Task<ActionResult<bool>> CheckEmailExistAsync([FromQuery] string email)
        //{
        //    return await _userManager.FindByEmailAsync(email) != null;
        //}

        [Authorize(AuthenticationSchemes = "bearer")]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

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
            var userResult = await _authenticationService.LoginUserAsync(loginDto.Email, loginDto.Password);
            if (!userResult.IsSuccess)
            {
                return Unauthorized(new ApiResponse(401, userResult.Error ?? null));

            }
            return Ok();
        }

        /// <summary>
        /// Registers a new customer and returns their details with an access token.
        /// </summary>
        /// <param name="registerDto"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> CustomerRegister([FromBody] RegisterDto registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
            };

            var registerResult = await _authenticationService.RegisterByPassAsync(user, registerDto.Password, registerDto.Role);

            if (!registerResult.IsSuccess) return BadRequest(new ApiResponse(400, registerResult.Error
                                                                                  ?? null));

            return Created();
        }

    }
}