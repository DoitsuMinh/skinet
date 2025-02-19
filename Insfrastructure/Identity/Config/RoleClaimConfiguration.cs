//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Insfrastructure.Identity.Config
//{
//    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
//    {
//        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
//        {
//            var adminClaim = new List<IdentityRoleClaim<string>>([
//                    new () {
//                        Id = 1, RoleId = "admin", ClaimType = "Permission", ClaimValue = "CanManageUsers"
//                    },
//                    new () {
//                        Id = 2, RoleId = "admin", ClaimType = "Permission", ClaimValue = "CanEdit"
//                    }
//                ]);
//            var spAdminClaim = new IdentityRoleClaim<string>
//            {
//                Id = 3,
//                RoleId = "spadmin",
//                ClaimType = "Permission",
//                ClaimValue = "CanEdit"
//            };
//            var customerClaim = new IdentityRoleClaim<string>
//            {
//                Id = 4,
//                RoleId = "customer",
//                ClaimType = "Permission",
//                ClaimValue = "CanViewOrders"
//            };

//            builder.HasData(adminClaim, spAdminClaim, customerClaim);
//        }
//    }
//}
