using Core.Enitities.Identity;
using Insfrastructure.Data;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class HostExtensions
    {
        public static async Task MigrateAndSeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    // Product Seeding
                    var context = services.GetRequiredService<StoreContext>();

                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsyn(context, loggerFactory);

                    // Identity Seeding
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.EnsureCreatedAsync();
                    await identityContext.Database.MigrateAsync();

                    await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);
                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                    await AppIdentityDbContextSeed.SeedRoleClaimsAsync(roleManager);
                    await AppIdentityDbContextSeed.SeedUserRolesAsync(userManager);                    

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
        }
    }
}