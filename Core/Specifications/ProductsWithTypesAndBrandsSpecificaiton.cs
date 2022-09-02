using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Enitities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecificaiton : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecificaiton()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecificaiton(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}