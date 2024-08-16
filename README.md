# Requirements

- .NET 8 
- Ms SQL Server
- Docker
- Visual Studio 2022

# Setup

1. update `DefaultConnection` in `appsettings.Development.json`. to use your local machine's mssql connection string (credentials). Please use the format below

```
Server=host.docker.internal,1433;Database=<DatabaseName>;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=true;User Id=<DatabaseUserName>;Password=<DatabasePassword>
```

2. run the application in visual studio. Select `Container (Dockerfile)` option


# ORM

- Entity Framework Core.

# Database Migration

### Migrate

Migrations are automatically run when the application starts

## API Reference

#### Upload

```http
  POST /api/csv/upload
```

| Header    | Type     | Description             |
| :-------- | :------- | :---------------------- |
| `Bearer`  | `string` | **Required**. JWT Token |

| Body      | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `CSV`     | `IFormFile` | **Required**. Csv file to process |

#### Authenticate
```http
  POST /api/csv/authenticate
```

| Body       | Type     | Description                    |
| :--------  | :------- | :----------------------------- |
| `UserName` | `string` | **Required**. Account username |
| `Password` | `string` | **Required**. Account password |

#### Get Users
```http
  GET /api/csv/getusers
```

| Header    | Type     | Description             |
| :-------- | :------- | :---------------------- |
| `Bearer`  | `string` | **Required**. JWT Token |

#### Get CSV Logs
```http
  GET /api/csv/getcsvlogs
```

| Header    | Type     | Description             |
| :-------- | :------- | :---------------------- |
| `Bearer`  | `string` | **Required**. JWT Token |


## File Processing
 The application exclusively handles CSV files and logs specific data during CSV uploads

| Name                | Description												|
| :--------           |  :----------------------								|
| `FileName`          | Filename of the uploaded CSV							|
| `FileSize`          | Filesize of the file (in bytes)							|
| `Duration`          | Processing time of the file (in milliseconds)			|
| `RecordsProcessed`  | Number of records successfully inserted in the database |
| `TotalRecords`      | Total number of rows available in the CSV file			|
| `DateCreated`       | Date and Time the CSV is uploaded in the application	|





