---
name: background-job-processing
description: Defines background processing patterns, scheduled jobs, queue processing, Hangfire usage, and long-running task execution standards. Use when creating or modifying background tasks.
---

## Purpose
Standardize the implementation of scheduled, recurring, and fire-and-forget background tasks.

## 1. Background Processing Framework
The project uses **Hangfire** for all background job processing.
- Do not use `IHostedService` or `BackgroundService` for long-running scheduled tasks unless strictly dealing with infrastructure that cannot use Hangfire.
- Do not use `Task.Run` in HTTP requests as a substitute for fire-and-forget jobs.

## 2. Job Implementation Rules
- **Statelessness**: Hangfire jobs must be stateless. Do not store state in memory between executions.
- **Idempotency**: Jobs can fail and retry automatically. Ensure the job's logic is safe to run multiple times without causing duplicates or data corruption.
- **Dependency Injection**: Inject dependencies via the constructor. Hangfire integrates with the ASP.NET Core DI container.

*(See `assets/HangfireJobTemplate.cs`)*

## 3. Scheduling Jobs
Schedule jobs during application startup in `Program.cs` or an initialization extension method.

```csharp
RecurringJob.AddOrUpdate<IMyBackgroundJob>(
    "DailyDataSync",
    job => job.ExecuteAsync(),
    Cron.Daily);
```

## 4. Error Handling
- Do not suppress exceptions inside the job using blanket `catch (Exception)` unless you are explicitly logging and rethrowing.
- Let the exception bubble up so Hangfire can capture it, record the failure, and trigger its built-in retry mechanism.

*(See `references/hangfire-best-practices.md`)*

---

## Validation Workflow

### Gate 1: Job Verification
Run the verification script to check for improper background thread usage:
```powershell
scripts/verify-jobs.ps1
```

---

## References & Assets

- `references/hangfire-best-practices.md`
- `assets/HangfireJobTemplate.cs`
