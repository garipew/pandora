# Pandora
![cs](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white) ![dotnet](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![postgres](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white) ![swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white) ![docker](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white) ![jwt](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white) ![ts](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white) ![angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white) [![CI](https://github.com/garipew/pandora/actions/workflows/ci.yml/badge.svg)](https://github.com/garipew/pandora/actions/workflows/ci.yml)
> A social book cataloguing webapp. Track, discover and share what you are reading!

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
