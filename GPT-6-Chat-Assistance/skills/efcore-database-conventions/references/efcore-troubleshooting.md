# EF Core Troubleshooting Guide

Review this guide if you experience slow API responses, high memory usage, database connection timeouts, or incorrect data access behavior.

## 1. N+1 Query Problem
**Symptoms**: Slow API response, hundreds of SQL queries generated in the console logs.
**Cause**: Executing queries or accessing navigation properties inside a loop.

**❌ Bad:**
```csharp
var users = await _context.Users.ToListAsync();
foreach(var user in users)
{
    var roles = user.Roles; // Triggers a new DB query for EVERY user!
}
```

**✅ Solution:** Use `.Include()` to fetch related data in a single query.
```csharp
var users = await _context.Users
    .Include(x => x.Roles)
    .ToListAsync();
```

## 2. Missing `AsNoTracking()`
**Symptoms**: High memory usage, slow read-only operations.
**Cause**: EF Core is tracking changes for entities that are only being read and never updated.

**✅ Solution:**
```csharp
var users = await _context.Users
    .AsNoTracking()
    .ToListAsync();
```

## 3. Premature `ToList()`
**Symptoms**: Application crash, Out of Memory Exception, incredibly slow queries.
**Cause**: Loading the entire table into server memory before filtering.

**❌ Bad:**
```csharp
var users = _context.Users.ToList()
    .Where(x => x.IsActive);
```

**✅ Good:**
```csharp
var users = await _context.Users
    .Where(x => x.IsActive)
    .ToListAsync();
```

## 4. Loading Entire Tables
**Symptoms**: Slow API response, high Database CPU usage.
**Cause**: Failing to cap the result set.

**❌ Bad:**
```csharp
var users = await _context.Users.ToListAsync();
```

**✅ Good:**
```csharp
var users = await _context.Users
    .Where(x => x.IsActive)
    .Take(100)
    .ToListAsync();
```

## 5. Missing Pagination
**Symptoms**: Queries get slower over time as the database grows.
Always paginate large datasets using `.Skip()` and `.Take()`.

```csharp
var users = await _context.Users
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

## 6. Soft Delete Issues
**Symptoms**: Deleted items are showing up in API responses.
**Cause**: Forgetting to filter out soft-deleted records.

**✅ Solution:** Verify `.Where(x => !x.IsDeleted)` is applied consistently across all relevant queries.

## 7. Missing Migrations
**Symptoms**: Database schema mismatch errors.
**Cause**: Changing an Entity class but forgetting to generate the migration.

**✅ Solution:** After modifying an Entity class, immediately run:
```powershell
Add-Migration MigrationName
Update-Database
```
