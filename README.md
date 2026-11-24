# Overtime (اورتایم)

An RTL Persian sneaker e-commerce demo built with ASP.NET Core Blazor Server on .NET 10.

## Projects

- `src/Overtime.Web` - customer storefront
- `src/Overtime.Admin` - back-office administration
- `src/Overtime.Data` - EF Core database context, entities, migrations, and demo seed data

Both applications use the same SQL Server database.

## Requirements

- .NET 10 SDK
- SQL Server

The default connection string uses `Server=.;Database=Overtime;Trusted_Connection=True;TrustServerCertificate=True`. Override `ConnectionStrings__Default` for another environment.

## Run locally

```powershell
dotnet build
dotnet run --project src/Overtime.Web
dotnet run --project src/Overtime.Admin
```

Storefront: `http://localhost:5036`  
Admin: `http://localhost:5160`

EF Core migrations are applied automatically when either application starts.

## Admin bootstrap

The first administrator is created only when `Admin:Password` is configured and no administrator exists.

Recommended development setup:

```powershell
dotnet user-secrets init --project src/Overtime.Admin
dotnet user-secrets set "Admin:Password" "<strong-local-password>" --project src/Overtime.Admin
```

For deployment, set the same value as the `Admin__Password` environment variable or your host secret manager. The username defaults to `admin` and can be changed with `Admin__Username`.

## Demo limitations

- Customer OTP codes are written to application logs in development; no SMS gateway is configured.
- The payment gateway is scaffolded only and is not registered in the checkout pipeline.
- SQL Server connection and configuration values must be supplied by the deployment environment.
