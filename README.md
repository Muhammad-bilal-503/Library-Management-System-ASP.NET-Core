<div align="center">

<img src="https://img.shields.io/badge/-.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/-ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/-Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" />
<img src="https://img.shields.io/badge/-SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white" />
<img src="https://img.shields.io/badge/-Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />

<br /><br />

# 📚 BookVault
### Library Management System

**A full-stack, containerized web application built with ASP.NET Core 8, Entity Framework Core, and Docker.**  
Manage books, members, and loans — with automatic cover images from the Open Library REST API.

<br />

</div>

---

## ✨ Features

| Module | Capabilities |
|--------|-------------|
| 📚 **Book Management** | Add, edit, delete, and search books · Auto cover images via ISBN (Open Library API) |
| 👥 **Member Management** | Register members · Membership types: Student, Regular, Premium, Staff |
| 📋 **Loan System** | Issue & return books · 14-day loan period · Rs. 10/day overdue fine calculation |
| 🔐 **Authentication** | Secure login & registration via ASP.NET Core Identity |
| 🐳 **Docker** | Multi-stage Dockerfile · One-command deploy with Docker Compose |
| 📊 **Dashboard** | Real-time stats — total books, members, active loans, overdue alerts |

---

## 🏗️ OOP Principles

This project explicitly demonstrates all four pillars of Object-Oriented Programming:

```
Encapsulation  →  IsAvailable, Fine, IsOverdue, Status — computed inside model classes
Inheritance    →  Member inherits from abstract base class Person
Polymorphism   →  GetRole() is abstract in Person, overridden in Member
Abstraction    →  Person cannot be instantiated — enforces a contract on subclasses
```

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — for containerized deployment
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) — for local development only

---

### ▶ Option 1 — Docker (Recommended)

```bash
# 1. Clone or extract the project
cd LibraryMS

# 2. Build and start the container
docker-compose up --build

# 3. Open in your browser
http://localhost:8080
```

> Make sure **Docker Desktop is running** before executing the command.

---

### ▶ Option 2 — Run Locally

```bash
# Restore dependencies and run
dotnet run

# Open in your browser
http://localhost:5000
```

---

### 🔑 Default Credentials

```
Email    :  admin@library.com
Password :  admin123
```

---

## 🐳 Docker Commands Reference

```bash
# Build the Docker image
docker build -t bookvault .

# Run as a standalone container
docker run -p 8080:8080 bookvault

# Start with Docker Compose
docker-compose up --build

# Stop and remove containers
docker-compose down

# View live application logs
docker-compose logs -f web
```

---

## 📁 Project Structure

```
LibraryMS/
│
├── Controllers/                  # Request handling & business logic (MVC)
│   ├── HomeController.cs         # Dashboard stats
│   ├── BooksController.cs        # Full CRUD for books
│   ├── MembersController.cs      # Full CRUD for members
│   ├── LoansController.cs        # Issue / return books
│   └── AccountController.cs     # Login / register
│
├── Models/                       # Domain models (OOP)
│   ├── Book.cs                   # Encapsulated availability & status
│   ├── Member.cs                 # Inherits from abstract Person
│   └── Loan.cs                   # Encapsulated fine & overdue logic
│
├── Data/
│   └── AppDbContext.cs           # EF Core DbContext + Identity + Seed data
│
├── Views/                        # Razor Views (UI layer)
│   ├── Books/                    # Index, Details, Create, Edit, Delete
│   ├── Members/                  # Index, Details, Create, Edit, Delete
│   ├── Loans/                    # Index, Issue
│   ├── Home/                     # Dashboard
│   ├── Account/                  # Login, Register
│   └── Shared/
│       └── _Layout.cshtml        # Global dark-gold themed layout
│
├── Dockerfile                    # Multi-stage build (SDK → Runtime)
├── docker-compose.yml            # Container orchestration
├── database_script.sql           # Database schema + seed SQL
├── appsettings.json              # App configuration
└── Program.cs                    # App startup, DI, middleware pipeline
```

---

## 🔌 API Integration

Book cover images are fetched automatically from the **[Open Library API](https://openlibrary.org/dev/docs/api)** — no API key required.

```
https://covers.openlibrary.org/b/isbn/{ISBN}-M.jpg
```

When a book is added or edited, the ISBN is used to construct the cover URL. If no cover exists, a placeholder is shown gracefully.

---

## 🛢️ Database

- **Engine:** SQLite (file-based, zero configuration)
- **ORM:** Entity Framework Core 8
- **Migration:** `Database.EnsureCreated()` on startup — no manual migration needed
- **Seed Data:** 6 books and 2 members are pre-loaded automatically

---

## 📦 Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# 12 |
| Framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| Database | SQLite |
| Auth | ASP.NET Core Identity |
| Containerization | Docker + Docker Compose |
| External API | Open Library REST API |
| UI Styling | Bootstrap 5 + Custom CSS |

---

## 📄 License

This project was developed as an **Open-Ended Lab submission** for a Software Engineering course.  
Free to use for educational purposes.

---

<div align="center">
  <sub>Built with ❤️ using ASP.NET Core 8 · Entity Framework · Docker</sub>
</div>
