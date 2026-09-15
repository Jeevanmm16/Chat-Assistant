---
name: observability-and-diagnostics
description: Defines logging, monitoring, tracing, debugging workflows, exception investigation, and production diagnostics practices. Use when writing log statements, configuring diagnostics, or debugging.
---

## Purpose
Ensure consistent logging across all APIs. Defines logging, monitoring, tracing, debugging workflows, exception investigation, and production diagnostics practices.

## Logging Standards

### Custom Logging Framework
Always use the project's injected logging abstraction (`ILogger<T>`).
```csharp
_logger.LogInformation("Processing timecard for user {UserId}", userId);
```

**❌ Do Not Use:**
- `Console.WriteLine()`
- `Debug.WriteLine()`

### What to Log
You must explicitly log:
- API entry points
- Business events (e.g., entity creation, state changes)
- Warnings (e.g., missing non-critical data)
- Errors and Exceptions
*(See `references/logging-events.md` for specific formatting rules).*

### What NOT to Log (Sensitive Information)
**Never log:**
- Passwords
- JWT tokens
- Connection strings
- API secrets

### Error Logging
Exceptions should be logged using the standard logging framework and handled implicitly by the Global Exception Middleware. Do not write `try/catch` blocks in controllers just to log errors. *(See `references/exception-middleware-logging.md`)*.

---

## Validation Workflow

Before returning code, ensure you have run the automated checks to verify logging standards.

### Gate 1: Automated Verification
Run the logging verification script:
```powershell
scripts/verify-logging.ps1
```
This script checks for forbidden `Console.WriteLine()` calls and ensures sensitive fields aren't being explicitly logged.

---

## References & Assets

- `references/logging-events.md` (Detailed event tracking conventions)
- `references/exception-middleware-logging.md` (Rules for logging exceptions)
- `assets/LoggingTemplate.cs` (Example class with DI logging setup)
