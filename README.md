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

The base connection string targets local SQL Server and validates its TLS certificate. The Development configuration allows a locally trusted/self-signed SQL Server certificate; do not use that development setting in production. Set `ConnectionStrings__Default` to a production connection whose server certificate is valid.

## Run locally

```powershell
dotnet build
dotnet run --project src/Overtime.Web
dotnet run --project src/Overtime.Admin
```

Storefront: `http://localhost:5036`  
Admin: `http://localhost:5160`

EF Core migrations are applied automatically when either application starts. Demo customers and orders are seeded only when `DemoData:Enabled` is true; this is enabled in the Development settings and disabled by default elsewhere.

## Admin bootstrap

The first administrator is created only when `Admin:Password` is configured and no administrator exists.

Recommended development setup:

```powershell
dotnet user-secrets init --project src/Overtime.Admin
dotnet user-secrets set "Admin:Password" "<strong-local-password>" --project src/Overtime.Admin
```

For deployment, set the same value as the `Admin__Password` environment variable or your host secret manager. The username defaults to `admin` and can be changed with `Admin__Username`.

## Demo limitations

- No SMS gateway is configured, so customer OTP codes are only shown in the development login flow. Codes are not written to logs.
- The payment gateway is scaffolded only and is not registered in the checkout pipeline.
- SQL Server connection and configuration values must be supplied by the deployment environment.
