---
name: api-design-standards
description: Enforce strict API design conventions, structured logging, Swagger documentation, and global error handling. Use before creating or modifying any API Controllers, endpoints, or DTOs.
---

## Purpose
Enforce strict API design conventions, structured logging, Swagger documentation, and global error handling for all endpoints in the `<ProjectName>Core.API` project.

---

## 1. Controller & Endpoint Rules

### Required Attributes
Every Controller endpoint MUST include the following attributes:
- **Authorization**: `[TypeFilter(typeof(TokenAuthorizationFilterAttribute))]` (unless explicitly anonymous)
- **Unit of Work / Logging**: `[TypeFilter(typeof(UnitOfWorkAndLoggerAttribute))]`
- **Swagger Documentation**: `[SwaggerOperation(Summary = "...", Description = "...")]`
- **Swagger Responses**: Must explicitly define expected HTTP status codes (e.g., `200`, `400`, `401`, `404`, `500`) using `[SwaggerResponse]`.

### Route Constants
**NEVER** hardcode route strings in the `[Route("...")]` attribute. 
- All API routes MUST be defined as constants inside `APIUrlConstants.cs`.
- Example: `[Route(APIUrlConstants.PostProcessMultipleToWorkDay)]`

---

## 2. Error Handling (Strictly Enforced)

**❌ FORBIDDEN: Try/Catch in Controllers**
Controllers MUST NOT contain `try/catch` blocks. 
- Do not handle exceptions in the Controller.
- Do not return `StatusCode(500, ex.Message)` directly.
- **Why**: The application uses a **Global Exception Middleware** to catch unhandled exceptions, log them securely, and return standardized error responses.

**✅ ALLOWED: Expected Business Errors**
If the Service layer returns an expected failure (e.g., "Timecard not found"), the Controller should return the appropriate HTTP code (e.g., `NotFound()`, `BadRequest()`).

---

## 3. Logging & PII Governance

- **Structured Logging**: Use structured logging (e.g., `_logger.LogInformation("Processing timecard {TimecardId}", request.TimecardId)`) instead of string interpolation.
- **PII / Secrets**: NEVER log sensitive information (Passwords, Tokens, Social Security Numbers, full Request payloads containing PII).

---

## 4. The "Golden" Controller Template

Here is the exact structure expected for a compliant endpoint. Notice the absence of `try/catch`.

```csharp
[TypeFilter(typeof(TokenAuthorizationFilterAttribute))]
[TypeFilter(typeof(UnitOfWorkAndLoggerAttribute))]
[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Result))]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
[SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(string))]
[SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(string))]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
[HttpPost]
[Route(APIUrlConstants.PostProcessMultipleToWorkDay)]
[SwaggerOperation(Summary = "Send Multiple Timecards to Workday", Description = "Sends multiple timecard data to Workday.")]
public async Task<IActionResult> CreateMultiplePunches([FromBody] PostMultipleWorkDayRequest request)
{
    // 1. Basic Request Validation (HTTP concerns only)
    if (request?.TimecardIds == null || !request.TimecardIds.Any())
        return BadRequest("TimecardIds are required");

    // 2. Call Service (NO try/catch block!)
    await _workdayService.ProcessMultipleTimecardsToWorkDayAsync(request.TimecardIds);
    
    // 3. Return Success
    return Ok("Timecards successfully sent to Workday.");
}
```

---

## Workflow

1. Define the route constant in `APIUrlConstants.cs` (or equivalent constants file).
2. Define the Request and Response DTOs.
3. Create the Controller method.
4. Apply all required `TypeFilter` and `Swagger` attributes.
5. Implement the logic (Validate Request → Call Service → Return Result).
6. Verify no `try/catch` blocks exist in the method.
7. Run Gate 1.

---

## Gates

### Gate 1: API Standard Verification
Run the verification script to instantly detect forbidden patterns like `try/catch`, hardcoded routes, and string interpolation in logs.
```powershell
scripts/verify-api-standards.ps1
```
- ✅ Pass → No forbidden patterns found.
- ❌ Fail → Script explicitly points out which Controller violates the rule.

### Gate 2: Build Analysis
Run a build to ensure the code compiles and no warnings are triggered.
```powershell
dotnet build src/<ProjectName>Core.API/<ProjectName>Core.API.csproj
```

---

## Gotchas

- **Try/Catch in Controllers** — AI frequently wraps Controller methods in `try/catch`. This is an immediate failure. Let the Global Exception Middleware handle it.
- **Hardcoded Routes** — AI frequently writes `[Route("api/v1/timecards")]`. This is an immediate failure. You MUST use the constants file.
- **String Interpolation Logging** — AI frequently writes `_logger.LogInformation($"User {userId} logged in")`. This breaks structured logging. Use `_logger.LogInformation("User {UserId} logged in", userId)`.
- **Missing Swagger Responses** — Endpoints must document `500`, `401`, etc., even if the Controller code doesn't explicitly throw them, because the middleware will.
- **Leaking Exception Details** — Never return `ex.Message` to the client. It exposes internal system details and poses a massive security risk.

---

## References

- `references/error-handling.md`
  Details how the Global Exception Middleware works and how to throw Custom Exceptions from the Service layer.
