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
- vista de compra, `Mis entradas` y panel admin

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
    "Connection": "Server=AGUSTIN\\SQLEXPRESS;Database=TicketingDb_2026;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Si el servidor local es distinto, cambiar `Server=...` por la instancia correcta.

### JWT, roles y reglas

Configuración pública:

```json
{
  "Jwt": {
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

La clave JWT no se guarda en el repo.

Definirla con una de estas opciones:

```powershell
dotnet user-secrets --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj set "Jwt:Key" "TP-Proyecto-Software-Jwt-Key-Local-2026-Segura"
```

o variable de entorno:

```powershell
$env:Jwt__Key="TP-Proyecto-Software-Jwt-Key-Local-2026-Segura"
```

Nota:
- la `Jwt:Key` debe tener al menos `32` caracteres para que `HS256` funcione correctamente

Para verificar que quedó cargada:

```powershell
dotnet user-secrets --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj list
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
- la migración inicial actual es `20260514213906_InitialCreate`
- el proyecto incluye seeds base
- este flujo está pensado para una base nueva o vacía
- al levantar la API también se ejecuta `Database.Migrate()` automáticamente
- al levantar la API se verifica además si falta el dataset base y, en ese caso, se inicializan las seeds
- no usar una base vieja armada con la historia anterior de migraciones, porque la nueva migración inicial intenta crear el esquema completo desde cero

### Prueba desde cero

Checklist previo:
- confirmar que la instancia de SQL Server del equipo exista y coincida con `Server=...`
- definir `Jwt:Key` antes de levantar la API
- elegir un nombre de base nuevo, por ejemplo `TicketingDb_Profesor`
- apuntar `ConnectionStrings:Connection` a esa base nueva
- compilar primero la solución para validar dependencias y configuración

1. definir `Jwt:Key` con User Secrets o variable de entorno
2. usar una base nueva o vacía en SQL Server
3. configurar la cadena de conexión en `backend/API/appsettings.json` o `appsettings.Development.json`
4. guardar el archivo si se cambió el nombre de la base
5. compilar:

```powershell
dotnet build TP-PROYECTO-SOFTWARE.sln
```

6. ejecutar migraciones:

```powershell
dotnet ef database update --project backend\Infraestructure\TP-PROYECTO-SOFTWARE.Infraestructure.csproj --startup-project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

7. levantar la API:

```powershell
dotnet run --project backend\API\TP-PROYECTO-SOFTWARE.API.csproj --launch-profile https
```

Si el navegador no se abre automáticamente, ingresar manualmente a:
- `https://localhost:7176/swagger`

8. verificar que la API quedó bien levantada:
- abrir `https://localhost:7176/swagger`
- probar `GET /api/v1/events`
- resultado esperado: responde `200 OK` y devuelve `1` evento semilla

9. levantar el frontend:

```powershell
cd frontend
npm install
npm run dev
```

10. si se quiere acceder al panel admin, registrar el usuario:
- `admintest@test.com`

Notas:
- si la base está vacía, la API carga automáticamente el dataset semilla base al iniciar
- las seeds base incluyen:
  - `1` evento
- `2` sectores
- `50` butacas por sector
- si aparece un error tipo `There is already an object named 'EVENT' in the database`, se está intentando aplicar la migración inicial sobre una base vieja; cambiar el nombre de la base o borrar esa base y recrearla
- si aparece un error indicando datos semilla incompletos, la base quedó en un estado parcial; usar una base nueva o vaciarla y volver a correr migraciones
- ese mail queda con rol `Admin` automáticamente por configuración

### Script idempotente

También se puede generar un script SQL idempotente de EF Core con:

```powershell
dotnet ef migrations script --idempotent --project backend\Infraestructure\TP-PROYECTO-SOFTWARE.Infraestructure.csproj --startup-project backend\API\TP-PROYECTO-SOFTWARE.API.csproj
```

Ese script:
- verifica la tabla `__EFMigrationsHistory`
- aplica sólo las migraciones que falten
- sirve para instalar o actualizar la base sin asumir un estado exacto

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
- `GET /api/v1/auditlogs`
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
- `GET /api/v1/users` soporta paginación y filtros:
  - `name`
  - `email`
  - `page`
  - `pageSize`

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
- al crear la reserva se guarda `ExpiresAt = UtcNow + 5 minutos`
- las reservas `Pending` vencidas se marcan como `Expired`
- cuando una reserva vence, la butaca vuelve a `Available`
- la expiración se aplica de forma lazy al listar butacas, reservar y pagar
- al confirmar el pago, la butaca pasa a `Sold` y la reserva a `Paid`
- no se puede pagar una reserva vencida
- `POST /api/v1/payments` recibe `reservationId`
- hoy el pago es simulado interno
- el checkout del frontend muestra countdown visible de la reserva
- si la reserva vence, el checkout bloquea el pago y fuerza a volver a elegir butaca
- las butacas usan `Version` como token real de concurrencia optimista
- si dos usuarios compiten por la misma butaca, uno gana y el otro recibe conflicto de disponibilidad
- si un evento, sector o butaca tiene reservas asociadas, no se permite su eliminación

### Auditoría

- `GET /api/v1/auditlogs` `Admin`

Ejemplos:
- `GET /api/v1/auditlogs?userId=3`
- `GET /api/v1/auditlogs?date=2026-05-24`
- `GET /api/v1/auditlogs?dateFrom=2026-04-01&dateTo=2026-04-23`
- `GET /api/v1/auditlogs?search=evento&page=1&pageSize=12`

Notas:
- `GET /api/v1/auditlogs` soporta:
  - `userId`
  - `search`
  - `date`
  - `dateFrom`
  - `dateTo`
  - `page`
  - `pageSize`
- `GET /api/v1/users` y `GET /api/v1/auditlogs` devuelven:
  - `items`
  - `totalCount`
  - `page`
  - `pageSize`
  - `totalPages`

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
- countdown visible de expiración en checkout
- Mis entradas
- panel admin:
  - crear evento completo con sectores y butacas en un solo flujo
  - ver usuarios con filtros por nombre y mail
  - ver auditoría con filtros por fecha y texto
  - paginación real para usuarios y auditoría
- títulos dinámicos por página
- fallback visual para eventos sin imagen
- lazy loading de páginas principales

Notas:
- el frontend consume eventos reales del backend
- si no hay `ImageUrl`, usa placeholders visuales
- si una reserva falla por vencimiento o disponibilidad, la vista de compra refresca el mapa de butacas
- si una reserva propia ya existía, el flujo puede retomar ese `reservationId`

## Segunda entrega implementada

- expiración real de reservas a los 5 minutos
- liberación lazy de butacas vencidas
- bloqueo de pago sobre reservas expiradas
- countdown visible de la reserva en checkout
- refresco del mapa de butacas ante conflictos o vencimientos
- concurrencia optimista real con `Seat.Version`
