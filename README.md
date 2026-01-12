# 🎮 GameStore API

A **production-ready ASP.NET Core 9 Web API** built with modern backend best practices, featuring **JWT authentication, role- and policy-based authorization, clean architecture, and secure configuration management**.

---

## 🚀 Project Overview

**GameStore API** is a backend service for managing games and genres with secure access control.  
It demonstrates **real-world backend architecture**, emphasizing scalability, maintainability, and security.  
The API uses **Entity Framework Core** for data access, **AutoMapper** for DTO mapping, and **Scalar** for API exploration.

---

## ✨ Key Features

### 🔐 Authentication & Authorization
- JWT-based authentication
- Access & refresh token implementation
- Role-based authorization (`Admin`, `User`)
- Policy-based authorization
- Admin-only protected endpoints
- Secure token validation and lifecycle management

### 👤 User Management
- User registration
- Login
- Refresh token
- Admin registration (**accessible only by Admins**)
- Automatic first admin seeding on initial run (if no admin exists)

### 🎮 Game & Genre Management
- Full CRUD operations for **Games**
- Full CRUD operations for **Genres**
- One-to-many relationship (**Genre → Games**)
- Only authenticated users can **Create** and **Update**
- Only Admin users can **Delete**

### 🧱 Architecture & Code Quality
- Clean separation of concerns: Controllers → Interfaces → Services
- Business logic and database access handled exclusively in services
- Dedicated DTOs for requests and responses
- AutoMapper for entity ↔ DTO mapping and update operations
- Centralized global exception handling
- Configuration logic extracted into extension methods

---

## 🛠️ Tech Stack

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Policy-based Authorization**
- **AutoMapper**
- **Scalar** (used instead of Swagger)
- **User Secrets** (for secure local development)

---

## 🗄️ Database Design

- **Genre → Games** (One-to-Many)
- Code-first approach using Entity Framework Core
- Automatic admin seeding during first application run

---

## 🔑 Security Practices

- Secrets never committed to source control
- JWT Secret Key stored securely using **User Secrets**
- Database connection string hidden
- Environment-based configuration
- Role and policy checks enforced at API level

---

## ⚙️ Configuration

### appsettings.json (Commit-safe)
```json
{
  "Jwt": {
    "Issuer": "GameStoreApi",
    "Audience": "GameStoreClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "ConnectionStrings": {
    "GameStore": ""
  }
}
```

## User Secrets (Development)

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:SecretKey" "YOUR_SUPER_SECURE_SECRET_KEY"
dotnet user-secrets set "ConnectionStrings:GameStore" "YOUR_DB_CONNECTION_STRING"
```

## ▶️ Running the Project

1. Clone the repository

2. Configure User Secrets

3. Update the database (if migrations exist)

4. Run the application

```bash
dotnet run
```

## 🧪 API Testing

- Scalar UI is enabled for API exploration

- JWT Bearer authentication supported

- Role & policy restrictions enforced at endpoint level

## 📂 Project Structure 

```plaintext
GameStoreApi
│
├── Controllers
├── Services
├── Interfaces
├── DTOs
├── Entities
├── Mappings
├── Extensions
├── Middleware
├── Data
└── Program.cs
```

## 📄 License

This project is for learning and demonstration purposes.

## 🙌 Author

GameStore API

Built with ❤️ using ASP.NET Core 9