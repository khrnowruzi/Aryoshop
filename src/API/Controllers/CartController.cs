using Domain.Entities.Carts;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Cart>> GetCart(string id)
        {
            var createdCart = await cartService.GetCartAsync(id);

            return Ok(createdCart ?? new Cart { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> UpdateCart(Cart cart)
        {
            var updatedCart = await cartService.SetCartAsync(cart);

            return Ok(updatedCart == null ? BadRequest("Problem with cart") : updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var deletedCartResult = await cartService.DeleteCartAsync(id);

            return !deletedCartResult ? BadRequest("Problem deleting cart") : Ok();
        }
    }
}
