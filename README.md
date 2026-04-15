# 🧱 Cornerstone Digital – E-Commerce Consulting Platform

Cornerstone Digital is a **service-based e-commerce web application** that enables users to browse, purchase, and manage **digital consulting services** such as **website development, UX/UI design, and portfolio creation**.

The platform is built using **C# with ASP.NET**, demonstrating full-stack development capabilities through a dynamic and database-driven architecture.

---

## 🚀 Project Overview

* Developed as part of a **group academic project**
* Focuses on **selling services instead of physical products**
* Simulates a **real-world digital consulting agency**
* Enables users to request services and track project progress

---

## 🎯 Objectives

* Build a **full-stack e-commerce application** using C#
* Implement **service-based purchasing workflows**
* Apply **software engineering best practices**
* Use **GitHub for version control and collaboration**

---

## 🧩 Services Offered

* Website Development
* UX / UI Design
* Portfolio Website Creation
* Digital Consulting
* Website Maintenance & Optimization

---

## ✨ Key Features

* User registration and authentication
* Browse and request consulting services
* Service-based checkout functionality
* Admin dashboard for service and order management
* Database-driven architecture using Entity Framework

---

## 🛠 Technology Stack

### 🔙 Backend

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server

### 🎨 Frontend

* ASP.NET MVC (Razor Views)
* Bootstrap

### 🧰 Tools

* Visual Studio
* Git & GitHub
* SQL Server Management Studio

---

## 📂 Project Structure

The project follows the **ASP.NET MVC architecture**, ensuring a clear separation of concerns:

```
/Controllers      # Handles HTTP requests and application flow
/Models           # Domain models and data entities
/Views            # Razor views (UI layer)
/Data             # Database context and migrations
/Services         # Business logic layer
/wwwroot          # Static files (CSS, JS, images)
/appsettings.json # Application configuration
```

---

## 🧑‍💻 How to Run the Project Locally

### Prerequisites

* Visual Studio 2022 or later
* .NET SDK
* SQL Server / SQL Server Express

### Steps

1. Clone the repository:

```
git clone https://github.com/PhumlileMtshali/CornerstoneDigital.git
```

2. Open the solution in Visual Studio

3. Restore NuGet packages (automatic or manual)

4. Update the database connection string in `appsettings.json`

5. Run database migrations:

```
Update-Database
```

or

```
dotnet ef database update
```

6. Run the application:

* Press **F5** or click **Run**
* Open in browser:

```
https://localhost:xxxx
```

---

## 📄 License

This project was developed for **educational purposes**.
All code and assets are intended for **academic use only**.
