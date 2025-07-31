using Application.Specification.Base;
using Core.Entities.Products;

namespace Infrastructure.Repositories.Products;

public class BrandsSpecification : Specification<Product, string>
{
    public BrandsSpecification() : base(null)
    {
        AddSelect(x => x.Brand);
        AddDistinct();
    }
}
