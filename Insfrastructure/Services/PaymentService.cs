using Core.Enitities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Enitities.Product;

namespace Insfrastructure.Services
{
    public class PaymentService(
        IConfiguration config,
        ICartService cartService,
        IGenericRepository<Product> productRepo,
        IGenericRepository<DeliveryMethod> dmRepo
        ) : IPaymentService
    {
        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var cart = await cartService.GetCartAsync(cartId);
            if (cart == null) return null;

            var shippingPrice = (double)0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await dmRepo.GetByIdAsync((int)cart.DeliveryMethodId);
                if (deliveryMethod is null) return null;

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Items)
            {
                var productItem = await productRepo.GetByIdAsync(item.ProductId);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent? intent = null;

            if(string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var amt = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100;
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                intent = await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await cartService.SetCartAsync(cart);
            return cart;
        }
    }
}
