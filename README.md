# AI Job Hunter Agent

A local-first application concept for discovering job opportunities, analyzing how they align with a verified professional profile, ranking them transparently, and supporting informed application decisions.

## Problem Statement

Job searching across multiple sources is repetitive, and job descriptions make it difficult to compare requirements, location eligibility, and fit consistently. AI Job Hunter Agent is intended to organize that workflow without inventing candidate qualifications, bypassing job-source restrictions, or submitting applications without human approval.

## Main Capabilities

Planned capabilities include:

- Discover and import jobs through permitted provider integrations or manual URL and description import.
- Preserve one original resume unchanged and extract career information for user verification.
- Analyze job requirements with structured AI outputs and compare them against verified candidate profile facts. AI must not invent qualifications or override deterministic eligibility rules.
- Evaluate eligibility and match scoring separately, and produce explainable assessments.
- Track saved, interested, rejected, and application-status information with notes.
- Keep the user in control of all application decisions and actions.

## Technology Stack

- Backend: C#, .NET 10, ASP.NET Core Web API
- Frontend: Angular and TypeScript, maintained as an independent application
- Persistence: PostgreSQL and Entity Framework Core
- Local infrastructure: Docker Compose for PostgreSQL
- AI: OpenAI API with Structured Outputs, combined with deterministic business rules
- Testing: xUnit and integration tests

Redis, RabbitMQ, and a separate background worker are not part of the MVP.

## High-Level Architecture

The system is planned as a modular monolith with multiple independent .NET projects and Clean Architecture dependency principles. The API coordinates application use cases; domain rules remain independent of persistence, web frameworks, and external providers. Provider integrations are isolated behind boundaries, while Angular communicates with the API. PostgreSQL is the MVP persistence store.

See [System Architecture](docs/architecture/system-architecture.md) and [Domain Model](docs/architecture/domain-model.md).

## MVP Scope

The MVP is a single-user local application with on-demand job discovery and analysis. It will support the approved job-source strategy, including manual import when automated access is unavailable or not authorized. The candidate profile is generic and configurable, with verified professional experience, technical skills, work authorization, and location preferences.

Eligibility evaluation determines `Eligible`, `Ineligible`, or `NeedsReview` based on mandatory constraints and available evidence. Match scoring separately evaluates job relevance and suitability; a high match score never overrides an `Ineligible` result. Unknown or insufficiently supported information is not automatically treated as eligible or ineligible and should be reviewed when appropriate.

Location preferences are configurable, with this priority:

1. Opportunities in the candidate's configured primary location or region.
2. Relocation opportunities within the candidate's configured relocation area.
3. Remote opportunities available to candidates at their configured residence.

Remote eligibility must be supported by the job's stated work-location requirements. Target job types are full-time and contract.

The original resume must remain unchanged. Resume customization, generated DOCX/PDF documents, automatic application submission, scheduled processing, and unapproved source access are out of scope.

## Security Principles

- Keep secrets, including the OpenAI API key, on the backend and out of source control and browser code.
- Treat resumes, profile information, and job descriptions as sensitive data; minimize exposure and avoid logging their contents.
- Treat imported text as untrusted input and validate structured AI output.
- Use deterministic rules for hard eligibility constraints; uncertain cases require review.
- Respect official provider permissions and terms. Do not bypass access controls or use prohibited scraping or automation.
- Require human approval for application actions.

## Local PostgreSQL and EF Core Setup

From the repository root, copy `.env.example` to `.env` and replace `REPLACE_WITH_A_LOCAL_SECRET` with a local PostgreSQL password. The root `.env` file is ignored by Git and is used only by Docker Compose:

```powershell
Copy-Item .env.example .env
docker compose up -d
docker compose ps
```

Wait for the `postgres` service to report healthy. PostgreSQL listens only on `127.0.0.1:5432` and stores data in the `postgres_data` named volume. To stop the service without deleting its data, run `docker compose down`. Do not use `docker compose down -v` unless you intend to delete the database volume.

The API does not read Docker Compose's `.env` file. For local Visual Studio development, set the API connection string in .NET User Secrets, using the same database, user, and password values as `.env`:

```powershell
dotnet user-secrets set "ConnectionStrings:JobHunter" "Host=localhost;Port=5432;Database=jobhunter;Username=jobhunter_dev;Password=YOUR_LOCAL_PASSWORD" --project src/JobHunter.Api/JobHunter.Api.csproj
```

Replace `YOUR_LOCAL_PASSWORD` with the local password from `.env`. The API launch profiles use the `Development` environment, where ASP.NET Core loads User Secrets. Configure the connection string through the deployment environment's configuration provider outside local development; do not commit credentials.

EF tooling is pinned in the repository-local tool manifest. Restore it from the repository root with:

```powershell
dotnet tool restore
```

There is intentionally no initial migration yet because the project has no persisted domain model. After persistence entities and mappings exist, create and apply migrations explicitly from the repository root:

```powershell
dotnet ef migrations add InitialCreate --project src/JobHunter.Infrastructure/JobHunter.Infrastructure.csproj --startup-project src/JobHunter.Api/JobHunter.Api.csproj --output-dir Persistence/Migrations -- --environment Development
dotnet ef database update --project src/JobHunter.Infrastructure/JobHunter.Infrastructure.csproj --startup-project src/JobHunter.Api/JobHunter.Api.csproj -- --environment Development
```

Migrations are stored in `JobHunter.Infrastructure/Persistence/Migrations` and are not applied automatically when the API starts.

## Development Roadmap

1. Establish architecture, domain, and integration decisions in documentation and ADRs.
2. Build the local development foundation and persistence setup.
3. Implement profile and immutable resume handling with user verification.
4. Implement manual job import, normalization, and tracking.
5. Add structured OpenAI analysis and deterministic, explainable matching.
6. Add only job-source integrations whose intended use and access have been verified.
7. Validate security, privacy, backups, and operational workflows.

## Current Project Status

Architecture and documentation baseline established. Application code, integrations, AI analysis, and job-search capabilities are not yet implemented. Source access, including LinkedIn and Indeed automated access, must not be assumed; the current plan supports manual URL and description import as a fallback.

## Architecture Decision Records

- [ADR-001: Modular Monolith](docs/adr/ADR-001-modular-monolith.md)
- [ADR-002: Job Source Integration](docs/adr/ADR-002-job-source-integration.md)
- [ADR-003: AI Integration](docs/adr/ADR-003-ai-integration.md)
- [ADR-004: Location Ranking](docs/adr/ADR-004-location-ranking.md)
