# TP-PROYECTO-SOFTWARE

Trabajo práctico de Proyecto de Software.

Integrantes:
- Agustin Cabeda
- Luciano Beizo

## Descripción

Sistema de ticketing de eventos con:
- backend en ASP.NET Core Web API
- frontend en React + Vite
- autenticación con JWT
- roles `Admin` y `User`
- catálogo de eventos, sectores y butacas
- reservas y pago simulado
- vista de compra y `Mis entradas`

La solución sigue una arquitectura por capas dentro de `backend`:
- `backend/Domain`
- `backend/Aplication`
- `backend/Infraestructure`
- `backend/API`

## Stack

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- AutoMapper
- FluentValidation
- JWT Bearer Authentication
- Swagger / OpenAPI

### Frontend
- React
- Vite
- React Router
- Bootstrap
- React DatePicker

## Estructura

### Backend
- `backend/Domain`: entidades del sistema
- `backend/Aplication`: DTOs, interfaces, validaciones y casos de uso
- `backend/Infraestructure`: DbContext, migraciones, seeds y repositorios
- `backend/API`: controllers, configuración, middleware y Swagger

### Frontend
- `frontend/src/pages`: vistas principales
- `frontend/src/components`: componentes reutilizables
- `frontend/src/services`: acceso a la API
- `frontend/src/hooks`: lógica reutilizable
- `frontend/src/lib`: utilidades, i18n y formateo
- `frontend/css`: estilos separados por feature

## Requisitos

Antes de levantar el proyecto, tener instalado:
- .NET SDK 8
- SQL Server o SQL Server Express
- Node.js 18+
- npm

## Configuración del backend

La API toma la cadena de conexión desde:
- [backend/API/appsettings.json](backend/API/appsettings.json)
- o [backend/API/appsettings.Development.json](backend/API/appsettings.Development.json)

Ejemplo:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=AGUSTIN\\SQLEXPRESS;Database=TicketingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si el servidor local es distinto, cambiar `Server=...` por la instancia correcta.

### JWT, roles y reglas

Ejemplo de configuración:

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
- el catálogo de eventos, sectores y butacas es público
- las reservas, `Mis entradas` y el pago requieren usuario autenticado
- el usuario cuyo mail esté en `AdminEmails` recibe rol `Admin`
- los endpoints administrativos requieren JWT con rol `Admin`

## Configuración del frontend

La URL base de la API está centralizada en:
- [frontend/src/lib/api.js](frontend/src/lib/api.js)

Acepta variable de entorno de Vite:

```env
VITE_API_BASE_URL=https://localhost:7176/api/v1
```

Si no existe `.env`, usa este fallback:

```txt
https://localhost:7176/api/v1
```

## Restaurar y compilar backend

Desde la raíz del repo:

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
- las migraciones ya están creadas en `backend\Infraestructure\Migrations`
- el proyecto incluye seeds base

## Ejecutar backend

```powershell
dotnet run --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

## Ejecutar frontend

```powershell
cd frontend
npm install
npm run dev
```

Build de producción:

```powershell
cd frontend
npm run build
```

## Endpoints actuales

### Públicos

- `GET /api/v1/events`
- `GET /api/v1/events/{id}`
- `GET /api/v1/events/{eventId}/sectors`
- `GET /api/v1/events/{eventId}/sectors/{sectorId}`
- `GET /api/v1/events/{eventId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats/{seatId}`
- `POST /api/v1/users`
- `POST /api/v1/users/login`

### Requieren autenticación

- `GET /api/v1/users/me`
- `POST /api/v1/reservations`
- `GET /api/v1/reservations/mine`
- `GET /api/v1/reservations/{id}`
- `POST /api/v1/payments`

### Solo Admin

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

## Resumen funcional del backend

### Usuarios

- `POST /api/v1/users`
- `POST /api/v1/users/login`
- `GET /api/v1/users/me`
- `GET /api/v1/users`
- `GET /api/v1/users/{id}`

Notas:
- las contraseñas se almacenan hasheadas con ASP.NET Core Identity
- el login devuelve `id`, `name`, `email`, `role` y `token`

### Eventos

- `GET /api/v1/events`
- `GET /api/v1/events/{id}`
- `GET /api/v1/events?name=rock`
- `GET /api/v1/events?eventDate=2026-07-15`
- `GET /api/v1/events?name=rock&eventDate=2026-07-15`
- `POST /api/v1/events` `Admin`
- `DELETE /api/v1/events/{id}` `Admin`

Notas:
- el evento soporta `ImageUrl`
- el evento soporta `Description`
- el estado `SoldOut` se calcula en lectura según disponibilidad real de butacas

### Sectores

- `GET /api/v1/events/{eventId}/sectors`
- `GET /api/v1/events/{eventId}/sectors/{sectorId}`
- `POST /api/v1/events/{eventId}/sectors` `Admin`
- `DELETE /api/v1/events/{eventId}/sectors/{sectorId}` `Admin`

### Butacas

- `GET /api/v1/events/{eventId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats`
- `GET /api/v1/sectors/{sectorId}/seats/{seatId}`
- `POST /api/v1/sectors/{sectorId}/seats` `Admin`
- `POST /api/v1/sectors/{sectorId}/seats/bulk` `Admin`
- `DELETE /api/v1/sectors/{sectorId}/seats/{seatId}` `Admin`

Notas:
- el endpoint de butacas por sector devuelve también:
  - `reservedByCurrentUser`
  - `activeReservationId`
- eso permite retomar pago sobre reservas propias pendientes

### Reservas y pagos

- `POST /api/v1/reservations`
- `GET /api/v1/reservations/mine`
- `GET /api/v1/reservations/{id}`
- `POST /api/v1/payments`

Notas:
- al reservar, la butaca pasa a `Reserved` y la reserva a `Pending`
- al confirmar el pago, la butaca pasa a `Sold` y la reserva a `Paid`
- `POST /api/v1/payments` recibe `reservationId`
- hoy el pago es simulado interno
- si un evento, sector o butaca tiene reservas asociadas, no se permite su eliminación

### Auditoría

- `GET /api/v1/audit-logs` `Admin`

Ejemplos:
- `GET /api/v1/audit-logs?userId=3`
- `GET /api/v1/audit-logs?date=2026-05-24`
- `GET /api/v1/audit-logs?dateFrom=2026-04-01&dateTo=2026-04-23`

## Reglas de catálogo admin

- un evento no puede tener más sectores que `TicketingRules.MaxSectorsPerEvent`
- `Capacity` del sector debe ser mayor a `0` y menor o igual a `TicketingRules.MaxSectorCapacity`
- no se pueden crear más asientos que la `Capacity` del sector
- en bulk, la cantidad de filas no puede superar `TicketingRules.MaxRowsPerBulkCreate`
- en bulk, la cantidad de asientos por fila debe estar entre `1` y `TicketingRules.MaxSeatsPerRow`
- las filas válidas salen de `TicketingRules.RowLabels`
- no se permiten butacas duplicadas dentro del sector

## Estado actual del frontend

Implementado:
- home rediseñado
- listado de eventos
- filtro por nombre
- filtro por fecha con datepicker custom
- login y register
- cambio de idioma
- cambio de tema
- detalle de evento
- sectores visibles en detalle
- flujo de compra:
  - elegir sector
  - elegir butaca
  - reservar al continuar
  - checkout
  - pago simulado
- Mis entradas
- títulos dinámicos por página
- fallback visual para eventos sin imagen

Notas:
- el frontend consume eventos reales del backend
- si no hay `ImageUrl`, usa placeholders visuales

## Pendiente real

- vista admin en frontend

## Pendiente de segunda entrega

- liberación automática de reservas vencidas
- expiración efectiva de reservas a los 5 minutos
- mejoras más fuertes de concurrencia
