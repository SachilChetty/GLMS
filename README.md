TechMove GLMS – Part 2 (ASP.NET Core MVC Monolith)
Overview

The TechMove Global Logistics Management System (GLMS) is a web-based application developed using ASP.NET Core MVC.
The system manages Clients, Contracts, and Service Requests while enforcing business rules, file validation, and external API integration.

This version represents the Monolithic Prototype (Part 2), focusing on core business logic, database integration, and unit testing.

Key Features
Client Management
Create and view clients
Store contact details and region
Link clients to contracts
Contract Management
Contracts linked to clients
Status workflow: Draft, Active, Expired, On Hold
Upload signed agreements (PDF only)
Download stored contract files
Service Requests
Created only for active contracts
Prevents invalid requests (Expired/On Hold)
Stores cost in ZAR
Currency API Integration
Uses an external API (ExchangeRate-API)
Converts USD to ZAR dynamically
Integrated using HttpClient
File Handling
Upload restricted to .pdf files
Invalid file types are rejected
Files stored in /wwwroot/uploads
Files downloadable via the user interface
Search and Filtering
Contracts can be filtered by:
Status
Date range (using LINQ queries)
Architecture

The system follows a monolithic architecture consisting of:

User Interface (Views)
Business Logic (Services)
Data Access (Entity Framework Core)
Design Patterns Used
Service Layer Pattern to separate business logic
Dependency Injection to achieve loose coupling
Repository-style usage through Entity Framework
Database
SQL Server (LocalDB)
Entity Framework Core (Code First approach)
Entities
Client
Contract
ServiceRequest
Unit Testing

Unit testing is implemented using xUnit.

Tests include:

Currency conversion logic
File validation (PDF only)
Workflow validation for active contracts
Setup Instructions
Clone the repository:
git clone <your-repo-link>
Open the project in Visual Studio
Run migrations:
Add-Migration InitialCreate
Update-Database
Run the application:
F5
Screenshots (To be added)
Client Creation
Contract Upload
Service Request Creation
Unit Test Results
Video Demonstration

Paste YouTube link here

Notes
Mock data is used (no real documents)
The system is designed for scalability in Part 3 (Docker and API integration)
Author

Sachil Chetty
2026
