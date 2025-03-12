using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Services.Interfaces;

namespace E_commerce_app_dotnet.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        /// Retrieves a list of all categories.
        [HttpGet]
        public ActionResult<List<Categories>> GetAllCategories()
        {
            var categories = _categoriesService.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("Category not found");
            }
            return Ok(categories);
        }
    }
}
