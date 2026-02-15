# MovieAppRamos - API (.NET)

Backend para gestion de peliculas y reseñas con arquitectura en capas:

- `src/MovieAppRamos.Api` (Web API)
- `src/MovieAppRamos.Application` (casos de uso)
- `src/MovieAppRamos.Domain` (modelo de dominio)
- `src/MovieAppRamos.Infrastructure` (EF Core, SQL Server, repositorios, migraciones)
- `tests/MovieAppRamos.Application.Tests` (pruebas unitarias)

## Requisitos

- .NET SDK `10.0.x` (en este repo se valido con `10.0.101`)
- SQL Server LocalDB o SQL Server
- PowerShell o terminal compatible

## Configuracion de base de datos

La API usa la cadena de conexion `ConnectionStrings:SqlServer`.

Valor por defecto en `src/MovieAppRamos.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "SqlServer": "Server=(localdb)\\MSSQLLocalDB;Database=MovieAppRamosDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Si quieres cambiarla por variable de entorno:

```powershell
$env:ConnectionStrings__SqlServer="Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True"
```

Para comandos `dotnet ef` (design-time factory), tambien puedes usar:

```powershell
$env:MOVIEAPPRAMOS_SQLSERVER="Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True"
```

## Levantar la API

Desde la raiz del repo:

```powershell
dotnet run --project src/MovieAppRamos.Api/MovieAppRamos.Api.csproj
```

Perfiles de desarrollo (`launchSettings.json`):

- `http://localhost:5252`
- `https://localhost:7027`

Swagger en desarrollo:

- `https://localhost:7027/swagger`
- `http://localhost:5252/swagger`

## Migraciones (EF Core)

Las migraciones viven en `src/MovieAppRamos.Infrastructure/Persistence/Migrations`.

Importante: en este repo los comandos EF se deben correr usando `Infrastructure` como `--project` y `--startup-project`.

### Listar migraciones

```powershell
dotnet ef migrations list `
  --project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj `
  --startup-project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj
```

### Aplicar migraciones a la base de datos

```powershell
dotnet ef database update `
  --project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj `
  --startup-project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj
```

### Crear una nueva migracion

```powershell
dotnet ef migrations add NombreDeLaMigracion `
  --project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj `
  --startup-project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj `
  --output-dir Persistence/Migrations
```

### Eliminar la ultima migracion (si aun no fue aplicada en prod)

```powershell
dotnet ef migrations remove `
  --project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj `
  --startup-project src/MovieAppRamos.Infrastructure/MovieAppRamos.Infrastructure.csproj
```

Si no tienes `dotnet-ef` o esta desactualizado:

```powershell
dotnet tool update --global dotnet-ef
```

## Seed automatico en Development

Cuando la API arranca en `Development`, ejecuta un seeding automatico que:

- aplica migraciones pendientes
- inserta data de ejemplo si la base esta vacia
- crea aprox. 300 peliculas, 1000 reseñas y 45 peliculas deshabilitadas

## Endpoints principales

Base route: `/movies`

- `POST /movies` crea pelicula
- `GET /movies/{id}` obtiene detalle
- `GET /movies` lista paginada (filtros/sort)
- `PATCH /movies/{id}/disable` deshabilita pelicula
- `POST /movies/{id}/reviews` agrega reseña
- `GET /movies/{id}/reviews` lista reseñas paginadas

### Ejemplo: crear pelicula

```http
POST /movies
Content-Type: application/json

{
  "title": "Inception",
  "description": "Sci-fi thriller",
  "releaseDate": "2010-07-16"
}
```

### Ejemplo: listar peliculas con filtros

```http
GET /movies?search=inception&minRating=4&sortBy=RatingAvg&sortDir=Desc&page=1&pageSize=10
```

Valores para `sortBy`: `Title`, `ReleaseDate`, `CreatedAt`, `RatingAvg`.

Valores para `sortDir`: `Asc`, `Desc`.

### Ejemplo: agregar reseña

```http
POST /movies/{movieId}/reviews
Content-Type: application/json

{
  "authorName": "Alex Ramos",
  "rating": 5,
  "comment": "Excelente pelicula"
}
```

Valores para `sort` en `GET /movies/{id}/reviews`:

- `created_asc`
- `rating_desc`
- `rating_asc`
- default: `created_desc`

## Validaciones y errores

- `422` validacion (FluentValidation)
- `404` recurso no encontrado
- `409` regla de negocio (ej: agregar reseña a pelicula deshabilitada)
- `400` formato o argumento invalido
- `500` error inesperado

## Comandos utiles

Compilar solucion:

```powershell
dotnet build MovieAppRamos.sln
```

Pruebas backend:

```powershell
dotnet test MovieAppRamos.sln
```

## Frontend

El frontend React/Vite esta en `webApp`.  
Ver documentacion completa en `webApp/README.md`.
