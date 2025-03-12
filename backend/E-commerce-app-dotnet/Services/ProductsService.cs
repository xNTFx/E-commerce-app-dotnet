using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Repositories.Interfaces;
using E_commerce_app_dotnet.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public Task<List<Product>> GetProductsByIdsAsync(List<string> productIds)
        {
            return _productsRepository.GetProductsByIdsAsync(productIds);
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _productsRepository.GetProductByIdAsync(productId);
        }

        public Task<List<Product>> GetProductsAsync(int offset, int limit, string sortBy, double? minPrice, double? maxPrice, List<string> categoryList)
        {
            return _productsRepository.GetProductsAsync(offset, limit, sortBy, minPrice, maxPrice, categoryList);
        }

        public Task<long> GetTotalProductsCountAsync(double? minPrice, double? maxPrice, List<string> categoryList)
        {
            return _productsRepository.GetTotalProductsCountAsync(minPrice, maxPrice, categoryList);
        }

        public List<Product> GetProductsByIds(List<string> productIds)
        {
            return GetProductsByIdsAsync(productIds).Result;
        }

        public Product GetProductById(string productId)
        {
            return GetProductByIdAsync(productId).Result;
        }
    }
}
