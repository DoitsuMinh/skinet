using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Insfrastructure.Identity
{
    //public class AppIdentityDbContextSeed
    //{
    //    /// <summary>
    //    /// User role mappings
    //    /// </summary>
    //    private static readonly Dictionary<string, string> UserRoleMappings = new()
    //    {
    //        { "admin@skinetgroup.cc", "Admin" },
    //        { "spadmin@skinetgroup.cc", "SpAdmin" },
    //        { "customer@test.com", "Customer" }
    //    };

    //    /// <summary>
    //    /// Seed users
    //    /// </summary>
    //    /// <param name="userManager"></param>
    //    public static async Task SeedUserAsync(UserManager<AppUser> userManager)
    //    {
    //        if (userManager.Users.Any()) return;

    //        var hasher = new PasswordHasher<AppUser>();

    //        var customerUser = new AppUser()
    //        {
    //            //Id = 3,
    //            DisplayName = "Sir Customer",
    //            Email = "customer@test.com",
    //            UserName = "customer@test.com",
    //            Address = new Address
    //            {
    //                FirstName = "Customer",
    //                LastName = "Jr.",
    //                Street = "Le Van Luong",
    //                City = "HCM City"
    //            },
    //            EmailConfirmed = false,
    //        };
    //        customerUser.PasswordHash = hasher.HashPassword(customerUser, "cust@123");

    //        var adminUser = new AppUser()
    //        {
    //            //Id = 2,
    //            DisplayName = "Sir Admin",
    //            Email = "admin@skinetgroup.cc",
    //            UserName = "admin@skinetgroup.cc",
    //            Address = new Address
    //            {
    //                FirstName = "Admin",
    //                LastName = ".",
    //                Street = "",
    //                City = ""
    //            },
    //            EmailConfirmed = true,

    //        };
    //        adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin@123");

    //        var spAdminUser = new AppUser()
    //        {
    //            //Id = 1,
    //            DisplayName = "Sir SpAdmin",
    //            Email = "spadmin@skinetgroup.cc",
    //            UserName = "spadmin@skinetgroup.cc",
    //            Address = new Address
    //            {
    //                FirstName = "Sp Admin",
    //                LastName = ".",
    //                Street = "",
    //                City = ""
    //            },
    //            EmailConfirmed = true,

    //        };
    //        spAdminUser.PasswordHash = hasher.HashPassword(spAdminUser, "spadmin@123");

    //        await userManager.CreateAsync(spAdminUser);
    //        await userManager.CreateAsync(adminUser);
    //        await userManager.CreateAsync(customerUser);
    //    }

    //    /// <summary>
    //    /// Seed roles
    //    /// </summary>
    //    /// <param name="roleManager"></param>
    //    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    //    {
    //        if (roleManager.Roles.Any()) return;

    //        foreach (var mapping in UserRoleMappings)
    //        {
    //            if (!await roleManager.RoleExistsAsync(mapping.Value))
    //            {
    //                await roleManager.CreateAsync(new IdentityRole(mapping.Value));
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Seed role claims
    //    /// </summary>
    //    /// <param name="roleManager"></param>
    //    public static async Task SeedRoleClaimsAsync(RoleManager<IdentityRole> roleManager)
    //    {
    //        foreach (var mapping in UserRoleMappings)
    //        {
    //            var userRole = await roleManager.FindByNameAsync(mapping.Value);
    //            if (userRole == null) break;
                
    //            var userRoleClaims = await roleManager.GetClaimsAsync(userRole);
    //            if (userRoleClaims.Any()) break;
                
    //            if (userRole.NormalizedName == "SPADMIN")
    //            {
    //                await roleManager.AddClaimAsync(userRole, new Claim("Products", "Manage"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Products", "Delete"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Users", "Manage"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "Manage"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "View"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Settings", "Manage"));
    //            }
    //            else if (userRole.NormalizedName == "ADMIN")
    //            {
    //                await roleManager.AddClaimAsync(userRole, new Claim("Products", "Create"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Products", "Edit"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "Manage"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "View"));
    //            }
    //            else if (userRole.NormalizedName == "CUSTOMER")
    //            {
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "View"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Orders", "Create"));
    //                await roleManager.AddClaimAsync(userRole, new Claim("Profile", "Edit"));
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Seed user roles
    //    /// </summary>
    //    /// <param name="userManager"></param>
    //    public static async Task SeedUserRolesAsync(UserManager<AppUser> userManager)
    //    {
    //        foreach (var mapping in UserRoleMappings)
    //        {

    //            var user = await userManager.FindByEmailAsync(mapping.Key);
    //            if (user != null)
    //            {
    //                var userRole = await userManager.GetRolesAsync(user);
    //                if (!userRole.Any())
    //                {
    //                    var result = await userManager.AddToRoleAsync(user, mapping.Value);
    //                    if (!result.Succeeded)
    //                    {
    //                        throw new InvalidOperationException($"Failed to add {mapping.Value} to {mapping.Key}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Seed user claims
    //    /// </summary>
    //    /// <param name="userManager"></param>
    //    /// <param name="roleManager"></param>
    //    public static async Task SeedUserClaimsAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        foreach (var mapping in UserRoleMappings)
    //        {
    //            var user = await userManager.FindByEmailAsync(mapping.Key);
    //            if (user != null)
    //            {
    //                var userClaims = await userManager.GetClaimsAsync(user);
    //                if (!userClaims.Any())
    //                {
    //                    var role = await roleManager.FindByNameAsync(mapping.Value);
    //                    var roleClaims = await roleManager.GetClaimsAsync(role);

    //                    var result = await userManager.AddClaimsAsync(user, roleClaims);
    //                    if (!result.Succeeded)
    //                    {
    //                        throw new InvalidOperationException($"Failed to add claims to {mapping.Value}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //                    }
    //                }
                   
    //            }
    //        }
    //    }
    //}
}