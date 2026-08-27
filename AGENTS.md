# AGENTS.md — Clínica San Salud

Contexto para agentes de código (opencode, Claude Code, etc.) que trabajen en este repo.

## Qué es

API RESTful educativa (.NET 10, C#) para gestión de turnos médicos de una clínica:
**Medicos**, **Pacientes** y **Turnos**. Proyecto de referencia para estudiantes
(Prácticas Profesionalizantes I). Documentación completa en `README.md`.

## Comandos

```bash
dotnet build Clinica-San-Salud.slnx        # compilar
dotnet test Clinica-San-Salud.slnx         # correr 16 tests (xUnit + Moq + integración)
dotnet run --project SanSaludAPI           # levantar API (Swagger/Scalar en /scalar/v1 en Development)
dotnet ef migrations add <Nombre> --project SanSaludAPI   # nueva migración (SQLite local)
```

## Arquitectura (N-Tier, estricta separación)

```
SanSaludAPI/
├── API/          # Controladores MVC (MedicosController, PacientesController, TurnosController)
├── BusinessLogic/# Servicios con reglas de negocio (interfaces + implementación por entidad)
├── DataAccess/   # EF Core: DbContext, entidades (Medico, Paciente, Turno), repositorios
├── Shared/       # DTOs (Create/Response/Update) y excepciones de negocio
└── Migrations/   # Migraciones EF Core
SanSaludAPI.Tests/
├── MedicoServiceTests.cs, TurnoServiceTests.cs      # unitarios (Moq sobre repositorios)
└── IntegrationTests/                                # WebApplicationFactory + SQLite en memoria
```

Flujo obligatorio: **Controller → Service → Repository → DbContext**.
Los controllers no acceden a datos directamente; los services no usan LINQ-to-entities.

## Convenciones del proyecto

- IDs tipo `Guid` generados en los repositorios (`CreateAsync` asigna `Guid.NewGuid()`).
- DTOs manuales (sin AutoMapper): métodos privados `MapToResponseDTO` en cada service.
- Errores de negocio → excepciones tipadas en `Shared/BusinessExceptions.cs`
  (`*NotFoundException`, `ValidationException`, `OverlappingScheduleException`);
  cada controller las mapea a códigos HTTP (404/400/409) con try/catch explícito.
- Repositorios usan `AsNoTracking()` en lecturas; `DeleteBehavior.Restrict` en FKs de Turno.
- Comentarios y mensajes de error en español.
- Estilo: clases con llaves en bloque `{ }` explícito, inyección por constructor.

## Reglas de negocio clave (Turnos)

- La fecha/hora del turno debe ser futura (`InvalidTurnoDateException`).
- Duración fija de **2 horas** por turno (`DuracionHoras = 2`).
- No se permite solapamiento de turnos del mismo médico — validación en
  `TurnoService.ValidateNoOverlappingTurnos` (consulta rango ±4h, luego overlap en memoria).
- Al actualizar un turno se excluye su propio Id de la validación de solapamiento.

## Stack

- .NET 10 (`net10.0`), Nullable + ImplicitUsings habilitados
- EF Core 10 + SQLite (`SanSalud.db` en la raíz del proyecto API)
- Scalar.AspNetCore para UI de docs OpenAPI (solo Development)
- Tests: xUnit, Moq, WebApplicationFactory

## Notas / pendientes conocidos

- ⚠️ NU1903: dependencia transitiva `Microsoft.OpenApi` 2.0.0 con vulnerabilidad alta — evaluar bump.
- Sin autenticación/autorización real (`UseAuthorization` sin políticas definidas).
- `notebooklm/` dentro del repo es material auxiliar (skills de Claude), no parte del código C#.
