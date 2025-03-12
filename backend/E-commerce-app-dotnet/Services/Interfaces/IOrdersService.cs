using System.Collections.Generic;
using E_commerce_app_dotnet.Models;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface IOrdersService
    {
        /// Retrieves a list of orders for a specific user.
        List<Order> GetOrders(string userId);

        /// Retrieves an order by its ID.
        Order GetOrderById(string orderId);

        /// Creates a new order for a specific user.
        Order CreateOrder(string userId, Order orderData);

        /// Processes an order based on the provided request body and user ID.
        public Order ProcessOrder(string userId, Dictionary<string, object> requestBody);

        /// Retrieves an enriched order with additional details.
        Order GetEnrichedOrder(string orderId);

        /// Retrieves the Firebase authentication service instance.
        IFirebaseAuthService GetFirebaseAuthService();
    }
}
