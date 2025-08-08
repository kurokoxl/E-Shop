using Eshop.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Data
{
    /// <summary>
    /// Entity Framework database context for the e-shop application.
    /// </summary>
    public class ProductDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the products table.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the users table.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the carts table.
        /// </summary>
        public DbSet<Cart> Carts { get; set; }

        /// <summary>
        /// Gets or sets the cart products junction table.
        /// </summary>
        public DbSet<CartProduct> CartProducts { get; set; }

        /// <summary>
        /// Initializes a new instance of the ProductDbContext class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// Configures the database connection and options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to configure.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Note: In production, move connection string to configuration files
            var connectionString = "server=localhost;database=ProductDb;user=root;password=uzumaki123;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        /// <summary>
        /// Configures the entity models and relationships, and seeds initial data.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureProductEntity(modelBuilder);
            ConfigureUserEntity(modelBuilder);
            ConfigureCartEntity(modelBuilder);
            ConfigureCartProductEntity(modelBuilder);
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Configures the Product entity.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.StockLevel).IsRequired();
            });
        }

        /// <summary>
        /// Configures the User entity.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID);
        }

        /// <summary>
        /// Configures the Cart entity.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void ConfigureCartEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartID);
            });
        }

        /// <summary>
        /// Configures the CartProduct entity and its relationships.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void ConfigureCartProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartProduct>(entity =>
            {
                entity.HasKey(cp => new { cp.CartID, cp.ProductID });
                entity.Property(e => e.Quantity).IsRequired();
            });

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartID);

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductID);
        }

        /// <summary>
        /// Seeds initial data for the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, Name = "Laptop", Price = 1200.00m, StockLevel = 10 },
                new Product { ProductID = 2, Name = "Phone", Price = 800.00m, StockLevel = 15 },
                new Product { ProductID = 3, Name = "Mouse", Price = 25.00m, StockLevel = 50 },
                new Product { ProductID = 4, Name = "Keyboard", Price = 45.00m, StockLevel = 30 },
                new Product { ProductID = 5, Name = "Monitor", Price = 200.00m, StockLevel = 20 }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Email = "ali@example.com", PasswordHash = "hashedpass1" },
                new User { UserID = 2, Email = "sara@example.com", PasswordHash = "hashedpass2" },
                new User { UserID = 3, Email = "kareem@example.com", PasswordHash = "hashedpass3" },
                new User { UserID = 4, Email = "laila@example.com", PasswordHash = "hashedpass4" },
                new User { UserID = 5, Email = "omar@example.com", PasswordHash = "hashedpass5" }
            );

            // Seed Carts - Each user has 1 cart
            modelBuilder.Entity<Cart>().HasData(
                new Cart { CartID = 1, UserID = 1 },
                new Cart { CartID = 2, UserID = 2 },
                new Cart { CartID = 3, UserID = 3 },
                new Cart { CartID = 4, UserID = 4 },
                new Cart { CartID = 5, UserID = 5 }
            );

            // Seed CartProducts
            modelBuilder.Entity<CartProduct>().HasData(
                new CartProduct { CartID = 1, ProductID = 1, Quantity = 2 },
                new CartProduct { CartID = 1, ProductID = 3, Quantity = 1 },
                new CartProduct { CartID = 2, ProductID = 2, Quantity = 5 },
                new CartProduct { CartID = 2, ProductID = 4, Quantity = 3 },
                new CartProduct { CartID = 3, ProductID = 1, Quantity = 1 },
                new CartProduct { CartID = 3, ProductID = 5, Quantity = 2 },
                new CartProduct { CartID = 4, ProductID = 2, Quantity = 1 },
                new CartProduct { CartID = 5, ProductID = 3, Quantity = 4 }
            );
        }
    }
}