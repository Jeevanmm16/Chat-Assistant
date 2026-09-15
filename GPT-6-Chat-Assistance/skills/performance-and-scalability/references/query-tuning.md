# Query Tuning

*(Note: Ensure you are following the `efcore-database-conventions` skill as well)*

## 1. AsNoTracking
When retrieving data for a GET request (where the entity will not be updated), always use `.AsNoTracking()`. This disables EF Core's Change Tracker, saving significant memory and CPU.

```csharp
// ✅ Good
var users = await _context.Users.AsNoTracking().ToListAsync();
```

## 2. Pagination
Never allow a query to return an unbounded result set.
```csharp
// ❌ Bad
var results = await _context.Logs.ToListAsync(); 

// ✅ Good
var results = await _context.Logs
    .OrderByDescending(x => x.CreatedAt)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

## 3. Projection
Select only the columns you need instead of fetching the entire entity.
```csharp
// ✅ Good
var userNames = await _context.Users
    .Where(u => u.IsActive)
    .Select(u => new UserSummaryDto { Id = u.Id, Name = u.Name })
    .ToListAsync();
```
