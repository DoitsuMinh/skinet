using Core.Enitities;
using Core.Interfaces;

namespace Insfrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            throw new NotImplementedException();
        }
    }
}
