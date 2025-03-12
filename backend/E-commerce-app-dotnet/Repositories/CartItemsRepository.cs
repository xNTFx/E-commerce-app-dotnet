using E_commerce_app_dotnet.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using E_commerce_app_dotnet.Repositories.Interfaces;

namespace E_commerce_app_dotnet.Repositories
{
    public class CartItemsRepository : ICartItemsRepository
    {
        private readonly IMongoCollection<CartItem> _collection;

        public CartItemsRepository(IConfiguration config, IMongoClient client)
        {
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _collection = database.GetCollection<CartItem>("cart_items");
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _collection.Find(item => item.UserId == userId).ToListAsync();
        }

        public async Task<CartItem> SaveAsync(CartItem cartItem)
        {
            if (string.IsNullOrEmpty(cartItem._id))
            {
                await _collection.InsertOneAsync(cartItem);
            }
            else
            {
                await _collection.ReplaceOneAsync(item => item._id == cartItem._id, cartItem);
            }
            return cartItem;
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _collection.DeleteOneAsync(item => item._id == id);
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            await _collection.DeleteManyAsync(item => item.UserId == userId);
        }
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            await _collection.ReplaceOneAsync(item => item._id == cartItem._id, cartItem);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _collection.InsertOneAsync(cartItem);
        }

    }
}
