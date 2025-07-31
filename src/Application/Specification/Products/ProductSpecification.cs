using Application.Specification.Base;
using Core.Entities.Products;

namespace Application.Specification.Products;

public class ProductSpecification : Specification<Product>
{
    public ProductSpecification(string? brand, string? model, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
        (string.IsNullOrWhiteSpace(model) || x.Model == model))
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
