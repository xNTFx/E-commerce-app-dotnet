using E_commerce_app_dotnet.Models;
using E_commerce_app_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// Retrieves a paginated list of products based on filters such as price range, categories, and sorting options.
        [HttpGet]
        public async Task<ActionResult<object>> GetProducts(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = null,
            [FromQuery] double? minPrice = null,
            [FromQuery] double? maxPrice = null,
            [FromQuery] List<string> categoryList = null)
        {
            var products = await _productsService.GetProductsAsync(offset, limit, sortBy, minPrice, maxPrice, categoryList);
            var totalCount = await _productsService.GetTotalProductsCountAsync(minPrice, maxPrice, categoryList);

            return Ok(new { data = products, totalCount });
        }

        /// Retrieves a single product by its ID.
        [HttpGet("singleProduct")]
        public async Task<ActionResult<Product>> GetSingleProduct([FromQuery] string id)
        {
            var product = await _productsService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product not found");
            return Ok(product);
        }

        /// Retrieves a list of products by an array of product IDs.
        [HttpGet("productsFromIdArray")]
        public async Task<ActionResult<List<Product>>> GetProductsFromIdArray([FromQuery] List<string> ids)
        {
            var products = await _productsService.GetProductsByIdsAsync(ids);
            return Ok(products);
        }
    }
}
