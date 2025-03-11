using Core.Enitities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Insfrastructure.Infrastructures
{
    public class TokenProvider (IConfiguration configuration)
    {
        /// <summary>
        /// Creates an access token including user role, email, and display name.
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
                    new Claim(ClaimTypes.GivenName, user.Email),
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
    }
}
