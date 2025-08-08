using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Eshop.Data;
using Eshop.Entities;

namespace Eshop.Controllers
{
    /// <summary>
    /// Controller for managing product operations including CRUD operations and search functionality.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _productDbContext;

        /// <summary>
        /// Initializes a new instance of the ProductController.
        /// </summary>
        /// <param name="context">The database context for product operations.</param>
        public ProductController(ProductDbContext context)
        {
            _productDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productDbContext.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The requested product if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Product ID must be greater than zero.");
                }

                var product = await _productDbContext.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Searches for products by name using case-insensitive partial matching.
        /// </summary>
        /// <param name="name">The name or partial name to search for.</param>
        /// <returns>A list of products matching the search criteria.</returns>
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchProduct(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Search name cannot be empty.");
                }

                var result = await _productDbContext.Products
                    .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Data transfer object for adding a new product.
        /// </summary>
        public record AddProductRequest
        {
            /// <summary>
            /// The name of the product.
            /// </summary>
            [Required]
            [StringLength(50, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 50 characters.")]
            public string Name { get; init; } = string.Empty;

            /// <summary>
            /// The price of the product.
            /// </summary>
            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
            public decimal Price { get; init; }

            /// <summary>
            /// The stock level of the product.
            /// </summary>
            [Required]
            [Range(0, int.MaxValue, ErrorMessage = "Stock level must be non-negative.")]
            public int StockLevel { get; init; }
        }

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="request">The product data to create.</param>
        /// <returns>The created product with its assigned ID.</returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid product data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    StockLevel = request.StockLevel
                };

                await _productDbContext.Products.AddAsync(product);
                await _productDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Data transfer object for updating an existing product.
        /// </summary>
        public record UpdateProductRequest
        {
            /// <summary>
            /// The unique identifier of the product to update.
            /// </summary>
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than zero.")]
            public int ProductID { get; init; }

            /// <summary>
            /// The updated name of the product.
            /// </summary>
            [Required]
            [StringLength(50, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 50 characters.")]
            public string Name { get; init; } = string.Empty;

            /// <summary>
            /// The updated price of the product.
            /// </summary>
            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
            public decimal Price { get; init; }

            /// <summary>
            /// The updated stock level of the product.
            /// </summary>
            [Required]
            [Range(0, int.MaxValue, ErrorMessage = "Stock level must be non-negative.")]
            public int StockLevel { get; init; }
        }

        /// <summary>
        /// Updates an existing product with new information.
        /// </summary>
        /// <param name="request">The updated product data.</param>
        /// <returns>Success message if updated, otherwise error response.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Product data is required.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingProduct = await _productDbContext.Products.FindAsync(request.ProductID);

                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {request.ProductID} not found.");
                }

                existingProduct.Name = request.Name;
                existingProduct.Price = request.Price;
                existingProduct.StockLevel = request.StockLevel;
                
                await _productDbContext.SaveChangesAsync();

                return Ok(new { message = "Product updated successfully.", product = existingProduct });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>Success message if deleted, otherwise error response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Product ID must be greater than zero.");
                }

                var existingProduct = await _productDbContext.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                
                _productDbContext.Products.Remove(existingProduct);
                await _productDbContext.SaveChangesAsync();
                
                return Ok(new { message = $"Product '{existingProduct.Name}' deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
