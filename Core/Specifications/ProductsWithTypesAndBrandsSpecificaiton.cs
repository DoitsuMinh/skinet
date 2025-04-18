using Core.Enitities;
using Core.Helpers;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecificaiton : BaseSpecification<Product>
    {
        //base: filtering brand and type
        public ProductsWithTypesAndBrandsSpecificaiton(ProductSpecParams productParams)
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower()
                    .Contains(productParams.Search)) &&
                (string.IsNullOrEmpty(productParams.BrandIds) || StringParser.ParseListIds(productParams.BrandIds).Contains(x.ProductBrandId)) &&
                (string.IsNullOrEmpty(productParams.TypeIds) || StringParser.ParseListIds(productParams.BrandIds).Contains(x.ProductTypeId))
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecificaiton(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}