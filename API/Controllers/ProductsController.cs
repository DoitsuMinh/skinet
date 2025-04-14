using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Enitities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes ="bearer")]
    [Route("api/[controller]")]
    public class ProductsController(IUnitOfWork _uow, IMapper _mapper) : BaseApiController
    {

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecificaiton(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _uow.Repository<Product>().CountAsync(countSpec);

            var products = await _uow.Repository<Product>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecificaiton(id);

            var product = await _uow.Repository<Product>().GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _uow.Repository<ProductBrand>().ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _uow.Repository<ProductType>().ListAllAsync());
        }

        [HttpGet("filtered-products")]
        public async Task<ActionResult<IReadOnlyList<ProductDictDto>>> GetProductsBySearchValue(string searchValue = "")
        {
            var spec = new ProductsWithProductNameSpecification(searchValue);
            var products = await _uow.Repository<Product>().ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<ProductDictDto>>(products));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _uow.Repository<Product>().Add(product);
            if(await _uow.Complete())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem at create product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product");
            }

            _uow.Repository<Product>().Update(product);

            if(await _uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem at update product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DelteeProduct(int id)
        {
            var product = await _uow.Repository<Product>().GetByIdAsync(id);
            if (product is null) return NotFound();

            _uow.Repository<Product>().Remove(product);

            if (await _uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem at delete product");
        }

        private bool ProductExists(int id)
        {
            return _uow.Repository<Product>().Exists(id);
        }
    }
}