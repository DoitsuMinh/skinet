using Insfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class SecretServiceExtentions
    {
        public static IServiceCollection GetSecretConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get connection string from environment variable first
            string connectionString = string.Empty;

            try
            {
                connectionString = Environment.GetEnvironmentVariable("Skinet_SQLiteConn");
                //var envDicts = Environment.GetEnvironmentVariables();
            }
            catch (Exception ex)
            {
                Console.WriteLine("***************ERROR***************");
                Console.WriteLine("Failed to get enviroment var!");
                throw new Exception(ex.Message);
            }
            
            // Fall back to configuration if not found in environment
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            // Add database context
            services.AddDbContext<StoreContext>(
                options => options.UseSqlite(connectionString)
            );
            // Register a connection provider service
            //services.AddSingleton<IConnectionProvider>(new ConnectionProvider(connectionString));

            // Add other services...

            return services;
        }
    }
}
