using E_commerce_app_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface IProductsService
    {
        /// Retrieves a list of products by their IDs asynchronously.
        Task<List<Product>> GetProductsByIdsAsync(List<string> productIds);

        /// Retrieves a product by its ID asynchronously.
        Task<Product> GetProductByIdAsync(string productId);

        /// Retrieves a paginated list of products based on filters such as price range, categories, and sorting options.
        Task<List<Product>> GetProductsAsync(int offset, int limit, string sortBy, double? minPrice, double? maxPrice, List<string> categoryList);

        /// Retrieves the total count of products based on filters such as price range and categories.
        Task<long> GetTotalProductsCountAsync(double? minPrice, double? maxPrice, List<string> categoryList);

        /// Retrieves a list of products by their IDs.
        List<Product> GetProductsByIds(List<string> productIds);

        /// Retrieves a product by its ID.
        Product GetProductById(string productId);
    }
}
