# TP-PROYECTO-SOFTWARE

Trabajo practico de Proyecto de Software.

Integrantes:
- Agustin Cabeda
- Luciano Beizo

## Descripcion

API REST para un sistema de ticketing de eventos. El proyecto permite:

- listar eventos
- listar sectores por evento
- listar asientos por evento o por sector
- crear usuarios
- hacer login simple de usuarios
- crear reservas de asientos
- consultar reservas por id

La solucion sigue una arquitectura por capas:

- `Domain`
- `Aplication`
- `Infraestructure`
- `API`

## Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Swagger / OpenAPI

## Estructura

- `Domain`: entidades del sistema
- `Aplication`: DTOs, interfaces, validaciones y casos de uso
- `Infraestructure`: DbContext, migraciones, seeds y repositorios
- `API`: controllers, configuracion, middleware y Swagger

## Requisitos

Antes de levantar el proyecto, tener instalado:

- .NET SDK 8
- SQL Server o SQL Server Express
- Visual Studio 2022 o `dotnet` CLI

## Configuracion de base de datos

La API toma la cadena de conexion desde [API/appsettings.json](API/appsettings.json).

Valor actual:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=AGUSTIN\\SQLEXPRESS;Database=TicketingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si el servidor local es distinto, cambiar `Server=...` por la instancia correcta.

## Restaurar y compilar

Desde la raiz del repo:

```powershell
dotnet restore TP-PROYECTO-SOFTWARE.sln
dotnet build TP-PROYECTO-SOFTWARE.sln
```

## Crear o actualizar la base

Para aplicar las migraciones existentes:

```powershell
dotnet ef database update --project Infraestructure\TP-PROYECTO-SOFTWARE.Infraestructure.csproj --startup-project API\TP-PROYECTO-SOFTWARE.API.csproj
```

Notas:

- las migraciones ya estan creadas en `Infraestructure\Migrations`
- el proyecto incluye seeds para:
  - 1 evento
  - 2 sectores
  - 50 asientos por sector

## Ejecutar la API

```powershell
dotnet run --project API\TP-PROYECTO-SOFTWARE.API.csproj
```

## Endpoints principales

### Eventos y catalogo

- `GET /api/v1/events`
- `GET /api/v1/events/{eventId}/sectors`
- `GET /api/v1/events/{eventId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats`

### Usuarios

- `GET /api/v1/users`
- `GET /api/v1/users/{id}`
- `POST /api/v1/users`
- `POST /api/v1/users/login`

### Reservas

- `POST /api/v1/reservations`
- `GET /api/v1/reservations/{id}`

## Estado actual

Backend de Entrega 1 implementado y probado.

Pendiente:

- frontend
- mejoras de Entrega 2 como concurrencia fuerte, confirmación de pago y liberación automática de reservas vencidas
