//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Insfrastructure.Identity.Config
//{
//    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
//    {
//        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
//        {
//            // Seed user roles
//            var userAdmin = new IdentityUserRole<string> { UserId = "user-admin", RoleId = "role-admin" };
//            var userSpAdmin = new IdentityUserRole<string> { UserId = "user-spadmin", RoleId = "role-spadmin" };
//            var userCustomer = new IdentityUserRole<string> { UserId = "user-customer", RoleId = "role-customer" };
//            builder.HasData(userAdmin, userSpAdmin, userCustomer);
//        }
//    }
//}
