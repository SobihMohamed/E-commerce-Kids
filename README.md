# 🛒 Mine Store E-Commerce API

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Clean_Architecture-Success?style=for-the-badge)

## 📖 Overview
A robust, enterprise-level backend API for an e-commerce platform specializing in customizable garments and children's clothing. Built with **.NET Core** and adhering strictly to **Clean Architecture (Onion Architecture)** principles, this system features a specialized workflow for applying physical cartoon graphics to base t-shirts, alongside seamless order processing, secure authentication, and comprehensive inventory management.

## ✨ Key Features
* **Custom Garment Engine:** A dedicated module allowing users to apply custom physical graphics and designs to base garments, separate from standard product variants.
* **Advanced Product Management:** Efficient handling of complex product variants (colors, sizes) with a strong focus on preventing ORM tracking conflicts and ensuring data integrity.
* **Secure Authentication & Authorization:** Implemented using ASP.NET Core Identity, featuring role-based access control, OTP validation, and secure password management policies.
* **Robust Order Processing:** Manages the complete transactional lifecycle—from shopping cart operations and stock validation to final order placement and invoice generation.
* **Automated Notifications:** Integrated SMTP email services (via MailKit) to send beautifully formatted HTML updates and push notifications to both customers and administrators upon order status changes.
* **Clean Data Handling:** Implements smart strategies for handling data constraints (e.g., unique phone numbers) without data loss, ensuring a smooth user experience.

## 🛠️ Tech Stack & Technologies
* **Framework:** .NET (ASP.NET Core Web API)
* **ORM / Database:** Entity Framework Core, Microsoft SQL Server
* **Architecture & Patterns:** Clean Architecture, Repository Pattern, Unit of Work, SOLID Principles, Dependency Injection
* **Security:** ASP.NET Core Identity, JWT Authentication
* **Utilities:** AutoMapper, MailKit (SMTP), MimeKit

## 🏗️ Architectural Highlights
The project architecture is designed for maximum maintainability and scalability. The Core (Domain) and Application (Service) layers contain all business logic and are completely isolated from external concerns. The Persistence layer depends entirely on abstractions, strictly adhering to the Dependency Inversion Principle. The implementation actively avoids common EF Core tracking pitfalls, ensuring high performance and reliable state management during complex updates.

## 🚀 Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation
Clone the repository:
   ```bash
   git clone [https://github.com/yourusername/MineStore-API.git](https://github.com/yourusername/MineStore-API.git)
