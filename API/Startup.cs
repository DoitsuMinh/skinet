using Microsoft.OpenApi.Models;
using Insfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Enitities;
using API.Helpers;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StoreContext>(c =>
                c.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.AddAutoMapper(typeof(MappingProfiles));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
