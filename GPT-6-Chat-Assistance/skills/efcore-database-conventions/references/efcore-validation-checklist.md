# EF Core Performance Checklist

Before returning repository code, verify the following:

- [ ] Query filters applied **before** execution (`.Where` comes before `.ToListAsync`).
- [ ] `.AsNoTracking()` used for read-only queries.
- [ ] No full table scans (unfiltered `ToListAsync()`).
- [ ] Pagination applied where required for large tables.
- [ ] N+1 issue avoided (queries do not run inside `foreach` loops).
- [ ] Only required columns selected (using Projection/`.Select`).
- [ ] `IQueryable` maintained until execution.
- [ ] No unnecessary `.Include()` chains that load unneeded data.
- [ ] Repository is correctly registered in the DI container.
