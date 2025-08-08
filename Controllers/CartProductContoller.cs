using System.ComponentModel.DataAnnotations;
using Eshop.Data;
using Eshop.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    /// <summary>
    /// Controller for managing shopping cart operations including adding, updating, and removing products from cart.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartProductController : ControllerBase
    {
        private readonly ProductDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CartProductController.
        /// </summary>
        /// <param name="context">The database context for cart operations.</param>
        public CartProductController(ProductDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all products in a user's cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of cart items for the specified user.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCartProducts(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User ID must be greater than zero.");
                }

                var items = await _context.CartProducts
                    .Include(cp => cp.Product)
                    .Include(cp => cp.Cart)
                    .Where(cp => cp.Cart != null && cp.Cart.UserID == userId)
                    .Select(cp => new
                    {
                        cp.ProductID,
                        cp.Product!.Name,
                        cp.Product.Price,
                        cp.Quantity,
                        TotalPrice = cp.Product.Price * cp.Quantity
                    })
                    .ToListAsync();

                if (!items.Any())
                {
                    return Ok(new { message = "Cart is empty for this user.", items = new List<object>() });
                }

                return Ok(new { message = "Cart items retrieved successfully.", items });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Data transfer object for adding products to cart.
        /// </summary>
        public record AddToCartRequest
        {
            /// <summary>
            /// The unique identifier of the product to add.
            /// </summary>
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than zero.")]
            public int ProductID { get; init; }

            /// <summary>
            /// The unique identifier of the user.
            /// </summary>
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than zero.")]
            public int UserID { get; init; }

            /// <summary>
            /// The quantity of the product to add to cart.
            /// </summary>
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
            public int Quantity { get; init; }
        }

        /// <summary>
        /// Adds a product to the user's cart or updates quantity if product already exists.
        /// </summary>
        /// <param name="request">The cart addition request containing product, user, and quantity information.</param>
        /// <returns>Success message if added, otherwise error response.</returns>
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid input data.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = await _context.Products.FindAsync(request.ProductID);
                if (product == null)
                {
                    return NotFound($"Product with ID {request.ProductID} not found.");
                }

                if (product.StockLevel < request.Quantity)
                {
                    return BadRequest($"Not enough stock available. Current stock: {product.StockLevel}");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartProducts)
                    .FirstOrDefaultAsync(c => c.UserID == request.UserID);

                if (cart == null)
                {
                    // Create a new cart for this user
                    cart = new Cart
                    {
                        UserID = request.UserID,
                        CartProducts = new List<CartProduct>()
                    };

                    await _context.Carts.AddAsync(cart);
                }

                // Check if product already exists in the cart
                var existingCartProduct = cart.CartProducts
                    .FirstOrDefault(cp => cp.ProductID == request.ProductID);

                if (existingCartProduct != null)
                {
                    // If it exists, update quantity
                    existingCartProduct.Quantity += request.Quantity;
                }
                else
                {
                    // If it doesn't exist, add new entry
                    var cartProduct = new CartProduct
                    {
                        Cart = cart,
                        Product = product,
                        Quantity = request.Quantity
                    };

                    cart.CartProducts.Add(cartProduct);
                }

                // Update product stock level
                product.StockLevel -= request.Quantity;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Product added to cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the quantity of a specific product in the user's cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="quantity">The new quantity for the product.</param>
        /// <returns>Success message if updated, otherwise error response.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCartQuantity(int userId, int productId, int quantity)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User ID must be greater than zero.");
                }

                if (productId <= 0)
                {
                    return BadRequest("Product ID must be greater than zero.");
                }

                if (quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero.");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartProducts)
                    .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.UserID == userId);

                if (cart == null)
                {
                    return NotFound("Cart not found for this user.");
                }

                var cartItem = cart.CartProducts.FirstOrDefault(c => c.ProductID == productId);
                if (cartItem == null)
                {
                    return NotFound("Product not found in cart.");
                }

                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Updated {cartItem.Product?.Name ?? "Unknown Product"}'s quantity to {cartItem.Quantity}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes a specific product from the user's cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>Success message if removed, otherwise error response.</returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("User ID must be greater than zero.");
                }

                if (productId <= 0)
                {
                    return BadRequest("Product ID must be greater than zero.");
                }

                var cart = await _context.Carts
                    .Include(c => c.CartProducts)
                    .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.UserID == userId);

                if (cart == null)
                {
                    return NotFound("Cart not found for this user.");
                }

                var cartItem = cart.CartProducts.FirstOrDefault(c => c.ProductID == productId);
                if (cartItem == null)
                {
                    return NotFound("Product not found in cart.");
                }

                var productName = cartItem.Product?.Name ?? "Unknown Product";
                cart.CartProducts.Remove(cartItem);
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Item '{productName}' removed from your cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
