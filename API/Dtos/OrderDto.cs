using Core.Enitities.OrderAggregate;
using Core.Enitities;

namespace API.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string BuyerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public required string DeliveryMethod { get; set; }
        public required double ShippingPrice { get; set; }
        public required PaymentSummary PaymentSummary { get; set; }
        public required List<OrderItemDto> OrderItems { get; set; }
        public double Subtotal { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public required string Status { get; set; }
        public required string PaymentIntentId { get; set; }
    }
}
