using Eshop.Entities;

namespace Eshop.Entities
{
    /// <summary>
    /// Represents the relationship between a cart and a product, including quantity.
    /// This is a junction table for the many-to-many relationship between Cart and Product.
    /// </summary>
    public class CartProduct
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart.
        /// </summary>
        public int CartID { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the cart.
        /// </summary>
        public Cart? Cart { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the product.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the cart.
        /// </summary>
        public int Quantity { get; set; }
    }
}
