using Core.Enitities;
using Core.Helpers;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
             : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower()
                    .Contains(productParams.Search)) &&
                (string.IsNullOrEmpty(productParams.BrandIds) || StringParser.ParseListIds(productParams.BrandIds).Contains(x.ProductBrandId)) &&
                (string.IsNullOrEmpty(productParams.TypeIds) || StringParser.ParseListIds(productParams.BrandIds).Contains(x.ProductTypeId))
            )
        {

        }
    }
}