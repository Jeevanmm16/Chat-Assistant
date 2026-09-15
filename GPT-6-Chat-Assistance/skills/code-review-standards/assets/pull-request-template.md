## Description
[Describe the purpose of this Pull Request. What feature does it add or what bug does it fix?]

## Impacted Layers
- [ ] API
- [ ] Service
- [ ] Repository
- [ ] Database
- [ ] Infrastructure

## Code Review Gate Checklist

### Security
- [ ] No hardcoded secrets (`GetBytes("password")`).
- [ ] Endpoints are protected with `[Authorize]`.
- [ ] UserId is pulled from Claims, not the payload.

### Architecture & Performance
- [ ] Controllers only call Services.
- [ ] Services only call Repositories.
- [ ] DTOs mapped cleanly without leaking Entities.
- [ ] No synchronous blocking (`.Wait()`, `.Result`).
- [ ] `.AsNoTracking()` is used on read-only queries.

### Testing
- [ ] Unit tests follow the AAA pattern.
- [ ] Mocks do not interact with a real SQL Server.
- [ ] Naming convention follows `MethodName_Scenario_ExpectedResult`.

## Automated Verifications
- [ ] `run-all-verifications.ps1` executes successfully.
