using Application.Specification.Base;
using Core.Entities.Products;

namespace Application.Specification.Products;

public class BrandsSpecification : BaseSpecification<Product, string>
{
    public BrandsSpecification() : base(null)
    {
        AddSelect(x => x.Brand);
        AddDistinct();
    }
}
