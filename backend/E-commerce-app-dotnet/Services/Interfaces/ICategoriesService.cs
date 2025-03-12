using System.Collections.Generic;
using E_commerce_app_dotnet.Models;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface ICategoriesService
    {
        /// Retrieves a list of all categories.
        List<Categories> GetAllCategories();
    }
}
