using E_commerce_app_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProductsAsync(int offset, int limit, string sortBy, double? minPrice, double? maxPrice, List<string> categoryList);
        Task<long> GetTotalProductsCountAsync(double? minPrice, double? maxPrice, List<string> categoryList);
        Task<Product> GetProductByIdAsync(string id);
        Task<List<Product>> GetProductsByIdsAsync(List<string> ids);
    }
}
