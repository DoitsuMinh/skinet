using System.Reflection;
using Core.Enitities;
using Insfrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        //allow to querry entities and retrive data from db
        public DbSet<Product> Products{ get;set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        //overide method inside dbcontext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }
    }
}