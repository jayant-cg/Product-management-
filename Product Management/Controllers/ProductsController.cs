using Microsoft.AspNetCore.Mvc;
using Product_Management.Services;
using ProductManagement.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Product_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all products</returns>
        /// <response code="200">Returns the list of products</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all products",
            Description = "Retrieves a list of all products in the database",
            Tags = new[] { "Products" }
        )]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get product by ID",
            Description = "Retrieves a specific product by its unique identifier",
            Tags = new[] { "Products" }
        )]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="product">Product details</param>
        /// <returns>Created product</returns>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid product data</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new product",
            Description = "Creates a new product with the provided details",
            Tags = new[] { "Products" }
        )]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="product">Updated product details</param>
        /// <returns>Updated product</returns>
        /// <response code="200">Product updated successfully</response>
        /// <response code="400">Invalid product data</response>
        /// <response code="404">Product not found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing product",
            Description = "Updates the details of an existing product",
            Tags = new[] { "Products" }
        )]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await _productService.UpdateProductAsync(id, product);

            if (updatedProduct == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return Ok(updatedProduct);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Product deleted successfully</response>
        /// <response code="404">Product not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a product",
            Description = "Deletes a product from the database",
            Tags = new[] { "Products" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
