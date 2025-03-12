using E_commerce_app_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_commerce_app_dotnet.Controllers
{
    [ApiController]
    [Route("stripe")]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        /// Initializes a new instance of the <see cref="StripeController"/> class.
        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        /// Retrieves the Stripe configuration settings.
        [HttpGet("config")]
        public ActionResult<Dictionary<string, string>> GetConfig()
        {
            return Ok(_stripeService.GetStripeConfig());
        }

        /// Creates a new payment intent using Stripe's API, preparing it for processing a transaction.
        [HttpPost("create-payment-intent")]
        public ActionResult<Dictionary<string, string>> CreatePaymentIntent()
        {
            var result = _stripeService.CreatePaymentIntent();
            return Ok(result);
        }
    }
}
