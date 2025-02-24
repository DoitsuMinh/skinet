namespace API.Extensions
{
    public static class CookieServiceExtensions
    {
        public static IServiceCollection AddCookieServices(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "skinet.AuthCookie";
                options.Cookie.HttpOnly = true; // Prevent JS access
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
                options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1); // Persistent for 1 min
                options.SlidingExpiration = true; // Refresh expiry on activity
                options.LoginPath = "/api/account/login";
                options.LogoutPath = "/api/account/logout";
            });
            
            return services;
        }
    }
}
