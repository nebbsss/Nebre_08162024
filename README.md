# Requirements

- .NET 8 
- Ms SQL Server
- Docker
- Visual Studio 2022

# Setup

1. update DefaultConnection in appsettings.Development.json. to use your local machine's mssql connection string (credentials). Please use the format below

```
Server=host.docker.internal,1433;Database=<DatabaseName>;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=true;User Id=<DatabaseUserName>;Password=<DatabasePassword>
```

2. update PG_CONNECTION_STRING in .env.local file to use your local machine's postgres connection string (credentials)

3. run the application in visual studio. Select Container (Dockerfile) option


# ORM

- Entity Framework Core.

# Database Migration

### Migrate

Migrations are automatically run when the application starts


# API

### Upload CSV

Use this API endpoint when uploading CSV file

#### Endpoint: /api/csv/upload
#### Parameter: CSV (The csv file)
#### Method: POST

### Authenticate

Use this API endpoint when generating JWT Token

Endpoint: /api/csv/authenticate
Parameters:		
	- UserName (Please use `admin`)
	- Password (Please use `admin`)
Method: POST



