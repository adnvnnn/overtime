# Contributing

## Development setup

Install the .NET 10 SDK and SQL Server, then follow the local setup instructions in [README.md](README.md).

## Changes

- Keep storefront and admin UI text and behavior consistent with the existing Persian RTL design.
- Add database schema changes through EF Core migrations in `src/Overtime.Data`.
- Do not commit credentials, local settings, logs, or generated build output.
- Run `dotnet build` before submitting a change.

## Pull requests

Describe the user-facing change, note any configuration or migration requirements, and include relevant screenshots for UI changes.
