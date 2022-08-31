using Core.Enitities;
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
    }
}