using Core.Enitities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
             : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower()
                    .Contains(productParams.Search)) &&
                (string.IsNullOrEmpty(productParams.BrandIds) || x.ProductBrandId == 1 || x.ProductBrandId == 2) &&
                (string.IsNullOrEmpty(productParams.TypeIds) || x.ProductTypeId == 1 || x.ProductTypeId == 2)
            )
        {

        }
    }
}