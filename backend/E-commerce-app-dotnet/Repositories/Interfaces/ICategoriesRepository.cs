using System.Collections.Generic;
using E_commerce_app_dotnet.Models;

namespace E_commerce_app_dotnet.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        List<Categories> GetAllCategories();
    }
}
