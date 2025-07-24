using Application.DTOs.Products;
using Application.Extensions.Products;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StoreContext context, ISlugService slugService) : ControllerBase
    {
        private readonly StoreContext _context = context;
        private readonly ISlugService _slugService = slugService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _context.Products.Select(p => p.ToDto()).ToListAsync();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var product = productDto.ToEntity();

            product.CreatedDate = DateTime.UtcNow;
            product.CreatedBy = "admin";

            product.Slug = _slugService.GenerateSlug(
                string.Join(' ', [product.Name, product.Brand, product.Model]));

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.ToDto();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            if (productDto.Id != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            product.UpdateFromDto(productDto);
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";
            product.Slug = _slugService.GenerateSlug(
                string.Join(' ', [product.Name, product.Brand, product.Model]));

            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            product.IsDeleted = true;
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = "admin";

            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(p => p.Id == id);
        }
    }
}
