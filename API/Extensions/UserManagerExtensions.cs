using System.Security.Authentication;
using System.Security.Claims;
using API.Dtos;
using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> GetUserByEmailAsync(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (userToReturn is null) throw new AuthenticationException("User not found");

            return userToReturn;
        }

        public static async Task<AppUser> GetUserByEmailWithAddressAsync(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (userToReturn is null) throw new AuthenticationException("User not found");

            return userToReturn;
        }

        public static string GetEmail(this UserManager<AppUser>
            userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Email claim not found");

            return email;
        }      
    }
}