using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    [Required]
    public string Brand { get; set; } = string.Empty;
    [Required]
    public string Model { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public bool IsSale { get; set; }
    [Required]
    public string PictureUrl { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
}
