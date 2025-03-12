using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using E_commerce_app_dotnet.Models;
using Microsoft.Extensions.Logging;
using E_commerce_app_dotnet.Services.Interfaces;

namespace E_commerce_app_dotnet.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersController> _logger;

        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        public OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        /// Retrieves a list of orders for a specific user based on the provided ID token.
        [HttpGet("getOrders")]
        public ActionResult<List<Order>> GetOrders([FromQuery] string idToken)
        {
            try
            {
                string userId = _ordersService.GetFirebaseAuthService().VerifyIdToken(idToken);
                List<Order> orders = _ordersService.GetOrders(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// Creates a new order based on the provided request body.
        [HttpPost("createOrder")]
        public ActionResult<Order> CreateOrder([FromBody] Dictionary<string, object> requestBody)
        {
            try
            {
                string userId;

                if (requestBody.ContainsKey("idToken"))
                {
                    string idToken = requestBody["idToken"].ToString();
                    userId = _ordersService.GetFirebaseAuthService().VerifyIdToken(idToken);
                }
                else
                {
                    userId = "anonymous";
                }

                if (!requestBody.ContainsKey("state"))
                {
                    return BadRequest(new { message = "Missing 'state' in request body" });
                }

                Order order = _ordersService.ProcessOrder(userId, requestBody);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// Retrieves details of a specific order by its ID.
        [HttpGet("getOrderDetails/{orderId}")]
        public ActionResult GetOrderDetails([FromRoute] string orderId)
        {
            try
            {
                Order enrichedOrder = _ordersService.GetEnrichedOrder(orderId);
                if (enrichedOrder == null)
                {
                    return NotFound(new { message = "Order not found" });
                }
                return Ok(enrichedOrder);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
