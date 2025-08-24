using System.Text.Json;
using Domain.Entities.Carts;
using Domain.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteCartAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<Cart?> GetCartAsync(string key)
    {
        var cart = await _database.StringGetAsync(key);

        return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart!);
    }

    public async Task<Cart?> SetCartAsync(Cart cart)
    {
        var createdCart = await _database.StringSetAsync(
                cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(20));

        return createdCart ? await GetCartAsync(cart.Id) : null;
    }
}
