# SafeX.Company_Module


# SafeX Company Module

A production-ready **Company Panel** developed for the **SafeX Internship Freelancing Platform**. This module enables companies to register, verify their profiles, manage job postings, track applicants, review intern portfolios, and hire interns through a modern and responsive web interface.

---

## 🚀 Features

- Company Registration & Secure Login
- Cookie-Based Authentication
- Company Dashboard
- Company Verification (Document Upload)
- Job Management (Create, Edit, Delete, View)
- Applicant Tracking
- Intern Portfolio View
- Hire Intern Functionality
- Chat Entry Point
- Responsive & Professional UI
- File Upload Management
- Form Validation
- Dashboard Statistics

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core (Code First)
- SQL Server (LocalDB)
- Razor Views
- Bootstrap 5
- JavaScript
- jQuery
- Font Awesome
- SweetAlert2

---

## 📂 Project Structure

```text
SafeX.CompanyPanel
│
├── Controllers
├── Models
├── ViewModels
├── Data
├── Repositories
├── Services
├── Helpers
├── Migrations
├── Views
├── wwwroot
│   ├── css
│   ├── js
│   ├── images
│   └── uploads
├── Program.cs
└── appsettings.json
```

---

## ✨ Main Modules

### Company Authentication
- Company Registration
- Login
- Logout
- Cookie Authentication

### Company Dashboard
- Dashboard Overview
- Statistics Cards
- Recent Jobs
- Recent Applicants

### Company Verification
- Company Logo Upload
- Business License Upload
- Tax Certificate Upload
- CNIC Upload
- Verification Status

### Job Management
- Create Job
- Edit Job
- Delete Job
- View Job Details
- Search & Filtering

### Applicant Management
- View Applicants
- Track Applications
- Review Proposals
- View Portfolio
- Hire Intern

### Chat
- Chat Entry Point for future messaging integration

---

## 🗄️ Database

The project uses **Entity Framework Core Code First** with SQL Server LocalDB.

### Main Entities

- Company
- CompanyVerification
- Job
- Applicant
- Hire

---

## 🎨 User Interface

- Responsive Design
- Bootstrap 5
- Modern Dashboard
- Professional Cards
- Responsive Tables
- Form Validation
- SweetAlert2 Notifications
- Clean Navigation

---

## ⚙️ Getting Started

### Clone Repository

```bash
git clone https://github.com/hasham-mustafa/SafeX.Company_Module.git
```

### Navigate to Project

```bash
cd SafeX.Company_Module
```

### Restore Packages

```bash
dotnet restore
```

### Apply Migrations

```bash
dotnet ef database update
```

### Run Project

```bash
dotnet run
```

---

## 📸 Screenshots

> Add screenshots of:
- Company Registration
- Login
- Dashboard
- Company Verification
- Job Management
- Applicant Tracking
- Portfolio View

---

## 🎯 Learning Outcomes

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- Authentication & Authorization
- Repository Pattern
- Service Layer
- Responsive UI Development
- File Upload Handling
- Clean Architecture
- Full-Stack Development

---

## 👨‍💻 Developer

**Hasham Mustafa**

BSCS Student | ASP.NET Core MVC Developer | Full Stack Web Developer

GitHub: https://github.com/hasham-mustafa

---

## 📄 License

This project was developed for educational and internship purposes.
