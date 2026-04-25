# TP-PROYECTO-SOFTWARE

Trabajo practico de Proyecto de Software.

Integrantes:
- Agustin Cabeda
- Luciano Beizo

## Descripcion

API REST para un sistema de ticketing de eventos. El proyecto permite:

- Listar eventos
- Crear y eliminar eventos, sectores y asientos con usuario administrador
- Listar sectores por evento
- Listar asientos por evento o por sector
- Crear usuarios
- Hacer login con JWT
- Crear reservas de asientos
- Confirmar pago simulado de reservas
- Consultar reservas por id

La solucion sigue una arquitectura por capas dentro de `backend`:

- `backend/Domain`
- `backend/Aplication`
- `backend/Infraestructure`
- `backend/API`

## Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- AutoMapper
- FluentValidation
- JWT Bearer Authentication
- Swagger / OpenAPI

## Estructura

- `backend/Domain`: entidades del sistema
- `backend/Aplication`: DTOs, interfaces, validaciones y casos de uso
- `backend/Infraestructure`: DbContext, migraciones, seeds y repositorios
- `backend/API`: controllers, configuracion, middleware y Swagger

## Requisitos

Antes de levantar el proyecto, tener instalado:

- .NET SDK 8
- SQL Server o SQL Server Express
- Visual Studio 2022 o `dotnet` CLI

## Configuracion de base de datos

La API toma la cadena de conexion desde [backend/API/appsettings.json](backend/API/appsettings.json).

Ejemplo:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=AGUSTIN\\SQLEXPRESS;Database=TicketingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si el servidor local es distinto, cambiar `Server=...` por la instancia correcta.

## Configuracion JWT, Identity, roles y reglas

La API usa ASP.NET Core Identity para registro, login, hash de contraseñas y gestión de roles. La autenticación de la API sigue siendo con JWT, y además se usan reglas configurables para el catálogo.

Ejemplo de configuracion en `backend/API/appsettings.json`:

```json
{
  "Jwt": {
    "Key": "TP-Proyecto-Software-Jwt-Key-2026-segura",
    "Issuer": "TP_PROYECTO_SOFTWARE.API",
    "Audience": "TP_PROYECTO_SOFTWARE.Client"
  },
  "AuthorizationSettings": {
    "AdminEmails": [ "admintest@test.com" ]
  },
  "TicketingRules": {
    "MaxSectorsPerEvent": 5,
    "MaxSectorCapacity": 200,
    "MaxRowsPerBulkCreate": 10,
    "MaxSeatsPerRow": 20,
    "RowLabels": [ "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" ]
  }
}
```

Notas:

- cualquier usuario puede registrarse y hacer login
- el catalogo de eventos, sectores y asientos es publico
- las reservas y los pagos requieren usuario autenticado
- Identity guarda usuarios, contraseñas hasheadas y roles en base de datos
- el usuario cuyo mail este en `AdminEmails` recibe rol `Admin` al registrarse o al iniciar la API si ya existe
- los endpoints administrativos requieren token JWT con rol `Admin`
- `POST /api/v1/users/login` valida credenciales con Identity y devuelve el token
- las reglas de sectores y asientos se leen desde `TicketingRules`

## Restaurar y compilar

Desde la raiz del repo:

```powershell
dotnet restore TP-PROYECTO-SOFTWARE.sln
dotnet build TP-PROYECTO-SOFTWARE.sln
```

## Crear o actualizar la base

Para aplicar las migraciones existentes:

```powershell
dotnet ef database update --project backend\Infraestructure\TP-PROYECTO-SOFTWARE.Infraestructure.csproj --startup-project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

Notas:

- las migraciones ya estan creadas en `backend\Infraestructure\Migrations`
- el proyecto incluye seeds para:
  - 1 evento
  - 2 sectores
  - 50 asientos por sector en los datos iniciales

## Ejecutar la API

```powershell
dotnet run --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

## Endpoints principales

### Acceso

- Publicos:
  - `POST /api/v1/users`
  - `POST /api/v1/users/login`
  - `GET /api/v1/events`
  - `GET /api/v1/events/{id}`
  - `GET /api/v1/events/{eventId}/sectors`
  - `GET /api/v1/events/{eventId}/sectors/{sectorId}`
  - `GET /api/v1/events/{eventId}/seats`
  - `GET /api/v1/sectors/{sectorId}/seats`
  - `GET /api/v1/sectors/{sectorId}/seats/{seatId}`
- Requieren autenticacion:
  - `GET /api/v1/users/me`
  - `POST /api/v1/reservations`
  - `GET /api/v1/reservations/{id}`
  - `POST /api/v1/payments`
- Solo `Admin`:
  - `POST /api/v1/events`
  - `DELETE /api/v1/events/{id}`
  - `POST /api/v1/events/{eventId}/sectors`
  - `DELETE /api/v1/events/{eventId}/sectors/{sectorId}`
  - `POST /api/v1/sectors/{sectorId}/seats`
  - `POST /api/v1/sectors/{sectorId}/seats/bulk`
  - `DELETE /api/v1/sectors/{sectorId}/seats/{seatId}`
  - `GET /api/v1/audit-logs`
  - `GET /api/v1/users`
  - `GET /api/v1/users/{id}`

### Eventos y catalogo

- `GET /api/v1/events`
- `GET /api/v1/events/{id}`
- `GET /api/v1/events?name=rock`
- `GET /api/v1/events?eventDate=2026-07-15`
- `GET /api/v1/events?name=rock&eventDate=2026-07-15`
- `POST /api/v1/events` `Admin`
- `DELETE /api/v1/events/{id}` `Admin`
- `GET /api/v1/events/{eventId}/sectors`
- `GET /api/v1/events/{eventId}/sectors/{sectorId}`
- `POST /api/v1/events/{eventId}/sectors` `Admin`
- `DELETE /api/v1/events/{eventId}/sectors/{sectorId}` `Admin`
- `GET /api/v1/events/{eventId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats/{seatId}`
- `POST /api/v1/sectors/{sectorId}/seats` `Admin`
- `POST /api/v1/sectors/{sectorId}/seats/bulk` `Admin`
- `DELETE /api/v1/sectors/{sectorId}/seats/{seatId}` `Admin`

### Usuarios

- `POST /api/v1/users` `Publico`
- `POST /api/v1/users/login` `Publico`
- `GET /api/v1/users/me` `Autenticado`
- `GET /api/v1/users` `Admin`
- `GET /api/v1/users/{id}` `Admin`

Notas:

- las contraseñas se almacenan hasheadas mediante ASP.NET Core Identity
- los roles `Admin` y `User` se persisten en la base de datos
- el login devuelve `id`, `name`, `email`, `role` y `token`

### Reservas

- `POST /api/v1/reservations` `Autenticado`
- `GET /api/v1/reservations/{id}` `Autenticado`
- `POST /api/v1/payments` `Autenticado`

Notas:

- al reservar, la butaca pasa a `Reserved` y la reserva a `Pending`
- al confirmar el pago, la butaca pasa a `Sold` y la reserva a `Paid`
- `POST /api/v1/payments` recibe `reservationId` en el body
- si un evento, sector o asiento tiene reservas asociadas, no se permite su eliminacion

### Auditoria

- `GET /api/v1/audit-logs` `Admin`

Notas:

- el listado y los filtros usan el mismo endpoint `GET /api/v1/audit-logs`
- los filtros se envian por query string
- `date` filtra un dia exacto
- `date` no se combina con `dateFrom` ni `dateTo`

Ejemplos:

- `GET /api/v1/audit-logs?userId=3`
- `GET /api/v1/audit-logs?date=2026-05-24`
- `GET /api/v1/audit-logs?dateFrom=2026-04-01&dateTo=2026-04-23`
- `GET /api/v1/audit-logs?userId=3&date=2026-05-24`
- `GET /api/v1/audit-logs?userId=3&dateFrom=2026-04-01&dateTo=2026-04-23`

### Reglas de catalogo admin

- un evento no puede tener mas sectores que `TicketingRules.MaxSectorsPerEvent`
- `Capacity` del sector debe ser mayor a `0` y menor o igual a `TicketingRules.MaxSectorCapacity`
- no se pueden crear mas asientos que la `Capacity` del sector
- el endpoint bulk de asientos permite crear asientos hasta completar la capacidad disponible del sector
- en bulk, la cantidad de filas no puede superar `TicketingRules.MaxRowsPerBulkCreate`
- en bulk, la cantidad de asientos por fila debe estar entre `1` y `TicketingRules.MaxSeatsPerRow`
- las filas validas para asientos individuales y bulk salen de `TicketingRules.RowLabels`
- no se permiten butacas duplicadas dentro del sector

Explicacion de reglas de asientos:

- Evento -> hasta 5 sectores
- Sector -> capacidad segun `TicketingRules.MaxSectorCapacity`
- Asientos del sector -> no pueden superar esa capacidad
- Configuracion actual -> 10 filas maximas (`A` a `J`) x 20 asientos maximos por fila = 200 asientos por sector
- Filas validas -> segun `TicketingRules.RowLabels`

## Estado actual

Backend de Entrega 1 implementado y extendido con ASP.NET Core Identity, autenticacion JWT, roles de administrador y operaciones administrativas sobre catalogo.

Pendiente:

- frontend
- mejoras de Entrega 2 como concurrencia fuerte y liberación automática de reservas vencidas
