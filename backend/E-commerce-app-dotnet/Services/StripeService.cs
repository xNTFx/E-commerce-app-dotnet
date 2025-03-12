using E_commerce_app_dotnet.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;

namespace E_commerce_app_dotnet.Services
{
    public class StripeService : IStripeService
    {
        private readonly string _secretKey;
        private readonly string _publishableKey;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"];
            _publishableKey = configuration["Stripe:PublishableKey"];

            StripeConfiguration.ApiKey = _secretKey;
        }

        public Dictionary<string, string> GetStripeConfig()
        {
            var response = new Dictionary<string, string>
            {
                { "publishableKey", _publishableKey }
            };
            return response;
        }

        public Dictionary<string, string> CreatePaymentIntent()
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1999,
                Currency = "eur",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            var response = new Dictionary<string, string>
            {
                { "clientSecret", paymentIntent.ClientSecret }
            };
            return response;
        }
    }
}
