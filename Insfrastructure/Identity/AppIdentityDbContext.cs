using Core.Enitities;
using Core.Enitities.Identity;
using Insfrastructure.Identity.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
/* DOC:
 * .NET 9 ASP.NET Core Identity - Identity model customization in ASP.NET Core
 * https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0
 */
namespace Insfrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        /// <summary>
        /// This method is called when the model for a derived context has been initialized 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base class implementation
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RefreshTokenConfiguration).Assembly);
        }
    }
}