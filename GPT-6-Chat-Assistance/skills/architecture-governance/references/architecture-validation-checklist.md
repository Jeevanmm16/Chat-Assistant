# Architecture Validation Checklist

Before marking a feature as complete, review this checklist to ensure architectural compliance.

## 1. Dependency Boundaries
- [ ] Is `DbContext` hidden from the Service layer?
- [ ] Does the Controller only inject Services (and not Repositories)?
- [ ] Is the Entity project free of NuGet packages like `EntityFrameworkCore`?

## 2. DTO and Mapping
- [ ] Are Entities mapped to DTOs before being returned from the API?
- [ ] Are mapping profiles (e.g. AutoMapper) located in the Service layer?
- [ ] Are incoming API payloads mapped into DTOs before interacting with the Service layer?

## 3. Data Access & Performance
- [ ] Are all read-only database queries using `.AsNoTracking()`?
- [ ] Has the N+1 problem been avoided (using `.Include()` or projections)?
- [ ] Are database results materialized (e.g., `.ToListAsync()`) before leaving the Repository?
- [ ] Is filtering and pagination happening in the database, not in memory?

## 4. Business Logic
- [ ] Is all business validation handled by the Service layer?
- [ ] Is the Controller free of `if/else` business rules?
