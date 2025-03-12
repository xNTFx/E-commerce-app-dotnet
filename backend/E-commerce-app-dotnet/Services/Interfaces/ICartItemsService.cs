using E_commerce_app_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface ICartItemsService
    {
        /// Retrieves a list of cart items with additional details for a specific user based on the provided ID token.
        Task<List<Dictionary<string, object>>> GetCartItemsWithDetails(string idToken);

        /// Adds an item to the cart based on the provided request data.
        Task<CartItem> AddItemToCart(Dictionary<string, object> request);

        /// Updates an existing cart item based on the provided request data.
        Task<CartItem> UpdateCart(Dictionary<string, object> request);

        /// Deletes a specific cart item based on the provided request data.
        Task DeleteCartItem(Dictionary<string, object> request);

        /// Deletes all items in the cart based on the provided request data.
        Task DeleteEntireCart(Dictionary<string, object> request);

        /// Retrieves a list of cart items for a specific user.
        List<CartItem> GetCartItems(string userId);

        /// Deletes all cart items for a specific user.
        void DeleteAllCartItems(string userId);
    }
}
