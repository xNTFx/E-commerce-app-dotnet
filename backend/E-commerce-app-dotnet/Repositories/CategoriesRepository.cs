using MongoDB.Driver;
using System.Collections.Generic;
using E_commerce_app_dotnet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using E_commerce_app_dotnet.Repositories.Interfaces;

namespace E_commerce_app_dotnet.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IMongoCollection<Categories> _categories;
        private readonly ILogger<CategoriesRepository> _logger;

        public CategoriesRepository(IConfiguration config, IMongoClient client, ILogger<CategoriesRepository> logger)
        {
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _categories = database.GetCollection<Categories>("categories");
            _logger = logger;
        }

        public List<Categories> GetAllCategories()
        {
            var categories = _categories.Find(c => true).ToList();
            _logger.LogInformation("Fetched {Count} categories", categories.Count);
            return categories;
        }
    }
}
