using Core.Enitities;

namespace Core.Specifications
{
    public class ProductsWithProductNameSpecification : BaseSpecification<Product>
    {
        public ProductsWithProductNameSpecification(string productName) : base(x => x.Name.ToLower().Contains(productName.ToLower()))
        {
            AddOrderBy(x => x.Name);
        }
    }
}
