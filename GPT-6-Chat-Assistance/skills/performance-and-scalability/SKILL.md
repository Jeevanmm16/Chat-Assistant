---
name: performance-and-scalability
description: Defines performance optimization techniques, caching strategies, query tuning, asynchronous processing, and scalability considerations. Use when building high-traffic endpoints or processing large datasets.
---

## Purpose
Ensure the application can scale efficiently by enforcing asynchronous processing, intelligent caching, and optimized database queries.

## 1. Asynchronous Processing
Always use `async`/`await` for I/O bound operations (Database calls, external API requests, file system access).

**❌ Forbidden (Thread Blocking):**
- `.Wait()`
- `.Result`
- `.GetAwaiter().GetResult()`

*(See `references/async-programming.md`)*

## 2. Caching Strategies
Implement caching for data that is frequently read but rarely modified.
- **In-Memory Cache**: Use for small, application-specific lookups.
- **Distributed Cache (Redis)**: Use for multi-instance deployments and shared session states.
- Always implement absolute or sliding expirations to prevent stale data.

*(See `references/caching-strategies.md`)*

## 3. Query Tuning & Data Access
- **`AsNoTracking`**: Use `.AsNoTracking()` on all Entity Framework queries where the data is read-only and won't be updated.
- **Pagination**: Never return full tables. Always implement `Skip` and `Take` for collections.
- **N+1 Prevention**: Explicitly `.Include()` related entities instead of lazy loading them in loops.

*(See `references/query-tuning.md`)*

## 4. Scalability Considerations
- Keep controllers lightweight. Offload heavy computations to background tasks or workers.
- Do not hold long-running locks in memory.
- Prefer `IAsyncEnumerable<T>` when streaming large datasets over the wire to prevent memory exhaustion.

---

## Validation Workflow

### Gate 1: Performance Audit
Run the automated performance linter to detect thread-blocking code:
```powershell
scripts/verify-performance.ps1
```

---

## References & Assets

- `references/async-programming.md` (Async best practices)
- `references/caching-strategies.md` (Cache expiration rules)
- `references/query-tuning.md` (EF Core performance)
- `assets/CacheServiceTemplate.cs` (Boilerplate caching implementation)
