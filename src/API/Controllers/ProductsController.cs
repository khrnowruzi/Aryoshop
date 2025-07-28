using System.Threading.Tasks;
using Application.DTOs.Products;
using Application.Extensions.Products;
using Application.Interfaces;
using Application.Interfaces.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repo, ISlugService slugService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(
            string? brand,
            string? model,
            string? sort
            )
        {
            var products = await repo.GetProductsAsync(brand, model, sort);

            return products.Select(p => p.ToDto()).ToList();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await repo.GetProductByIdAsync(id);

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

            repo.AddProduct(product);

            if (await repo.SaveChangeAsync())
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

            var product = await repo.GetProductByIdAsync(id);
            if (product is null)
                return NotFound();

            product.UpdateFromDto(productDto);
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";
            product.Slug = slugService.GenerateSlug(
                string.Join(' ', [product.Name, product.Brand, product.Model]));

            repo.UpdateProduct(product);

            if (await repo.SaveChangeAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product is null) return NotFound();

            product.IsDeleted = true;
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";

            repo.UpdateProduct(product);

            if (await repo.SaveChangeAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await repo.GetBrandsAsync());
        }

        [HttpGet("models")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetModels()
        {
            return Ok(await repo.GetModelsAsync());
        }
    }
}
