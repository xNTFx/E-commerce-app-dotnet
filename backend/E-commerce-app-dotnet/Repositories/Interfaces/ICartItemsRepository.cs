using System.Collections.Generic;
using System.Threading.Tasks;
using E_commerce_app_dotnet.Models;

namespace E_commerce_app_dotnet.Repositories.Interfaces
{
    public interface ICartItemsRepository
    {
        Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task<CartItem> SaveAsync(CartItem cartItem);
        Task DeleteByIdAsync(string id);
        Task DeleteByUserIdAsync(string userId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task AddCartItemAsync(CartItem cartItem);

    }
}
