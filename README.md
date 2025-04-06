
# WebApplicationMV (.NET 8 Web API)

A modular, scalable Web API built with .NET 8, Entity Framework Core, JWT Authentication.

--------------------------------------------------

## Project Architecture

- .NET Version: .NET 8
- Authentication: JWT Token-based
- ORM: Entity Framework Core (Code-First with Automatic Migrations to MS SQL Server)
- Swagger: For API documentation and testing
- Logging: Serilog (Console + File)

--------------------------------------------------

## Setup Instructions

1. Clone the repository

2. Configure the database:
   Update `appsettings.json` with your SQL Server connection string:
   "ConnectionStrings": {
     "DatabaseConnection": "Server=YOUR_SERVER;Database=CompanyDB;Trusted_Connection=True;"
   }

3. Run migrations:
   The app uses code-first approach and it is configured to auto-apply migration on the first run.

4. Run the project:
   dotnet run --project WebApplicationMV.API

5. Access Swagger UI:
   https://localhost:{PORT}/swagger

--------------------------------------------------

## API Overview

### Endpoints Overview

| Resource   			   | CRUD Support |
|--------------------------|--------------|
| Contacts   			   | Yes          |
| Companies  			   | Yes          |
| Countries  			   | Yes          |
| Authauthentication       | No           |

### Special Functionality

- FilterContacts
  GET /api/contacts/filter?countryId=1&companyId=3
  Filters contacts by optional countryId, companyId, or both.

- GetCompanyStatisticsByCountryId
  GET /api/countries/{countryId}/statistics
  Returns a dictionary of companies and how many contacts they have in the specified country.
  
- Pagination
  Supports `skip` and `take` parameters in GET endpoints 
  There are two GET endpoints one with pagination and one that returns the whole dataset

--------------------------------------------------

## Authentication and Authorization

JWT Secured Endpoints

All endpoints are open except one protected example:

Protected Endpoint:
GET /api/companies/secure
Authorization: Bearer <your_token>

To access:
1. Register or login via /api/authentication endpoint
2. Use the returned JWT token to access protected resources

--------------------------------------------------

# WebApplicationMV (.NET 8 Web API)

A modular, scalable Web API built with .NET 8, Entity Framework Core, JWT Authentication.

--------------------------------------------------

## Project Architecture

- .NET Version: .NET 8
- Authentication: JWT Token-based
- ORM: Entity Framework Core (Code-First with Automatic Migrations to MS SQL Server)
- Swagger: For API documentation and testing
- Logging: Serilog (Console + File)

--------------------------------------------------

## Setup Instructions

1. Clone the repository

2. Configure the database:
   Update `appsettings.json` with your SQL Server connection string:
   "ConnectionStrings": {
     "DatabaseConnection": "Server=YOUR_SERVER;Database=CompanyDB;Trusted_Connection=True;"
   }

3. Run migrations:
   The app uses code-first approach and it is configured to auto-apply migration on the first run.

4. Run the project:
   dotnet run --project WebApplicationMV.API

5. Access Swagger UI:
   https://localhost:{PORT}/swagger

--------------------------------------------------

## API Overview

### Endpoints Overview

| Resource   			   | CRUD Support |
|--------------------------|--------------|
| Contacts   			   | Yes          |
| Companies  			   | Yes          |
| Countries  			   | Yes          |
| Authauthentication       | No           |

### Special Functionality

- FilterContacts
  GET /api/contacts/filter?countryId=1&companyId=3
  Filters contacts by optional countryId, companyId, or both.

- GetCompanyStatisticsByCountryId
  GET /api/countries/{countryId}/statistics
  Returns a dictionary of companies and how many contacts they have in the specified country.
  
- Pagination
  Supports `skip` and `take` parameters in GET endpoints 
  There are two GET endpoints one with pagination and one that returns the whole dataset

--------------------------------------------------

## Authentication and Authorization

JWT Secured Endpoints

All endpoints are open except one protected example:

Protected Endpoint:
GET /api/companies/secure
Authorization: Bearer <your_token>

To access:
1. Register or login via /api/authentication endpoint
2. Use the returned JWT token to access protected resources

--------------------------------------------------
![Screenshot 2025-04-06 214124](https://github.com/user-attachments/assets/35b454d8-6ef3-422f-9f94-fadd7cc21af0)


