using Core.Enitities.Identity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Insfrastructure.Services
{
    /// <summary>
    /// DOC:
    /// https://dev.to/cotter/localstorage-vs-cookies-all-you-need-to-know-about-storing-jwt-tokens-securely-in-the-front-end-15id
    /// </summary>
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateToken(AppUser user, string userRole)
        {
            var secretKey = configuration["Token:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            //Token is going to be valid if its before expire day and issued by Issuer
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, userRole)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Token:ExpirationInMinutes"])),
                SigningCredentials = creds,
                Issuer = configuration["Token:Issuer"],
                Audience = configuration["Token:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates an access token including user role, email.
        /// The token is valid for 30 minutes.
        /// </summary>
        /// <param name="user">The AppUser object containing user details.</param>
        /// <param name="userRole">The role of the user to include in the token.</param>
        public string CreateAccessToken(AppUser user, string userRole)
        {
            var secretKey = configuration["Token:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            //Token is going to be valid if its before expire day and issued by Issuer
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, userRole)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Token:ExpirationInMinutes"])),
                SigningCredentials = creds,
                Issuer = configuration["Token:Issuer"],
                Audience = configuration["Token:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a random refresh token.
        /// </summary>
        /// <param name="user"></param>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}