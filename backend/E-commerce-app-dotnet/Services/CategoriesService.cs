using System.Collections.Generic;
using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Repositories.Interfaces;
using E_commerce_app_dotnet.Services.Interfaces;

namespace E_commerce_app_dotnet.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public List<Categories> GetAllCategories() => _categoriesRepository.GetAllCategories();
    }
}
