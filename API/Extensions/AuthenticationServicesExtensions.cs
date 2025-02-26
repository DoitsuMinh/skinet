using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class AuthenticationServicesExtensions
    {
        public static IServiceCollection AddJWTAuthenticationServices(this IServiceCollection services,
          IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidAudience = config["Token:Audience"]
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        else
                        {
                            context.Response.Headers.Append("Token-Error", context.Exception.GetType().ToString());
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
