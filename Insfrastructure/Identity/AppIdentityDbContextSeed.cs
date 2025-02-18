using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Insfrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser() 
                {
                    DisplayName = "Jason",
                    Email = "jason@test.com",
                    UserName = "jason@test.com",
                    Address = new Address
                    {
                        FirstName = "Jason",
                        LastName = "Vu",
                        Street = ".",
                        City = "HCM City"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}