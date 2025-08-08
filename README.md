# E-Shop API

A simple e-commerce API built with ASP.NET Core 9.0 and Entity Framework Core, featuring product management and shopping cart functionality.

## Features

- **Product Management**: Create, read, update, delete, and search products
- **Shopping Cart**: Add products to cart, update quantities, and remove items
- **User Management**: Basic user system with cart association
- **Database Integration**: MySQL database with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI integration

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core
- MySQL Database
- Swagger/OpenAPI
- C# 12

## API Endpoints

### Products

- `GET /api/Product` - Get all products
- `GET /api/Product/{id}` - Get a specific product by ID
- `GET /api/Product/search/{name}` - Search products by name
- `POST /api/Product` - Create a new product
- `PUT /api/Product` - Update an existing product
- `DELETE /api/Product/{id}` - Delete a product

### Cart

- `GET /api/CartProduct?userId={userId}` - Get cart items for a user
- `POST /api/CartProduct` - Add product to cart
- `PUT /api/CartProduct?userId={userId}&productId={productId}&quantity={quantity}` - Update cart item quantity
- `DELETE /api/CartProduct?userId={userId}&productId={productId}` - Remove item from cart

## Data Models

### Product
```json
{
  "productID": 1,
  "name": "Laptop",
  "price": 1200.00,
  "stockLevel": 10
}
```

### Add Product Request
```json
{
  "name": "New Product",
  "price": 99.99,
  "stockLevel": 50
}
```

### Add to Cart Request
```json
{
  "productID": 1,
  "userID": 1,
  "quantity": 2
}
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- MySQL Server
- Visual Studio 2022 or VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/E-Shop.git
   cd E-Shop
   ```

2. **Configure Database**
   - Ensure MySQL is running
   - Update connection string in `Program.cs` if needed:
     ```csharp
     "server=localhost;database=ProductDb;user=root;password=uzumaki123;"
     ```

3. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access API Documentation**
   - Open browser and navigate to `https://localhost:5001` (or the port shown in console)
   - Swagger UI will be displayed with all available endpoints

## Project Structure

```
E-Shop/
├── Controllers/           # API Controllers
│   ├── ProductController.cs
│   └── CartProductController.cs
├── Data/                 # Database Context
│   └── ProductDbContext.cs
├── Entities/             # Entity Models
│   ├── Product.cs
│   ├── User.cs
│   ├── Cart.cs
│   └── CartProduct.cs
├── Migrations/           # EF Core Migrations
├── Properties/           # Launch Settings
└── Program.cs           # Application Entry Point
```

## Code Quality Features

- **Comprehensive XML Documentation**: All public methods and classes are documented
- **Input Validation**: Data annotations and model validation
- **Error Handling**: Try-catch blocks with proper error responses
- **Consistent Naming**: Following C# naming conventions
- **Separation of Concerns**: DTOs separate from entities
- **Dependency Injection**: Proper DI container usage
- **Async/Await**: Asynchronous operations throughout

## Database Schema

The application uses the following entities:

- **Products**: Store product information
- **Users**: User accounts with email and password hash
- **Carts**: Shopping carts associated with users
- **CartProducts**: Junction table for cart-product relationships

## Sample Data

The application includes seeded data:
- 5 sample products (Laptop, Phone, Mouse, Keyboard, Monitor)
- 5 sample users
- Pre-populated shopping carts with items

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

## Security Notes

- **Database Credentials**: Move connection strings to configuration files for production
- **Password Hashing**: Implement proper password hashing for production use
- **Authentication**: Add JWT or similar authentication mechanism
- **Authorization**: Implement role-based access control
- **Input Sanitization**: Add additional input validation and sanitization

## Future Enhancements

- Authentication and Authorization
- Order Management
- Payment Integration
- Product Categories
- User Profiles
- Inventory Management
- Email Notifications
- Admin Dashboard
