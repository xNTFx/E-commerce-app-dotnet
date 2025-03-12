using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductsRepository(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:Uri"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _products = database.GetCollection<Product>("products");
        }

        public async Task<List<Product>> GetProductsAsync(int offset, int limit, string sortBy, double? minPrice, double? maxPrice, List<string> categoryList)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                var priceFilter = Builders<Product>.Filter.Gte(p => p.Price, minPrice ?? 0);
                if (maxPrice.HasValue)
                    priceFilter &= Builders<Product>.Filter.Lte(p => p.Price, maxPrice.Value);

                filter &= priceFilter;
            }

            if (categoryList != null && categoryList.Count > 0)
            {
                filter &= Builders<Product>.Filter.In(p => p.Category, categoryList);
            }

            var sortDefinition = GetSortOrder(sortBy);
            return await _products.Find(filter)
                                  .Sort(sortDefinition)
                                  .Skip(offset)
                                  .Limit(limit)
                                  .ToListAsync();
        }

        public async Task<long> GetTotalProductsCountAsync(double? minPrice, double? maxPrice, List<string> categoryList)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                var priceFilter = Builders<Product>.Filter.Gte(p => p.Price, minPrice ?? 0);
                if (maxPrice.HasValue)
                    priceFilter &= Builders<Product>.Filter.Lte(p => p.Price, maxPrice.Value);

                filter &= priceFilter;
            }

            if (categoryList != null && categoryList.Count > 0)
            {
                filter &= Builders<Product>.Filter.In(p => p.Category, categoryList);
            }

            return await _products.CountDocumentsAsync(filter);
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products.Find(p => p._id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<string> ids)
        {
            return await _products.Find(p => ids.Contains(p._id)).ToListAsync();
        }

        private SortDefinition<Product> GetSortOrder(string sortBy)
        {
            return sortBy switch
            {
                "price-asc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "price-desc" => Builders<Product>.Sort.Descending(p => p.Price),
                "name-az" => Builders<Product>.Sort.Ascending(p => p.Title),
                "name-za" => Builders<Product>.Sort.Descending(p => p.Title),
                _ => Builders<Product>.Sort.Ascending(p => p._id)
            };
        }
    }
}
