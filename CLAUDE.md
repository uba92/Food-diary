# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

FoodDiary is a full-stack food & symptom diary. Its purpose is to let a single user record, day by day, both the foods eaten and the symptoms those foods may have triggered, so the collected data can later be extracted as a **PDF report to share with a nutritionist**. Meal planning (weekly plans, reusable food alternatives) is the foundation on top of which daily symptom tracking and the report/PDF export are built.

Backend is an ASP.NET Core Web API (.NET 10) backed by PostgreSQL via EF Core; frontend is a React 19 + Vite SPA.

> Status: implemented end-to-end — meal planning (FoodAlternative + FoodCategory, WeeklyPlan, PlannedMeal with an `Eaten` flag, per-day view, usage stats), daily **symptom logging** (`SymptomEntry`, per-day in the plan detail), an aggregation **report** (`ReportService`: daily timeline, symptom frequency, food↔symptom co-occurrence correlations) and its **PDF export** via QuestPDF (`GET /api/report/pdf`). The food↔symptom link uses only meals flagged as eaten.

## Commands

### Backend (`backend/FoodDiary.Api/FoodDiary.Api`)

- Run the API: `dotnet run` — serves on `http://localhost:5240` and `https://localhost:7288`, Swagger UI at `/swagger` (Development only)
- Build: `dotnet build`
- EF Core migrations: `dotnet ef migrations add <Name>` / `dotnet ef database update` (requires the `dotnet-ef` tool and a reachable Postgres instance per the connection string)
- The DB password is **not** in `appsettings.json` (left as empty `Password=`). It's stored via .NET user secrets (`UserSecretsId` set in the `.csproj`). Set it locally with:
  `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=food_diary_db;Username=postgres;Password=<your-password>"`
- No test project exists yet.

### Frontend (`frontend/`)

- `npm run dev` — Vite dev server on `http://localhost:5173`
- `npm run build` — production build
- `npm run lint` — ESLint
- `npm run preview` — preview production build

## Architecture

### Backend layering

Each domain entity (`FoodAlternative`, `WeeklyPlan`, `PlannedMeal`, `SymptomEntry`) follows the same vertical slice:

`Controllers/` → `Interfaces/I*Service` → `Services/` → `Interfaces/I*Repository` → `Repositories/` → `Data/FoodDiaryDbContext`

- Controllers are thin: bind request DTOs, call the service, translate results to HTTP status codes.
- Services hold validation (manual `ArgumentException` checks — no DataAnnotations) and orchestration; repositories only do EF Core CRUD + `Include`s.
- `Helpers/*Mapper` classes do all entity ↔ DTO conversion (`ToEntity`, `ToResponseDto`, `ToByDayResponseDto`). There is no AutoMapper — mappers are plain classes with static methods.
- `Middlewares/GlobalExceptionHandler` only catches `ArgumentException` → 400 `{ error: message }`. Other exceptions are unhandled.
- `ReportService` is read-only and has **no repository of its own**: it reuses `IWeeklyPlanRepository` + `ISymptomEntryRepository` to aggregate. `Helpers/SymptomReportDocument` (QuestPDF `IDocument`) renders the report to PDF.
- Convention reminders when adding an entity: register both `I*Repository`/`I*Service` in `Program.cs`; PUT endpoints return the updated DTO (or `null` → 404), not a `bool`; invalid related ids throw `ArgumentException` (→ 400).

### Domain model

- `FoodAlternative` is a reusable meal option (Name, MealType, Quantity, WeeklyFrequency, Notes, **FoodCategory** — nullable string e.g. Proteine/Cereali/Verdura).
- `WeeklyPlan` (StartDate/EndDate) has many `PlannedMeal`.
- `PlannedMeal` belongs to a `WeeklyPlan` and a `FoodAlternative`, has a `DayOfWeekType` enum (Monday..Sunday) and an **`Eaten`** bool (was the planned meal actually eaten).
- `SymptomEntry` is a logged symptom: `OccurredAt` (date+time, UTC), `Type` (string), `Severity` (1–5), optional `MealType`, `Notes`, and optional `FoodAlternativeId` (suspected food).
- `WeeklyPlanService.GetFoodAlternativeUsageStatsAsync` computes per-plan usage counts vs. each `FoodAlternative.WeeklyFrequency` to flag over-limit items (`IsOverLimited`).
- `ReportService.GetReportAsync(from, to)` aggregates a `SymptomReportDto`: daily timeline (eaten foods + symptoms), symptom frequency, and food↔symptom **co-occurrence** correlations. A meal's date = `plan.StartDate.Date + DayOfWeek`; **only meals with `Eaten == true` count** toward correlations. Correlation is co-occurrence, NOT causation (label it as such; the PDF carries a medical disclaimer).

### Important conventions

- **UTC dates** — `WeeklyPlan.StartDate`/`EndDate` and `SymptomEntry.OccurredAt` map to Postgres `timestamp with time zone`. Any `DateTime` written to these columns must go through `DateTime.SpecifyKind(value, DateTimeKind.Utc)` (see `WeeklyPlanMapper.ToEntity`, `SymptomEntryMapper.ToEntity`, the `Update*` services and `ReportService`) — otherwise Npgsql throws, since dates deserialized from JSON have `DateTimeKind.Unspecified`.
- **Enums are serialized as integers** — there is no `JsonStringEnumConverter`. `DayOfWeekType` travels over JSON as a number (Monday=0 … Sunday=6); the frontend sends/maps integers. The `by-day` endpoint groups meals server-side into `monday`..`sunday` (camelCase) so the client doesn't need the enum there.
- **PDF** — QuestPDF (Community license set in `Program.cs`). `GET /api/report/pdf?from=&to=` returns `application/pdf`.

### Frontend structure

- `src/api/axiosClient.js` — axios instance pointed at `https://localhost:7288/api`; must match the backend's HTTPS launch profile and the `AllowFrontend` CORS policy in `Program.cs` (currently hardcoded to `http://localhost:5173`).
- `src/services/*Service.js` — one file per backend resource, thin wrappers around `axiosClient` (`weeklyPlanService`, `foodAlternativeService`, `plannedMealService`, `symptomService`, `reportService`).
- Routing via `react-router-dom` (`BrowserRouter` in `main.jsx`). Routes in `App.jsx`: `/weekly-plans`, `/weekly-plans/:id` (detail = per-day meals + symptoms), `/food-alternatives`, `/report`. `components/Navbar.jsx` is the persistent nav.
- `src/pages/*Page.jsx` — page-level components that call services directly; **no global state store** (each page manages its own state/`useEffect` fetch).
- `src/components/` — shared UI: `ConfirmDialog` (styled confirm modal, used for all deletes), `SymptomFormModal` (mounted only when open, with a `key` so it resets — no sync `useEffect`).
- Styling is a single hand-written `src/index.css` (CSS variables, wellness/green theme, responsive at 768/600/380px). No CSS framework.
- ESLint (`npm run lint`) enforces `react-hooks/set-state-in-effect`: do **not** call setState synchronously in an effect body — fetch inside an inline `async` function and set state only after `await` (see existing pages).

## Notes

- No authentication/authorization implemented (single-user app).
- Request DTOs have no validation attributes; all validation happens manually inside services.
- **Known issue**: the `SymptomEntry → FoodAlternative` FK is `ON DELETE NO ACTION`. Deleting a `FoodAlternative` referenced by a symptom fails at the DB level, and that exception is **not** handled by `GlobalExceptionHandler` (→ 500). Fix later via `ON DELETE SET NULL` (new migration) or by catching it.
- No automated tests yet; `ReportService` correlation logic is the first sensible candidate.
- A detailed write-up of the 2026-06-17 work session lives in `docs/sessione-2026-06-17.md`.

## Rules

1. Non modificare mai le migration esistenti in `Migrations/`: per qualsiasi cambio allo schema crea sempre una nuova migration (`dotnet ef migrations add <Name>`).
2. Non inserire mai credenziali (password, connection string complete, token, ecc.) in `appsettings.json` o in qualsiasi altro file tracciato da git. Usa i .NET user secrets o variabili d'ambiente.
3. Rispondi sempre in italiano.
4. Preferisci task piccoli e atomici; chiedi conferma prima di eseguire refactoring estesi o che toccano molti file.
