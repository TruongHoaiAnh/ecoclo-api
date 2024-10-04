using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepo _shoppingCart;
        public ShoppingCartController(IShoppingCartRepo shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }
        [HttpGet]
        public async Task<IActionResult> GetListCartItem()
        {
            var result = await _shoppingCart.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(string idPro, string idProItem, int quantity, float price)
        {
            var result = await _shoppingCart.AddToCart(idPro, idProItem, quantity, price);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("{idProItem}")]
        public async Task<IActionResult> UpdateQuantity(string idProItem, int quantity)
        {
            var result = await _shoppingCart.UpdateQuantity(idProItem, quantity);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("{idProItem}")]
        public async Task<IActionResult> Remove(string idProItem)
        {
            var result = await _shoppingCart.RemoveFromCart(idProItem);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
