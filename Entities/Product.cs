namespace Eshop.Entities
{
    /// <summary>
    /// Represents a product in the e-shop system.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the current stock level of the product.
        /// </summary>
        public int StockLevel { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for cart products that contain this product.
        /// </summary>
        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}