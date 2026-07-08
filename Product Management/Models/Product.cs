
using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductManagement.Models
{
    public class Product
    {
        [SwaggerSchema(Description = "Unique identifier for the product", ReadOnly = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [SwaggerSchema(Description = "Name of the product")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [SwaggerSchema(Description = "Category of the product")]
        public string Category { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [SwaggerSchema(Description = "Price of the product in USD")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        [SwaggerSchema(Description = "Number of items in stock")]
        public int Stock { get; set; }

        [SwaggerSchema(Description = "Date when the product was created", ReadOnly = true)]
        public DateTime CreatedDate { get; set; }

        [SwaggerSchema(Description = "Date when the product was last updated", ReadOnly = true)]
        public DateTime UpdatedDate { get; set; }

    }
}
