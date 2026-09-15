---
name: architecture-governance
description: Defines strict architectural rules, N-Tier layer responsibilities, dependency direction, and project-specific design principles. Use when writing any new classes, methods, or modifying project dependencies.
---

## Purpose
Ensure all business logic, data access, and API routing strictly adhere to the <ProjectName> N-Tier architectural boundaries.

---

## Architecture Flow

```text
      API
       ↓
    Service
       ↓
   Repository
       ↓
    Database

DTO ↔ Service ↔ API
Entities ↔ Repository
```

---

## Dependency Rule

Dependencies must flow inward only:

**✅ Allowed:**
- API → Service, DTO
- Service → Repository, DTO
- Repository → Entities

**⚠ Optional:**
- Service → Entities (only when domain entities are required)

**❌ Forbidden:**
- DTO → Entities
- Repository → Service
- Repository → API
- Entities → Repository
- Entities → Service
- Entities → API

---

## Layer Rules

### 1. Entities (`<ProjectName>Core.Entities`)
- Must have absolutely NO external dependencies (no Entity Framework, no API knowledge).
- Cannot reference any other layer.

### 2. DTO (`<ProjectName>Core.DTO`)
- Cannot reference `Service`, `Repository`, or `API`.

### 3. Repository (`<ProjectName>Core.Repository`)
- The **only** layer allowed to reference `Microsoft.EntityFrameworkCore.*` (except API for DI setup).
- Must NOT contain business rules (e.g., "if user is admin, do X").
- Controllers must never call repositories directly.

### 4. Service (`<ProjectName>Core.Service`)
- All business logic must be implemented here.
- Must NOT contain any HTTP context (`HttpContext`, `IActionResult`).
- Must not inject `DbContext` directly.
- Must communicate with persistence through Repository interfaces only.
- **Forbidden Packages**: `Microsoft.EntityFrameworkCore`, `Microsoft.AspNetCore.*`.

### 5. API (`<ProjectName>Core.API`)
- Exposes HTTP endpoints (Controllers).
- Controllers may only call Services.
- MUST NOT contain business logic or direct database queries.

---

## Design Principles

### Interface Ownership
- Repository interfaces belong to the `Service` layer.
- Repository implementations belong to the `Repository` layer.
- Service interfaces belong to the `Service` layer.
- Controllers must depend on Service interfaces only.

### Dependency Injection
- Never instantiate (`new`) services or repositories directly. Always inject interfaces.

### Dependency Registration
Every new Service and Repository must be registered in the API's DI container.
```csharp
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserRepository, UserRepository>();
```

### Single Responsibility Principle (SRP)
- A service class should handle one specific domain area.

### DTO Mapping
- For DTO mapping patterns read: `references/dto-mapping.md`.

---

## Implementation Workflow

When implementing a new feature, follow this exact sequence:

1. Create Entity changes.
2. Create DTOs.
3. Create Repository interface.
4. Create Repository implementation.
5. Create Service interface.
6. Create Service implementation.
7. Create Controller endpoint.
8. Register dependencies in API DI container.
9. Run Gate 1 and Gate 2. Then follow `references/architecture-validation-checklist.md`.

---

## Gates

### Gate 1: Dependency Check
Before committing structural changes, verify that the N-Tier flow is respected:
```powershell
scripts/verify-dependencies.ps1
```
- ✅ Pass → No forbidden project references found.
- ❌ Fail → Script explicitly states which reference is illegal. Remove it and re-architect.

### Gate 2: Build & Analyze
```powershell
dotnet build
```
- ✅ Pass → No warnings from analyzers or compilation errors.
- ❌ Fail → Fix compilation errors related to missing references or circular dependencies.

---

## Gotchas

### Architectural & Validation
- **Request validation belongs in the Service layer.**
- **Controllers should only validate HTTP-specific concerns** (route parameters, model binding).
- **Business validation must not be duplicated across layers.** Validation logic should have a single owner.
- **Controllers must never call repositories directly.**
- **Services must never inject DbContext directly.**
- **Entities must never be returned from API endpoints.** (Always map to DTOs)
- **Circular dependencies between projects are strictly forbidden.**
- **Missing Interfaces** — AI sometimes creates concrete classes without extracting an interface. Dependency Injection requires interfaces.



### Package Governance
- **Service layer must never reference `Microsoft.EntityFrameworkCore`**.
- **Service layer must never reference `Microsoft.AspNetCore.*`**.
- **Repository layer is the only layer allowed to execute EF Core queries.**

---

## References

- `references/dto-mapping.md`
  Read when creating or modifying DTO mappings.

- `references/architecture-validation-checklist.md`
  Run after implementing a feature.

- `references/dependency-examples.md`
  Examples of valid and invalid project references.
