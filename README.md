# FootballLeague

A simple ASP.NET Core Web API for managing football teams, matches, and league rankings.

## Features

- CRUD operations for teams
- CRUD operations for matches
- League rankings calculation
- Strategy pattern for point calculation
- EF Core with SQL Server
- Unit tests with NUnit

## Setup

1. Configure the connection string in:

```json
appsettings.json
```

2. Run the application.

Database migrations will be applied automatically on startup.
Default teams will also be seeded automatically.
