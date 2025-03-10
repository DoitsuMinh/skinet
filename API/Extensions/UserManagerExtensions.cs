using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByUserByClaimsPrincipleWithAddressAsync(this UserManager<AppUser>
            input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<(AppUser, string userRole)> FindByEmailFromClaimsPrinciple(this UserManager<AppUser>
            input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            var appUser = await input.Users.SingleOrDefaultAsync(x => x.Email == email);
            var role = await input.GetRolesAsync(appUser);
            return (appUser, role[0]);
        }
    }
}