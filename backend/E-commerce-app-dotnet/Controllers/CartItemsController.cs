using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Services.Interfaces;

namespace E_commerce_app_dotnet.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemsService _cartItemsService;
        private readonly ILogger<CartItemsController> _logger;

        /// Initializes a new instance of the <see cref="CartItemsController"/> class.
        public CartItemsController(ICartItemsService cartItemsService, ILogger<CartItemsController> logger)
        {
            _cartItemsService = cartItemsService;
            _logger = logger;
        }

        /// Retrieves a list of cart items with additional details for a specific user.
        [HttpGet]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetCartItems([FromQuery] string userId)
        {
            try
            {
                var result = await _cartItemsService.GetCartItemsWithDetails(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// Adds an item to the cart based on the provided request data.
        [HttpPost("addItemToCart")]
        public async Task<ActionResult<CartItem>> AddItemToCart([FromBody] Dictionary<string, JsonElement> request)
        {
            try
            {
                string token = request.ContainsKey("userId") ? request["userId"].GetString() : throw new ArgumentException("userId is missing");
                string productId = request.ContainsKey("productId") ? request["productId"].GetString() : throw new ArgumentException("productId is missing");
                int count = request.ContainsKey("count") ? request["count"].GetInt32() : throw new ArgumentException("count is missing");

                var result = await _cartItemsService.AddItemToCart(new Dictionary<string, object>
                {
                    { "userId", token },
                    { "productId", productId },
                    { "count", count }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// Updates an existing cart item based on the provided request data.
        [HttpPost("updateCart")]
        public async Task<ActionResult<CartItem>> UpdateCart([FromBody] Dictionary<string, JsonElement> request)
        {
            try
            {
                string token = request.ContainsKey("userId") ? request["userId"].GetString() : throw new ArgumentException("userId is missing");
                string productId = request.ContainsKey("productId") ? request["productId"].GetString() : throw new ArgumentException("productId is missing");
                int count = request.ContainsKey("count") ? request["count"].GetInt32() : throw new ArgumentException("count is missing");

                var result = await _cartItemsService.UpdateCart(new Dictionary<string, object>
                {
                    { "userId", token },
                    { "productId", productId },
                    { "count", count }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// Deletes a specific cart item based on the provided request data.
        [HttpDelete("deleteCartItem")]
        public async Task<IActionResult> DeleteCartItem([FromBody] Dictionary<string, JsonElement> request)
        {
            try
            {
                string token = request.ContainsKey("userId") ? request["userId"].GetString() : throw new ArgumentException("userId is missing");
                string cartId = request.ContainsKey("cartId") ? request["cartId"].GetString() : throw new ArgumentException("cartId is missing");

                await _cartItemsService.DeleteCartItem(new Dictionary<string, object>
                {
                    { "userId", token },
                    { "cartId", cartId }
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// Deletes all items in the cart for a specific user.
        [HttpDelete("deleteEntireCart")]
        public async Task<IActionResult> DeleteEntireCart([FromBody] Dictionary<string, JsonElement> request)
        {
            try
            {
                string token = request.ContainsKey("userId") ? request["userId"].GetString() : throw new ArgumentException("userId is missing");

                await _cartItemsService.DeleteEntireCart(new Dictionary<string, object>
                {
                    { "userId", token }
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
