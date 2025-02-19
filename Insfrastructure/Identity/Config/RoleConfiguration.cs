//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Insfrastructure.Identity.Config
//{
//    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
//    {
//        public void Configure(EntityTypeBuilder<IdentityRole> builder)
//        {
//            // Seed roles
//            var spAdminRole = new IdentityRole("SpAdmin") { Id = "admin", NormalizedName= "SPADMIN" };
//            var adminRole = new IdentityRole("Admin") { Id = "admin", NormalizedName = "ADMIN" };
//            var customerRole = new IdentityRole("Customer") { Id = "customer", NormalizedName="CUSTOMER" };

//            builder.HasData(adminRole, spAdminRole, customerRole);
//        }
//    }
//}
