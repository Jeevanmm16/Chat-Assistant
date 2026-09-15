# Feature Implementation Plan

**Feature Name:** [Insert Feature Name]
**Description:** [Brief summary of the requirement]

## 1. Impacted Architectural Layers
- [ ] API (Controllers, DTOs, Routing)
- [ ] Service (Business Logic, Interfaces)
- [ ] Repository (EF Core, Data Access)
- [ ] Database (Migrations, Schema)
- [ ] Infrastructure (External APIs, Background Jobs)

## 2. Required Skills Loaded
Based on the impacted layers, the following governance skills have been loaded into context:
- `project-foundation`
- `architecture-governance`
- [List others based on matrix...]

## 3. Execution Steps

### Phase 1: Contracts & Scaffolding
- [ ] Define Request/Response DTOs.
- [ ] Create Interface in Service layer.
- [ ] Create empty Controller endpoint.

### Phase 2: Core Logic & Data
- [ ] Update EF Core Entity (if needed).
- [ ] Generate Database Migration (if needed).
- [ ] Implement Repository method.
- [ ] Implement Service logic (Map Entity to DTO).

### Phase 3: Integration & Security
- [ ] Apply `[Authorize]` and validate Claims.
- [ ] Inject `ILogger` and implement structured logging.

### Phase 4: Validation & Quality
- [ ] Write Unit Tests for the Service layer.
- [ ] Run required validation scripts (`verify-api-standards.ps1`, etc.).
