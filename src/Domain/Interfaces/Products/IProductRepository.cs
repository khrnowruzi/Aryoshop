using Domain.Entities.Products;

namespace Domain.Interfaces.Products;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? model, string? sort);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetModelsAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<bool> SaveChangeAsync();
}
