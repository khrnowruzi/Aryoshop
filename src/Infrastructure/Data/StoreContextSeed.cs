using System.Text.Json;
using Application.DTOs.Products;
using Application.Extensions.Products;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ISlugService slugService)
    {
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var productsDto = JsonSerializer.Deserialize<List<ProductDto>>(productsData);

            if (productsDto is null) return;

            var products = productsDto.Select(p =>
            {
                var product = p.ToEntity();
                product.CreatedDate = DateTime.UtcNow;
                product.CreatedBy = "admin";
                product.Slug = slugService.GenerateSlug(
                    string.Join(' ', [product.Name, product.Brand, product.Model]));

                return product;
            });

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
