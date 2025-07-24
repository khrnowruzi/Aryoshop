using Application.DTOs.Products;
using Core.Entities.Products;

namespace Application.Extensions.Products;

public static class ProductExtensions
{
    public static Product ToEntity(this ProductDto productDto)
    {
        return new Product
        {
            Name = productDto.Name,
            Brand = productDto.Brand,
            Model = productDto.Model,
            Description = productDto.Description,
            Price = productDto.Price,
            QuantityInStock = productDto.QuantityInStock,
            IsSale = productDto.IsSale,
            PictureUrl = productDto.PictureUrl,
            VideoUrl = productDto.VideoUrl
        };
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Brand = product.Brand,
            Model = product.Model,
            Description = product.Description,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock,
            IsSale = product.IsSale,
            PictureUrl = product.PictureUrl,
            VideoUrl = product.VideoUrl
        };
    }

    public static void UpdateFromDto(this Product product, ProductDto productDto)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        if (productDto == null) throw new ArgumentNullException(nameof(productDto));

        product.Name = productDto.Name;
        product.Brand = productDto.Brand;
        product.Model = productDto.Model;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.QuantityInStock = productDto.QuantityInStock;
        product.IsSale = productDto.IsSale;
        product.PictureUrl = productDto.PictureUrl;
        product.VideoUrl = productDto.VideoUrl;
    }
}
