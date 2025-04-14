using API.Helpers;
using Insfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class SecretServiceExtentions
    {
        public static IServiceCollection GetSecretConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get connection string from environment variable first
            string dbConnectionString = string.Empty;

            //try
            //{
            //    connectionString = Environment.GetEnvironmentVariable("Skinet_SQLiteConn");
            //    //var envDicts = Environment.GetEnvironmentVariables();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("***************ERROR***************");
            //    Console.WriteLine("Failed to get enviroment var!");
            //    throw new Exception(ex.Message);
            //}
            
            // Fall back to configuration if not found in environment
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                var encryptedConnectionString = configuration.GetConnectionString("DefaultConnection");
                dbConnectionString = AesEncryptionHelper.Decrypt(encryptedConnectionString);
                //connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            if (string.IsNullOrEmpty(dbConnectionString))
            {
                throw new ArgumentNullException(nameof(dbConnectionString));
            }

            // Add database context
            services.AddDbContext<StoreContext>(
                options => options.UseSqlite(dbConnectionString)
            );
            // Register a connection provider service
            //services.AddSingleton<IConnectionProvider>(new ConnectionProvider(connectionString));

            var redisConnectionString = string.Empty;

            if (string.IsNullOrEmpty(redisConnectionString))
            {
                var encryptedConnectionString = configuration.GetConnectionString("Redis");
                redisConnectionString = AesEncryptionHelper.Decrypt(encryptedConnectionString);
            }

            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentNullException(nameof(redisConnectionString));
            }

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            return services;
        }
    }
}
