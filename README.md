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

Valor actual:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=AGUSTIN\\SQLEXPRESS;Database=TicketingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si el servidor local es distinto, cambiar `Server=...` por la instancia correcta.

## Configuracion JWT y roles

La API usa JWT para autenticacion y roles.

Valores actuales en `backend/API/appsettings.json`:

```json
{
  "Jwt": {
    "Key": "TP-Proyecto-Software-Jwt-Key-2026-segura",
    "Issuer": "TP_PROYECTO_SOFTWARE.API",
    "Audience": "TP_PROYECTO_SOFTWARE.Client"
  },
  "AuthorizationSettings": {
    "AdminEmails": [ "agustin@test.com" ]
  }
}
```

Notas:

- cualquier usuario puede registrarse y hacer login
- el usuario cuyo mail este en `AdminEmails` recibe rol `Admin`
- los endpoints administrativos requieren token JWT con rol `Admin`
- `POST /api/v1/users/login` devuelve el token

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
  - 50 asientos por sector

## Ejecutar la API

```powershell
dotnet run --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

## Endpoints principales

### Eventos y catalogo

- `GET /api/v1/events`
- `GET /api/v1/events?name=rock`
- `GET /api/v1/events?eventDate=2026-07-15`
- `GET /api/v1/events?name=rock&eventDate=2026-07-15`
- `POST /api/v1/events` `Admin`
- `DELETE /api/v1/events/{id}` `Admin`
- `GET /api/v1/events/{eventId}/sectors`
- `POST /api/v1/events/{eventId}/sectors` `Admin`
- `DELETE /api/v1/events/{eventId}/sectors/{sectorId}` `Admin`
- `GET /api/v1/events/{eventId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats`
- `POST /api/v1/sectors/{sectorId}/seats` `Admin`
- `POST /api/v1/sectors/{sectorId}/seats/bulk` `Admin`
- `DELETE /api/v1/sectors/{sectorId}/seats/{seatId}` `Admin`

### Usuarios

- `POST /api/v1/users`
- `POST /api/v1/users/login`
- `GET /api/v1/users` `Admin`
- `GET /api/v1/users/{id}` `Admin`

Notas:

- las contraseñas se almacenan hasheadas con `PasswordHasher`
- el login devuelve `id`, `name`, `email`, `role` y `token`

### Reservas

- `POST /api/v1/reservations`
- `GET /api/v1/reservations/{id}`
- `POST /api/v1/reservations/{id}/payment`

Notas:

- al reservar, la butaca pasa a `Reserved` y la reserva a `Pending`
- al confirmar el pago, la butaca pasa a `Sold` y la reserva a `Paid`
- si un evento, sector o asiento tiene reservas asociadas, no se permite su eliminacion

### Reglas de catalogo admin

- un evento no puede tener mas de `5` sectores
- `Capacity` del sector debe ser mayor a `0` y menor o igual a `100`
- no se pueden crear mas asientos que la `Capacity` del sector
- el endpoint bulk de asientos permite crear asientos hasta completar la capacidad disponible del sector
- en bulk, la cantidad de filas no puede superar `10`
- en bulk, la cantidad de asientos por fila debe estar entre `1` y `10`
- en bulk, no se permiten filas repetidas ni butacas duplicadas

Explicacion de reglas de asientos:

- Evento -> hasta 5 sectores
- Sector -> capacidad entre 1 y 100
- Asientos del sector -> no pueden superar esa capacidad

## Estado actual

Backend de Entrega 1 implementado y extendido con autenticacion JWT, roles de administrador y operaciones administrativas sobre catalogo.

Pendiente:

- frontend
- mejoras de Entrega 2 como concurrencia fuerte, confirmación de pago y liberación automática de reservas vencidas
