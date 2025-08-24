using Domain.Entities.Carts;

namespace Domain.Interfaces;

public interface ICartService
{
    Task<Cart?> GetCartAsync(string key);
    Task<Cart?> SetCartAsync(Cart cart);
    Task<bool> DeleteCartAsync(string key);
}
