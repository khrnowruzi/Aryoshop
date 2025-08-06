using Application.Specification.Base;
using Core.Entities.Products;

namespace Application.Specification.Products;

public class ModelsSpecification : BaseSpecification<Product, string>
{
    public ModelsSpecification() : base(null)
    {
        AddSelect(x => x.Model);
        AddDistinct();
    }
}