SafeX Company Module

A production-ready Company Panel developed for the SafeX Internship Freelancing Platform. The module currently enables companies to register, authenticate, verify their profiles, manage job postings, track applicants, review intern portfolios, and hire interns through a modern and responsive web interface.

The project is currently being expanded into a complete internship and employment marketplace with dedicated Company, Applicant, and Super Admin functionality.

Project Overview

SafeX is being developed as a platform that connects companies with applicants and interns through a structured job marketplace.

The current Company Module provides the foundation for company-side operations. The next stage of development introduces Super Admin management, company approval, applicant job discovery, applications, recruitment workflows, and additional platform-level features.

Current Development Status

The existing Company Module is functional and includes company authentication, verification, job management, applicant tracking, portfolio viewing, and hiring functionality.

The Super Admin foundation has now been added as the first step toward expanding the project into a complete multi-role platform.

Completed Features

Company Registration and Authentication

- Company registration
- Secure company login
- Cookie-based authentication
- Company logout
- Authentication-based access control

Company Dashboard

- Dashboard overview
- Company statistics
- Recent jobs
- Recent applicants
- Company activity overview

Company Verification

- Company verification submission
- Company logo upload
- Business license upload
- Tax certificate upload
- CNIC upload
- Verification status tracking

Job Management

- Create jobs
- Edit jobs
- Delete jobs
- View job details
- Job search and filtering
- Job status management

Applicant Management

- View applicants
- Track applications
- Review proposals
- View applicant portfolios
- Hire interns

Chat

- Chat entry point prepared for future messaging functionality

Super Admin Foundation

The first stage of the Super Admin system has been implemented to support centralized platform administration.

Completed Super Admin foundation includes:

- Admin entity
- Admin database configuration
- Admin email validation
- Unique Admin email constraint
- Admin account activation status
- Admin account timestamps
- Last login tracking
- Admin database table
- Entity Framework Core migration for the Admin entity

The Super Admin authentication and administration dashboard will be implemented in the next development stage.

Planned Super Admin Features

- Super Admin login
- Super Admin authentication and authorization
- Super Admin dashboard
- Company verification review
- Company approval and rejection
- Company management
- Job moderation
- Applicant management
- Platform statistics
- Platform activity monitoring
- Administrative controls

Planned Company Verification Workflow

The planned verification workflow is:

Company Registration

    ↓

Company Verification Submission

    ↓

Super Admin Review

    ↓

Approve or Reject

    ↓

Verified Company

    ↓

Company Can Publish Jobs

Only approved and verified companies will be allowed to publish jobs on the applicant-facing marketplace.

Companies will be able to manage their own jobs after approval, including creating, editing, closing, and deleting job postings.

Planned Applicant System

The Applicant module will allow applicants to use SafeX as a job discovery and application platform.

Planned applicant features include:

- Applicant registration
- Applicant login
- Applicant profile
- Skills and qualifications
- Resume or CV management
- Portfolio management
- Job discovery
- Job search
- Job filtering
- Job details
- Job applications
- Application tracking
- Application status
- Saved jobs
- Shortlisting
- Recruitment communication

Planned Job Marketplace

The platform will provide an applicant-facing job marketplace where approved company jobs are displayed.

The planned marketplace will include:

- Job listings
- Job search
- Location filtering
- Category filtering
- Skills filtering
- Internship and job type filtering
- Salary or stipend filtering
- Experience filtering
- Job details
- Company information
- Application functionality
- Saved jobs
- Job recommendations

Platform Architecture

SafeX is being developed around three primary user roles:

Company

Companies will be responsible for:

- Creating and managing their company profile
- Completing verification
- Posting jobs after approval
- Managing job postings
- Reviewing applicants
- Shortlisting applicants
- Communicating with applicants
- Hiring applicants

Applicant

Applicants will be responsible for:

- Creating their profile
- Managing skills and qualifications
- Creating portfolios
- Discovering job opportunities
- Applying for jobs
- Tracking applications
- Communicating with companies
- Managing their recruitment activity

Super Admin

Super Admin will be responsible for:

- Managing the platform
- Reviewing company registrations
- Approving or rejecting companies
- Monitoring companies
- Managing platform activity
- Moderating job postings
- Managing platform-level operations
- Monitoring applicants and recruitment activity

Technology Stack

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core
- Code First Database Development
- SQL Server LocalDB
- Razor Views
- Bootstrap 5
- JavaScript
- jQuery
- Font Awesome
- SweetAlert2

Architecture and Development Patterns

The project follows a structured application architecture using:

- MVC architecture
- Repository Pattern
- Service Layer
- Dependency Injection
- Entity Framework Core
- Fluent Entity Configuration
- Cookie-Based Authentication
- ViewModels
- Data Validation
- Code First Migrations

Project Structure

SafeX.CompanyPanel

    Controllers
    Models
    ViewModels
    Data
    Repositories
    Services
    Helpers
    Extensions
    Migrations
    Views
    wwwroot
        css
        js
        images
        uploads
    Program.cs
    appsettings.json

Database

The project uses Entity Framework Core Code First with SQL Server LocalDB.

Current main entities include:

- Company
- CompanyVerification
- Job
- Applicant
- Hire
- Admin

The Admin entity was added as part of the Super Admin foundation and includes:

- Admin identification
- Full name
- Email
- Password hash
- Active account status
- Creation timestamp
- Update timestamp
- Last login timestamp

Database migrations are maintained through Entity Framework Core migrations.

Current Development Roadmap

Phase 1: Super Admin Foundation

Completed:

- Admin entity
- Admin configuration
- Admin DbSet
- Admin database table
- Unique Admin email constraint
- Admin migration

Phase 2: Super Admin Authentication

Planned:

- Admin login
- Secure password handling
- Admin cookie authentication
- Admin authorization
- Admin logout
- Admin session management

Phase 3: Super Admin Dashboard

Planned:

- Admin dashboard
- Company statistics
- Pending verification statistics
- Job statistics
- Applicant statistics
- Platform activity overview

Phase 4: Company Approval System

Planned:

- View pending companies
- Review company information
- Review submitted documents
- Approve company
- Reject company
- Rejection reason
- Verification status management

Phase 5: Applicant Marketplace

Planned:

- Applicant registration
- Applicant authentication
- Applicant profiles
- Job marketplace
- Job search
- Job filtering
- Job details
- Job applications
- Application tracking

Phase 6: Recruitment System

Planned:

- Applicant shortlisting
- Application status management
- Interview management
- Hiring workflow
- Recruitment communication
- Notifications

Phase 7: Advanced Platform Features

Planned:

- Saved jobs
- Advanced search
- Company ratings
- Applicant ratings
- Notifications
- Messaging
- Recruitment analytics
- Platform analytics
- Trust and safety features
- Advanced company verification
- AI-powered platform features

Development Approach

SafeX is being developed incrementally.

Each major feature is implemented, tested, documented, and then added to the repository before moving to the next feature.

The development process follows:

- Feature planning
- Architecture design
- Implementation
- Database changes
- Testing
- Bug fixing
- Documentation
- Git commit
- Repository update

Getting Started

Clone the repository

    git clone https://github.com/hasham-mustafa/SafeX.Company_Module.git

Navigate to the project

    cd SafeX.Company_Module

Restore packages

    dotnet restore

Apply database migrations

    dotnet ef database update

Run the project

    dotnet run

User Interface

The project uses a responsive interface based on Bootstrap 5 and Razor Views.

Current interface features include:

- Responsive dashboard
- Navigation system
- Statistics cards
- Responsive tables
- Job management screens
- Applicant management screens
- Verification forms
- File upload interfaces
- Form validation
- SweetAlert2 notifications

Screenshots

Screenshots will be added as the platform continues to develop.

Planned screenshots include:

- Company Registration
- Company Login
- Company Dashboard
- Company Verification
- Job Management
- Applicant Tracking
- Applicant Portfolio
- Super Admin Login
- Super Admin Dashboard
- Company Approval
- Applicant Job Marketplace
- Job Details
- Application Management

Learning Outcomes

This project provides practical experience with:

- ASP.NET Core MVC
- .NET 8
- C#
- Entity Framework Core
- SQL Server
- Code First Development
- Authentication and Authorization
- Cookie Authentication
- Repository Pattern
- Service Layer
- Dependency Injection
- Fluent API Configuration
- Database Migrations
- File Upload Management
- Responsive UI Development
- Full-Stack Application Development
- Multi-role Platform Architecture

Developer

Hasham Mustafa

BSCS Student | ASP.NET Core MVC Developer | Full Stack Web Developer

GitHub Repository: hasham-mustafa/SafeX.Company_Module

License

This project was developed for educational and internship purposes.