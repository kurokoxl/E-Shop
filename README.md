# 🛒 E-Shop API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=c-sharp)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger)

*A modern, scalable e-commerce API built with ASP.NET Core 9.0*


</div>

## ✨ Features

🎯 **Product Management** - Complete CRUD operations with search functionality  
🛍️ **Shopping Cart** - Add, update, and remove items with quantity management  
👥 **User System** - Basic user accounts with cart association  
🗄️ **Database Integration** - MySQL with Entity Framework Core  
📖 **API Documentation** - Interactive Swagger/OpenAPI interface  
🔍 **Input Validation** - Comprehensive request validation and error handling  
⚡ **Async Operations** - Non-blocking database operations throughout  

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core
- **Database**: MySQL 8.0
- **Documentation**: Swagger/OpenAPI
- **Language**: C# 12
- **Architecture**: RESTful API with Repository Pattern

## 📡 API Endpoints

### 🏷️ Products
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/Product` | Get all products |
| `GET` | `/api/Product/{id}` | Get specific product |
| `GET` | `/api/Product/search/{name}` | Search products by name |
| `POST` | `/api/Product` | Create new product |
| `PUT` | `/api/Product` | Update existing product |
| `DELETE` | `/api/Product/{id}` | Delete product |

### 🛒 Shopping Cart
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/CartProduct?userId={id}` | Get user's cart items |
| `POST` | `/api/CartProduct` | Add product to cart |
| `PUT` | `/api/CartProduct` | Update item quantity |
| `DELETE` | `/api/CartProduct` | Remove item from cart |

## 📋 Data Models

### Product Model
```json
{
  "productID": 1,
  "name": "Gaming Laptop",
  "price": 1299.99,
  "stockLevel": 15
}
```

### Add Product Request
```json
{
  "name": "Wireless Mouse",
  "price": 29.99,
  "stockLevel": 100
}
```

### Cart Request
```json
{
  "productID": 1,
  "userID": 1,
  "quantity": 2
}
```

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### 🔧 Setup

1. **📥 Clone the repository**
   ```bash
   git clone https://github.com/kurokoxl/E-Shop.git
   cd E-Shop
   ```

2. **🗄️ Configure Database**
   ```bash
   # Ensure MySQL is running
   # Update connection string in Program.cs if needed
   ```

3. **🔄 Run Migrations**
   ```bash
   dotnet ef database update
   ```

4. **▶️ Start the Application**
   ```bash
   dotnet run
   ```

5. **🌐 Access API Documentation**
   ```
   https://localhost:5001
   ```
   The Swagger UI will provide interactive API documentation.

## 📁 Project Structure

```
📦 E-Shop/
├── 🎛️ Controllers/         # API Controllers
│   ├── ProductController.cs
│   └── CartProductController.cs
├── 🗄️ Data/               # Database Context
│   └── ProductDbContext.cs
├── 📊 Entities/           # Data Models
│   ├── Product.cs
│   ├── User.cs
│   ├── Cart.cs
│   └── CartProduct.cs
├── 🔄 Migrations/         # EF Core Migrations
├── ⚙️ Properties/         # Configuration
└── 🚀 Program.cs         # Application Entry Point
```

## ✅ Code Quality Features

- 📝 **XML Documentation** - Comprehensive inline documentation
- 🛡️ **Input Validation** - Data annotations and model validation
- 🔄 **Error Handling** - Try-catch blocks with proper responses
- 🎯 **Clean Architecture** - Separation of concerns with DTOs
- 💉 **Dependency Injection** - Proper DI container usage
- ⚡ **Async/Await** - Non-blocking operations

## 🗂️ Database Schema

| Entity | Purpose |
|--------|---------|
| **Products** | Store product catalog |
| **Users** | User accounts and authentication |
| **Carts** | Shopping cart containers |
| **CartProducts** | Cart-product relationships |

## 📊 Sample Data

The application includes pre-seeded data:
- 🖥️ **5 Products**: Laptop, Phone, Mouse, Keyboard, Monitor
- 👤 **5 Users**: Test user accounts
- 🛒 **Populated Carts**: Sample cart items


## 🔒 Security & Production Notes

- 🔐 Move connection strings to secure configuration
- 🔑 Implement proper password hashing
- 🎫 Add JWT authentication
- 👮 Implement role-based authorization
- 🧹 Enhanced input sanitization

## 🚀 Future Roadmap

- [ ] 🔐 JWT Authentication & Authorization
- [ ] 📦 Order Management System
- [ ] 💳 Payment Gateway Integration
- [ ] 🏷️ Product Categories & Tags
- [ ] 👤 User Profile Management
- [ ] 📊 Inventory Tracking
- [ ] 📧 Email Notifications
- [ ] 🎛️ Admin Dashboard

---

<div align="center">

**⭐ Star this repository if you found it helpful!**

Made with ❤️ by [kurokoxl](https://github.com/kurokoxl)

</div>
