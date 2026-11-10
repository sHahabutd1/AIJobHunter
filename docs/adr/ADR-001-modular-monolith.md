# ADR-001: Modular Monolith

- **Status:** Accepted
- **Date:** 2026-01-19

## Context

AI Job Hunter Agent begins as a single-user local application, but should have clear boundaries suitable for a professional portfolio and future extension. The approved stack includes ASP.NET Core Web API and .NET 10, with Clean Architecture principles. There is no MVP requirement for independent service deployment, distributed scaling, or a separate worker.

## Decision

Build the backend as a modular monolith using multiple independent .NET projects and Clean Architecture dependency principles. Keep the domain independent of frameworks and infrastructure. The application layer coordinates use cases and defines boundaries for persistence and external integrations. Infrastructure and provider adapters implement those boundaries. The API is the HTTP composition boundary. The Angular frontend remains an independent application.

Use PostgreSQL with EF Core. Do not introduce microservices, RabbitMQ, Redis, or a separate Worker project in the MVP. Add projects or finer module boundaries only where they clarify ownership and dependency direction; do not create a project for every class or feature by default.

## Consequences

### Positive

- Keeps deployment and local operation simpler than a distributed system.
- Establishes testable domain and application boundaries.
- Allows job-source and AI integrations to evolve independently behind application-owned interfaces.
- Supports future extraction of a component if a concrete deployment or scaling need appears.

### Trade-offs

- Multiple projects introduce dependency wiring and build configuration overhead compared with a single-project application.
- A monolith requires discipline to prevent module boundaries from eroding over time.
- Future service extraction may require explicit contracts and data ownership work.

### Operational constraints

- The MVP runs on demand without a separate worker or message broker.
- PostgreSQL is the initial persistence system and runs in Docker Compose for development.

## Alternatives Considered

- **Single .NET project:** Simpler initially, but offers weaker compile-time separation for the explicitly approved independent backend concerns.
- **Microservices:** Rejected for the MVP because independent deployment and distributed operation are not required and would add operational complexity.
- **Modular monolith with a separate worker:** Deferred because scheduled background processing is not part of the initial execution model.
