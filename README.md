# Pandora
> A social book cataloguing webapp. Track, discover and share what you are reading!

> [!NOTE]
> Pandora is in early development. Come back later! 👋

---

## Tech-stack

The app leverages the following technologies:

- C#, .NET Core, ASP.NET Core
- Swagger
- EntityFramework Core (EF Core), LINQ
- JWT Authentication, Role-based Authorization
- PostgreSQL
- Docker
- xUnit

## Getting started
### 1. Clone the repo
```
git clone https://github.com/garipew/pandora
cd pandora
```

### 2. Run it with Docker (Recommended)
Make sure you have Docker installed, then run:
```
docker compose up --build
```

### 3. Run it locally (Without Docker)
Optionally, you can run Pandora locally. To do that, ensure you have the .NET 9.0 SDK installed and your database is properly configured, then:
```
dotnet restore
dotnet ef database update --project backend/pandora.csproj
dotnet run --project backend/pandora.csproj
```

> [!IMPORTANT]
> Make sure to pass the DB connection string to the app on the env `ConnectionStrings__PandoraContext`

Then, open the OpenAPI docs on `http://localhost:5000/swagger` in your browser

## Running tests
Start the test database, then run the test suite:
```
docker compose up -d db-test
dotnet test
```

## Contributing

Issues and pull requests are welcomed.

## License

This project is licensed under MIT license.
