using Microsoft.AspNetCore.Mvc;
using Product_Management.Services;
using ProductManagement.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Product_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SearchController : ControllerBase
    {
        private readonly IProductService _productService;

        public SearchController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Search products by category
        /// </summary>
        /// <param name="category">Product category to search for</param>
        /// <returns>List of products in the specified category</returns>
        /// <response code="200">Returns products in the category</response>
        /// <response code="400">Invalid category parameter</response>
        [HttpGet("category/{category}")]
        [SwaggerOperation(
            Summary = "Search products by category",
            Description = "Retrieves all products that match the specified category (case-insensitive)",
            Tags = new[] { "Search" }
        )]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> SearchByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(new { message = "Category parameter is required." });
            }

            var products = await _productService.SearchProductsByCategoryAsync(category);
            return Ok(products);
        }
    }
}
