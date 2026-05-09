# overtime-api-dotnet

Proyecto liviano de practica empresarial con .NET 10, ASP.NET Core Web API, EF Core, PostgreSQL, Swagger y un frontend simple con Razor Pages + Bootstrap.

El sistema simula gestion de empleados, solicitudes de horas extra, aprobaciones, rechazos, reportes y carga ETL desde CSV.

## Estructura

```text
overtime-api-dotnet/
  Controllers/     Endpoints REST
  Services/        Reglas de negocio simples
  Repositories/    Acceso a datos con EF Core
  Models/          Entidades de base de datos
  DTOs/            Objetos usados por la API
  Data/            DbContext de Entity Framework
  Pages/           Frontend Razor Page con fetch API
  data/            CSV de ejemplo
```

## Requisitos

- .NET SDK 10
- Docker Desktop, opcional para levantar PostgreSQL local
- Herramienta EF Core CLI

Instalar EF Core CLI si no la tienes:

```bash
dotnet tool install --global dotnet-ef
```

## PostgreSQL local

Con Docker Compose:

```bash
docker compose up -d
```

Base configurada:

- Base: `overtime_dotnet`
- Usuario: `postgres`
- Password: `postgres`
- Puerto: `5432`

La cadena de conexion esta en `appsettings.json`.

## Migraciones EF Core

Crear migracion inicial:

```bash
dotnet ef migrations add InitialCreate
```

Aplicar migracion en PostgreSQL:

```bash
dotnet ef database update
```

## Ejecutar API y frontend

```bash
dotnet run
```

Abrir:

- Frontend: `https://localhost:xxxx/` o `http://localhost:xxxx/`
- Swagger: `https://localhost:xxxx/swagger`

El puerto exacto aparece en la consola al ejecutar `dotnet run`.

## Endpoints principales

- `GET /api/empleados`
- `GET /api/empleados/{id}`
- `POST /api/empleados`
- `PUT /api/empleados/{id}`
- `DELETE /api/empleados/{id}`
- `GET /api/horas-extra`
- `POST /api/horas-extra`
- `PUT /api/horas-extra/{id}/aprobar`
- `PUT /api/horas-extra/{id}/rechazar`
- `GET /api/horas-extra/pendientes`
- `GET /api/reportes/horas-aprobadas`
- `POST /api/etl/cargar-empleados`

## Ejemplos JSON

Crear empleado:

```json
{
  "nombre": "Juan Perez",
  "correo": "juan@empresa.cl",
  "cargo": "Analista",
  "area": "TI",
  "activo": true
}
```

Crear solicitud de horas extra:

```json
{
  "empleadoId": 1,
  "fecha": "2026-05-09",
  "horas": 2.5,
  "motivo": "Cierre mensual"
}
```

Cargar empleados desde CSV:

```json
{
  "rutaArchivo": "data/empleados.csv"
}
```

## Frontend

El frontend esta en `Pages/Index.cshtml`.

Usa Bootstrap para la interfaz y `fetch` para llamar a la API:

```javascript
fetch('/api/empleados')
```

Flujo completo:

1. El usuario completa un formulario en el navegador.
2. JavaScript envia JSON con `fetch` a un endpoint `/api/...`.
3. El Controller recibe el request.
4. El Service aplica reglas simples.
5. El Repository usa EF Core.
6. `AppDbContext` guarda o consulta PostgreSQL.
7. La API responde JSON y el frontend actualiza las tablas.

## Conceptos del backend

Controller: recibe HTTP, valida lo basico y devuelve respuestas como `Ok`, `Created` o `NotFound`.

Service: contiene reglas de negocio simples, por ejemplo aprobar o rechazar solicitudes.

Repository: centraliza consultas y escritura en base de datos usando EF Core.

DbContext: representa la conexion con PostgreSQL y mapea entidades como `Empleado` y `SolicitudHoraExtra` a tablas.

## Consultas SQL para practicar

```sql
select * from "Empleados";

select * from "SolicitudesHorasExtra"
where "Estado" = 'PENDIENTE';

select e."Nombre", sum(s."Horas") as total_horas
from "SolicitudesHorasExtra" s
join "Empleados" e on e."Id" = s."EmpleadoId"
where s."Estado" = 'APROBADA'
group by e."Nombre";
```

## Git workflow sugerido

```bash
git init
git add .
git commit -m "crear proyecto overtime api dotnet"
git checkout -b feature/aprobaciones
git add .
git commit -m "agregar flujo de aprobaciones"
git checkout main
git merge feature/aprobaciones
git remote add origin https://github.com/tu-usuario/overtime-api-dotnet.git
git push -u origin main
```

## Ejercicios sugeridos

- Agregar validaciones para que `Horas` no sea mayor a 12.
- Agregar filtro por area en empleados.
- Agregar comentario obligatorio al rechazar una solicitud.
- Crear endpoint `GET /api/horas-extra/empleado/{empleadoId}`.
- Agregar paginacion simple al listado de solicitudes.
- Crear una migracion nueva agregando campo `Rut` a `Empleado`.
- Leer el codigo desde Controller hasta Repository y dibujar el flujo.

## Nota de alcance

Este proyecto evita autenticacion avanzada, microservicios, colas, Kubernetes y arquitectura compleja. La idea es practicar un flujo parecido a empresa, pero facil de ejecutar y leer localmente.
