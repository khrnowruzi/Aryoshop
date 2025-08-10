using Application.DTOs.Products;
using Application.Extensions.Products;
using Application.RequestHelpers;
using Application.Specification.Products;
using Domain.Entities.Products;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product, Guid> repo, ISlugService slugService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            var products = await repo.GetAllWithSpecificationAsync(spec);
            var count = await repo.CountAsync(spec);

            var pagination = new Pagination<ProductDto>(
                specParams.PageIndex,
                specParams.PageSize,
                count,
                [.. products.Select(p => p.ToDto())]
            );

            return Ok(pagination);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await repo.GetByIdAsync(id);

            if (product == null) return NotFound();

            return product.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            product.CreatedDate = DateTime.UtcNow;
            product.CreatedBy = "admin";

            product.Slug = slugService.GenerateSlug(
                string.Join(' ', [product.Name, product.Brand, product.Model]));

            repo.Add(product);

            if (await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            if (productDto.Id != id)
                return BadRequest("Cannot update this product");

            var product = await repo.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            product.UpdateFromDto(productDto);
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";
            product.Slug = slugService.GenerateSlug(
                string.Join(' ', [product.Name, product.Brand, product.Model]));

            repo.Update(product);

            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await repo.GetByIdAsync(id);

            if (product is null) return NotFound();

            product.IsDeleted = true;
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";

            repo.Update(product);

            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandsSpecification();

            return Ok(await repo.GetAllWithSpecificationAsync(spec));
        }

        [HttpGet("models")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetModels()
        {
            var spec = new ModelsSpecification();

            return Ok(await repo.GetAllWithSpecificationAsync(spec));
        }
    }
}
