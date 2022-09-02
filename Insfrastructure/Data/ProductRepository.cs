using Core.Enitities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;

        }

        public async Task<IReadOnlyList<Product>> GetProductAsync()
        {
            // var typeId = 1;
            // var products = _context.Products
            //     .Where(c => c.ProductTypeId == typeId).Include(x => x.ProductType).ToListAsync();

            return await _context.Products
                .Include(c => c.ProductBrand)
                .Include(c => c.ProductType)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(c => c.ProductBrand)
                .Include(c => c.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}