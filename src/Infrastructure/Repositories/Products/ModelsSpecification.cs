using Application.Specification.Base;
using Core.Entities.Products;

namespace Infrastructure.Repositories.Products;

public class ModelsSpecification : Specification<Product, string>
{
    public ModelsSpecification() : base(null)
    {
        AddSelect(x => x.Model);
        AddDistinct();
    }
}