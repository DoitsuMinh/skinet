using Core.Enitities.Identity;
using Insfrastructure.Data;
using Insfrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<StoreContext>();


            //var builder = services.AddIdentity<AppUser, IdentityRole>();

            //builder = new IdentityBuilder(builder.UserType, builder.Services);
            //builder.AddRoles<IdentityRole>();
            //builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            //builder.AddDefaultTokenProviders();
            //builder.AddSignInManager();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //});

           
            return services;
        }
    }
}