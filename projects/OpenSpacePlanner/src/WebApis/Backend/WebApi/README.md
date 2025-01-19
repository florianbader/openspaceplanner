# Introduction

## Prerequisites

To configure the web API, ensure that the database connection string is set up via user secrets to avoid committing any secrets:

```bash
dotnet user-secrets set DatabaseConnectionString "Server=<ServerName>;Database=<DatabaseName>;Integrated Security=true"
```

## Setting up the database

When starting the web API in development mode (default if running in debug configuration), the application will automatically apply pending migrations and set up the database based on the configured `DatabaseConnectionString`.

## Debug

To run the web api locally, start it from your IDE or use the terminal with the following command:

```bash
dotnet watch run
```
