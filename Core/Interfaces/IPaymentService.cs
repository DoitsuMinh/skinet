﻿using Core.Enitities;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
        
    }
}
