namespace Core.Enitities
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public int BrandID { get; set; }
        public int TypeID { get; set; }

    }
}
