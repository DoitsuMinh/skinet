using System.Reflection;
using System.Text.Json;
using Core.Enitities;
using Microsoft.Extensions.Logging;

namespace Insfrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILogger logger)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                //create product brand in db
                if (!context.ProductBrands.Any())
                {
                    var brandData = File
                        .ReadAllText(path + @"/Data/SeedData/brands.json");

                    //deserialize json to brand obj
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                    //add to db via context
                    context.ProductBrands.AddRange(brands);

                    await context.SaveChangesAsync();
                }

                //create product type in db
                if (!context.ProductTypes.Any())
                {
                    var typeData = File
                        .ReadAllText(path + @"/Data/SeedData/types.json");

                    //deserialize json to brand obj
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                    //add to db via context
                    context.ProductTypes.AddRange(types);

                    await context.SaveChangesAsync();
                }

                //create product in db
                if (!context.Products.Any())
                {
                    var productData = File
                        .ReadAllText(path + @"/Data/SeedData/products.json");

                    //deserialize json to brand obj
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    //add to db via context
                    context.Products.AddRange(products);

                    await context.SaveChangesAsync();
                }

                //create delivery method in db
                if (!context.DeliveryMethods.Any())
                {
                    var deliveryMethodData = File.ReadAllText("../Insfrastructure/Data/SeedData/delivery.json");

                    //deserialize json to brand obj
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

                    //add to db via context
                    context.DeliveryMethods.AddRange(deliveryMethods);

                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}