namespace Core.Enitities.OrderAggregate
{
    public class Order: BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public required string BuyerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public PaymentSummary PaymentSummary { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; } = [];
        public double Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required string PaymentIntentId { get; set; }
    }
}
