using System.Text.Json;
using Core.Enitities;
using Microsoft.Extensions.Logging;

namespace Insfrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsyn(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                //create product brand in db
                if (!context.ProductBrands.Any())
                {
                    var brandData = File.ReadAllText("../Insfrastructure/Data/SeedData/brands.json");

                    //deserialize json to brand obj
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                    //add to db via context
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                //create product type in db
                if (!context.ProductTypes.Any())
                {
                    var typeData = File.ReadAllText("../Insfrastructure/Data/SeedData/types.json");

                    //deserialize json to brand obj
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                    //add to db via context
                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                //create product in db
                if (!context.Products.Any())
                {
                    var productData = File.ReadAllText("../Insfrastructure/Data/SeedData/products.json");

                    //deserialize json to brand obj
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    //add to db via context
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}