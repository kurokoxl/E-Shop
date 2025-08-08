using Eshop.Entities;

namespace Eshop.Entities
{
    /// <summary>
    /// Represents a shopping cart in the e-shop system.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart.
        /// </summary>
        public int CartID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns this cart.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the user who owns this cart.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets the collection of products in this cart.
        /// </summary>
        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}
