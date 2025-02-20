using Core.Enitities.Identity;
using Insfrastructure.Data;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    /// <summary>
    /// Migrates and seeds the databases with retry logic and environment-aware configuration.
    /// </summary>
    /// <param name="host">The IHost instance providing service access.</param>
    /// <returns>A Task representing the async operation.</returns>
    /// <exception cref="AggregateException">Thrown if all retries fail.</exception>
    public static class HostExtensions
    {
        public static async Task MigrateAndSeedDatabase(this IHost host)
        {
            ArgumentNullException.ThrowIfNull(host, nameof(host));

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(MigrateAndSeedDatabase));
                
            try
            {
                logger.LogInformation("Starting database migration and seeding...");

                // Product Seeding
                var context = services.GetRequiredService<StoreContext>();
                await context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context, logger);

                // Identity Seeding
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();


                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.EnsureCreatedAsync();
                await identityContext.Database.MigrateAsync();

                await Task.WhenAll(
                    AppIdentityDbContextSeed.SeedRolesAsync(roleManager),
                    AppIdentityDbContextSeed.SeedUserAsync(userManager)
                );
                await AppIdentityDbContextSeed.SeedRoleClaimsAsync(roleManager);
                await AppIdentityDbContextSeed.SeedUserRolesAsync(userManager);
                await AppIdentityDbContextSeed.SeedUserClaimsAsync(userManager, roleManager);

                logger.LogInformation("Database migration and seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database migration and seeding");
                throw; // Re-throw to fail startup if critical
            }
            
        }
    }
}