using Core.Enitities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
/* DOC
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

        // This method is used to configure the model that was discovered by convention from the entity types
        // exposed in DbSet<TEntity> properties on your derived context. The base implementation of this method
        // does nothing, but it can be overridden in a derived class such that the model can be further configured
        // before it is locked down.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base class implementation
            base.OnModelCreating(modelBuilder);

        }
    }
}