namespace Core.Enitities
{
    public class Product : BaseEntity
    {

        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public required string PictureUrl { get; set; }
        public  ProductType ProductType { get; set; }
        public required int ProductTypeId { get; set; }
        public  ProductBrand ProductBrand { get; set; }
        public required int ProductBrandId { get; set; }
        public int QuantityInStock { get; set; }
    }
}