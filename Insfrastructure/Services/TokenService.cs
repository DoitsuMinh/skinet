using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Insfrastructure.Services 
{
    /// <summary>
    /// DOC:
    /// https://dev.to/cotter/localstorage-vs-cookies-all-you-need-to-know-about-storing-jwt-tokens-securely-in-the-front-end-15id
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateToken(AppUser user,string userRole)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.GivenName, user.DisplayName),
                new (ClaimTypes.Role, userRole)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //Token is going to be valid if its before expire day and issued by Issuer
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates an access token including user role, email, and display name.
        /// The token is valid for 30 minutes.
        /// </summary>
        /// <param name="user">The AppUser object containing user details.</param>
        /// <param name="userRole">The role of the user to include in the token.</param>
        public string CreateAccessToken(AppUser user, string userRole)
        {
            // Define claims to include in the token
            var claims = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.GivenName, user.DisplayName),
                new (ClaimTypes.Role, userRole)
            };

            // Create signing credentials using the secret key and HMAC-SHA512 algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Configure the token descriptor with claims, expiration, credentials, and issuer
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            // Initialize JWT handler to create and serialize the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates a refresh token including user role, email, and display name.
        /// The token is valid for 1 day.
        /// </summary>
        /// <param name="user"></param>
        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}