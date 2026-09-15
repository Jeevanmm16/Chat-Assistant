# Pull Request Review Checklist

When reviewing code or opening a PR, ensure the following constraints are met:

## Security
- [ ] No hardcoded secrets (`GetBytes("password")`).
- [ ] Endpoints are protected with `[Authorize]` or explicitly `[AllowAnonymous]`.
- [ ] UserId is pulled from Claims, not the payload.

## Architecture
- [ ] Controllers only call Services.
- [ ] Services only call Repositories.
- [ ] DTOs are mapped cleanly without leaking Entities.

## Performance
- [ ] No synchronous blocking (`.Wait()`, `.Result`).
- [ ] Pagination is applied to unbounded queries.
- [ ] `.AsNoTracking()` is used on read-only queries.
