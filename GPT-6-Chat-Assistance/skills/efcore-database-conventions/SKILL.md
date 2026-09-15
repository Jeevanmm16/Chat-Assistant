---
name: efcore-database-conventions
description: Create and maintain repositories following data access standards. Focuses on EF Core query performance, query optimization, pagination, tracking, and N+1 prevention. Use before writing any new repository methods, queries, or data access logic.
---

## Purpose
Create and maintain repositories following `<ProjectName>Core` data access standards. This skill focuses on EF Core query performance, repository implementation, query optimization, pagination, tracking behavior, and database access best practices.

---

## 1. Repository Workflow

1. **Create repository interface** in the Service layer.
2. **Create repository implementation** in the Repository layer, injecting `<ProjectName>CoreContext`.
3. **Build query using `IQueryable`**: Always start with `IQueryable<T> query = _context.Entities;` and apply filters before execution.
4. **Execute query at the end**: Use `await query.ToListAsync();` (Avoid premature execution).

---

## 2. Query Standards & Performance

### Read-Only Queries
Always use `.AsNoTracking()` for Search APIs, Dashboard APIs, Reporting APIs, and Lookup APIs.
```csharp
var users = await _context.Users.AsNoTracking().ToListAsync();
```

### Update Operations
Do **NOT** use `.AsNoTracking()` when entity updates are required.

### `IQueryable` vs `IEnumerable`
- **Preferred**: `IQueryable<User> query = _context.Users;` (Query executes in SQL Server).
- **Avoid**: `IEnumerable<User> users = _context.Users.ToList();` (Entire result set loads into memory).

---

## 3. Filtering & Pagination

- **Always filter in the database**: Never call `.ToList()` before a `.Where()`.
- **Large tables must use pagination**:
```csharp
var users = await _context.Users
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

---

## 4. N+1 Query Prevention & Includes

- **Prevent N+1**: Never run a query inside a `foreach` loop. Use `.Include()` to fetch related data in a single query.
- **Include Usage**: Only include required navigation properties. Avoid loading unnecessary relationships.
- **Projection Standards**: Avoid returning entire entities if only a few fields are needed. Use `.Select()` to project directly into a DTO.

---

## 5. Execution
- **Project Standard**: Use `.ToListAsync()`.
- Use `.ToArrayAsync()` only when an array output is explicitly required.

---

## Gates

### Gate 1: Performance Linter
Run the performance verification script before committing:
```powershell
scripts/verify-efcore-performance.ps1
```

---

## Gotchas

- **Premature ToList()**: Calling `.ToList()` before `.Where()` causes the entire table to load into memory.
- **Connection Timeout**: Usually caused by missing filters, missing indexes, full table scans, N+1 queries, or large Includes.
- **Returning Entire Entity Graph**: Avoid large `.Include()` chains unless absolutely required.

---

## References

- `references/efcore-troubleshooting.md`
  Read this guide if you experience slow queries, high memory usage, or N+1 issues.
- `references/efcore-validation-checklist.md`
  Run before finalizing repository code.
- `MIGRATION_SKILL.md`
  Load this file instead when creating or modifying Database Migrations!
