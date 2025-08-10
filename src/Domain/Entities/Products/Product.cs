using Domain.Entities.Base;

namespace Domain.Entities.Products;

public class Product : AuditableEntity<Guid>
{
    public required string Name { get; set; }
    public string Slug { get; set; } = default!;
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public bool IsSale { get; set; }
    public required string PictureUrl { get; set; }
    public string? VideoUrl { get; set; }
}