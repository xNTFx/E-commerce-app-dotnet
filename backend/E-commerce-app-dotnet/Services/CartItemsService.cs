using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Repositories.Interfaces;
using E_commerce_app_dotnet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services
{
    public class CartItemsService : ICartItemsService
    {
        private readonly ICartItemsRepository _cartRepo;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IProductsService _productsService;

        public CartItemsService(ICartItemsRepository cartRepo,
                                IFirebaseAuthService firebaseAuthService,
                                IProductsService productsService)
        {
            _cartRepo = cartRepo;
            _firebaseAuthService = firebaseAuthService;
            _productsService = productsService;
        }

        public async Task<List<Dictionary<string, object>>> GetCartItemsWithDetails(string userId)
        {
            string decodedUserId = await _firebaseAuthService.VerifyIdTokenAsync(userId);
            var cartItems = await _cartRepo.GetCartItemsByUserIdAsync(decodedUserId);
            if (cartItems == null || !cartItems.Any())
            {
                return new List<Dictionary<string, object>>();
            }

            var productIds = cartItems.Select(ci => ci.ProductId).Distinct().ToList();
            var products = await _productsService.GetProductsByIdsAsync(productIds);

            var productMap = products.ToDictionary(p => p._id, p => p);

            var result = cartItems.Select(ci => new Dictionary<string, object>
            {
                { "_id", ci._id },
                { "userId", ci.UserId },
                { "productId", ci.ProductId },
                { "count", ci.Count },
                { "productDetails", productMap.ContainsKey(ci.ProductId) ? new List<object> { productMap[ci.ProductId] } : new List<object>() }
            }).ToList();

            return result;
        }

        public async Task<CartItem> AddItemToCart(Dictionary<string, object> request)
        {
            if (!ValidateRequest(request, out string token, out string productId, out int count))
            {
                throw new ArgumentException("userId, productId, and count are required");
            }
            string decodedUserId = await _firebaseAuthService.VerifyIdTokenAsync(token);
            return await UpdateOrAddCartItem(decodedUserId, productId, count);
        }

        public async Task<CartItem> UpdateCart(Dictionary<string, object> request)
        {
            if (!ValidateRequest(request, out string token, out string productId, out int count))
            {
                throw new ArgumentException("userId, productId, and count are required");
            }
            string decodedUserId = await _firebaseAuthService.VerifyIdTokenAsync(token);
            return await UpdateCartItem(decodedUserId, productId, count);
        }

        public async Task DeleteCartItem(Dictionary<string, object> request)
        {
            if (request == null ||
                !request.ContainsKey("userId") || string.IsNullOrWhiteSpace(request["userId"]?.ToString()) ||
                !request.ContainsKey("cartId") || string.IsNullOrWhiteSpace(request["cartId"]?.ToString()))
            {
                throw new ArgumentException("userId and cartId are required");
            }
            string token = request["userId"].ToString();
            string decodedUserId = await _firebaseAuthService.VerifyIdTokenAsync(token);
            string cartId = request["cartId"].ToString();
            await _cartRepo.DeleteByIdAsync(cartId);
        }

        public async Task DeleteEntireCart(Dictionary<string, object> request)
        {
            if (request == null ||
                !request.ContainsKey("userId") || string.IsNullOrWhiteSpace(request["userId"]?.ToString()))
            {
                throw new ArgumentException("userId is required");
            }
            string token = request["userId"].ToString();
            string decodedUserId = await _firebaseAuthService.VerifyIdTokenAsync(token);
            await _cartRepo.DeleteByUserIdAsync(decodedUserId);
        }

        public List<CartItem> GetCartItems(string userId)
        {
            return _cartRepo.GetCartItemsByUserIdAsync(userId).Result;
        }

        public void DeleteAllCartItems(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty");
            }

            _cartRepo.DeleteByUserIdAsync(userId).Wait();
        }

        // Pomocnicza metoda do walidacji
        private bool ValidateRequest(Dictionary<string, object> request, out string token, out string productId, out int count)
        {
            token = null;
            productId = null;
            count = 0;

            if (request == null ||
                !request.ContainsKey("userId") || string.IsNullOrWhiteSpace(request["userId"]?.ToString()) ||
                !request.ContainsKey("productId") || string.IsNullOrWhiteSpace(request["productId"]?.ToString()) ||
                !request.ContainsKey("count") || string.IsNullOrWhiteSpace(request["count"]?.ToString()))
            {
                return false;
            }
            token = request["userId"].ToString();
            productId = request["productId"].ToString();
            count = Convert.ToInt32(request["count"]);
            return true;
        }
        private async Task<CartItem> UpdateOrAddCartItem(string userId, string productId, int count)
        {
            var existingItems = await _cartRepo.GetCartItemsByUserIdAsync(userId);
            var existingCartItem = existingItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingCartItem != null)
            {
                existingCartItem.Count += count;
                return await _cartRepo.SaveAsync(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = count
                };
                return await _cartRepo.SaveAsync(newCartItem);
            }
        }

        private async Task<CartItem> UpdateCartItem(string userId, string productId, int count)
        {
            var existingItems = await _cartRepo.GetCartItemsByUserIdAsync(userId);
            var existingCartItem = existingItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingCartItem == null)
            {
                throw new ArgumentException("Cart item not found for given product");
            }
            existingCartItem.Count = count;
            return await _cartRepo.SaveAsync(existingCartItem);
        }

    }
}
