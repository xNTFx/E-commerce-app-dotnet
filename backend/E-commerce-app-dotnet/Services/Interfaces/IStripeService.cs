using System.Collections.Generic;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface IStripeService
    {
        /// Retrieves the Stripe configuration settings.
        Dictionary<string, string> GetStripeConfig();

        /// Creates a new payment intent using Stripe's API, preparing it for processing a transaction
        Dictionary<string, string> CreatePaymentIntent();
    }
}
