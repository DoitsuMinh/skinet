namespace Core.Enitities.OrderAggregate
{
    public class OrderItem: BaseEntity
    {
        public ProductionItemOrdered ItemOrdered { get; set; } = null!;
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
