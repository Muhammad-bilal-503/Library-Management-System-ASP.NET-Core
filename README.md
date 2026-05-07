# 📚 BookVault — Library Management System
### Built with ASP.NET Core 8 + SQLite + Docker

---

## 🚀 Quick Start

### Option 1: Run with Docker (Recommended)

```bash
# 1. Build and start
docker-compose up --build

# 2. Open in browser
http://localhost:8080

# Default login:
# Email:    admin@library.com
# Password: admin123
```

### Option 2: Run Locally (requires .NET 8 SDK)

```bash
dotnet run
# Open: http://localhost:5000
```

---

## ✅ Features

| Feature | Details |
|---|---|
| 📚 Book Management | Add, edit, delete books with cover images from Open Library API |
| 👥 Member Management | Register members with types (Student, Regular, Premium, Staff) |
| 📋 Loan System | Issue/return books, auto fine calculation (Rs. 10/day) |
| 🔐 Authentication | Login/Register with ASP.NET Identity |
| 🐳 Docker | Single Dockerfile + docker-compose.yml |
| 🖼️ Book Covers | Auto-fetched from Open Library REST API using ISBN |

---

## 🏗️ OOP Concepts Applied

- **Encapsulation** — Properties like `IsAvailable`, `Fine`, `Status` computed inside models
- **Inheritance** — `Member` inherits from abstract `Person` class
- **Polymorphism** — `GetRole()` overridden in `Member`
- **Abstraction** — Abstract `Person` base class

---

## 🐳 Docker Commands

```bash
# Build image
docker build -t bookvault .

# Run container
docker run -p 8080:8080 bookvault

# With compose
docker-compose up --build    # start
docker-compose down          # stop
docker-compose logs -f web   # view logs
```

---

## 📁 Project Structure

```
LibraryMS/
├── Controllers/          # MVC Controllers (CRUD logic)
│   ├── HomeController.cs
│   ├── BooksController.cs
│   ├── MembersController.cs
│   ├── LoansController.cs
│   └── AccountController.cs
├── Models/               # OOP Domain Models
│   ├── Book.cs
│   ├── Member.cs         # Inherits Person (abstract)
│   └── Loan.cs
├── Data/
│   └── AppDbContext.cs   # EF Core + Identity + Seed Data
├── Views/                # Razor Views (UI)
│   ├── Books/
│   ├── Members/
│   ├── Loans/
│   ├── Home/
│   ├── Account/
│   └── Shared/_Layout.cshtml
├── Dockerfile
├── docker-compose.yml
├── database_script.sql
└── Program.cs
```
