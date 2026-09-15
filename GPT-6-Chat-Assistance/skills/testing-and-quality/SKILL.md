---
name: testing-and-quality
description: Use this skill when generating, maintaining, or validating unit tests for ASP.NET Core applications following project standards. Focuses on NUnit, mocking, and business rule validation.
---

## Purpose
Generate and maintain unit tests for business logic and application behavior following project standards.

## Workflow
1. Identify component under test.
2. Identify dependencies.
3. Mock external dependencies using Moq (or approved mocking library).
4. Create test scenarios (Success, Validation Failures, Exception Scenarios).
5. Implement tests using the Arrange-Act-Assert pattern.
6. Verify assertions.
7. Execute tests.
8. Run Validation Checklist.

## Test Naming Convention
Use: `MethodName_Scenario_ExpectedResult`

**Examples:**
- `CreateUser_ValidRequest_ReturnsUser`
- `CreateUser_InvalidEmail_ThrowsValidationException`
- `DeleteUser_ValidRequest_SetsIsDeletedTrue`
- `GetUser_UserNotFound_ReturnsNull`

## Test Structure
Always separate the logical blocks of the test:
```csharp
// Arrange
...
// Act
...
// Assert
...
```

## What To Test

### Service Layer
- Business rules, validation logic, conditional flows, exception handling, and repository interactions.
- See: `references/service-testing.md`

### Controller Layer
- Status codes, service invocations, request validation, authentication/authorization behavior.
- See: `references/controller-testing.md`

### Repository Layer
- Do **not** create unit tests for repositories unless custom business logic exists. (Repository testing requires integration tests, not unit tests).

## Required Scenarios
Verify: Success path, validation failures, exception scenarios, null handling, empty collections, and boundary values.

---

## Gotchas

- **Guid Primary Keys**: All entities use `Guid` identifiers. Do not use integer identifiers in tests.
- **Soft Delete**: Entities use `IsDeleted`. Verify `IsDeleted` becomes true on delete operations, and active queries exclude deleted records. (See `references/soft-delete-testing.md`)
- **Authentication**: Authenticated endpoints read `UserId` from JWT claims. Do not pass `UserId` through request payloads when production code reads `UserId` from claims. (See `references/jwt-testing.md`)
- **Mocking**: Mock all external dependencies. Never connect to SQL Server during unit tests. (See `references/repository-mocking.md`)

---

## Gates

### Gate 1: Test Verification
Run the verification script to ensure naming conventions and AAA structure are followed:
```powershell
scripts/verify-tests.ps1
```

### Gate 2: Validation
Before returning tests, ensure:
- [ ] Tests compile successfully.
- [ ] Naming conventions followed (`MethodName_Scenario_ExpectedResult`).
- [ ] Dependencies mocked (No SQL Server calls).
- [ ] Assertions verify behavior.
- [ ] Success and Failure scenarios covered.

---

## References & Assets

- `assets/test-template.cs` - Standard NUnit test class template.
- `references/service-testing.md`
- `references/controller-testing.md`
- `references/repository-mocking.md`
- `references/jwt-testing.md`
- `references/soft-delete-testing.md`
