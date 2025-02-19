using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Insfrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var hasher = new PasswordHasher<AppUser>();

                var customerUser = new AppUser() 
                {
                    Id = "3",
                    DisplayName = "Sir Customer",
                    Email = "customer@test.com",
                    UserName = "customer@test.com",
                    Address = new Address
                    {
                        FirstName = "Customer",
                        LastName = "Jr.",
                        Street = "Le Van Luong",
                        City = "HCM City"
                    },
                    EmailConfirmed = false,
                    
                };
                customerUser.PasswordHash = hasher.HashPassword(customerUser, "cust@123");
                await userManager.CreateAsync(customerUser);

                var adminUser = new AppUser()
                {
                    Id = "1",
                    DisplayName = "Sir Admin",
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = ".",
                        Street = "",
                        City = ""
                    },
                    EmailConfirmed = true,

                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin@123");
                await userManager.CreateAsync(adminUser);

                var spAdminUser = new AppUser()
                {
                    Id = "2",
                    DisplayName = "Sir SpAdmin",
                    Email = "spadmin@test.com",
                    UserName = "spadmin@test.com",
                    Address = new Address
                    {
                        FirstName = "Sp Admin",
                        LastName = ".",
                        Street = "",
                        City = ""
                    },
                    EmailConfirmed = true,

                };
                spAdminUser.PasswordHash = hasher.HashPassword(spAdminUser, "spadmin@123");
                await userManager.CreateAsync(spAdminUser);
            }
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = ["Admin", "SpAdmin", "Customer"];
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedRoleClaimsAsync(RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "CanManageUsers"));
                await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "CanEdit"));
            }

            var spAdminRole = await roleManager.FindByNameAsync("SpAdmin");
            if (spAdminRole != null)
            {
                await roleManager.AddClaimAsync(spAdminRole, new Claim("Permission", "CanEdit"));
            }

            var customerRole = await roleManager.FindByNameAsync("Customer");
            if (customerRole != null)
            {
                await roleManager.AddClaimAsync(customerRole, new Claim("Permission", "CanViewOrders"));
            }
        }

        public static async Task SeedUserRolesAsync(UserManager<AppUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@test.com");
            if (adminUser != null)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var spAdminUser = await userManager.FindByEmailAsync("spadmin@test.com");
            if (spAdminUser != null)
            {
                await userManager.AddToRoleAsync(spAdminUser, "SpAdmin");
            }

            var customerUser = await userManager.FindByEmailAsync("customer@test.com");
            if (customerUser != null)
            {
                await userManager.AddToRoleAsync(customerUser, "Customer");
            }
        }
    }
}