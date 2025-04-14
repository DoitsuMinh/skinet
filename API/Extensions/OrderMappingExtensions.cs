using API.Dtos;
using Core.Enitities.OrderAggregate;

namespace API.Extensions
{
    public static class OrderMappingExtensions
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                PaymentSummary = order.PaymentSummary,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderItems = order.OrderItems.Select(x => x.toDto()).ToList(),
                Subtotal = order.Subtotal,
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId,
                DeliveryMethod = order.DeliveryMethod.Description,
            };
        }

        public static OrderItemDto toDto(this OrderItem order)
        {
            return new OrderItemDto
            {
                ProductId = order.Id,
                PictureUrl = "",
                ProductName = "order.ProductName",
                Price = order.Price,
                Quantity = order.Quantity
            };
        }
    }
}
