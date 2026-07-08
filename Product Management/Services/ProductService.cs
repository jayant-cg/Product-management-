using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using ProductManagement.Models;

namespace Product_Management.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product Created: {ProductId} - {ProductName}", product.Id, product.Name);

            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Product Updated: {ProductId} - {ProductName}", existingProduct.Id, existingProduct.Name);

            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product Deleted: {ProductId} - {ProductName}", product.Id, product.Name);

            return true;
        }

        public async Task<IEnumerable<Product>> SearchProductsByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower())
                .ToListAsync();
        }
    }
}
