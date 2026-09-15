# Hangfire Best Practices

## 1. Keep Payloads Small
When enqueuing fire-and-forget jobs, do not pass large objects (like full Entities) as parameters. Hangfire serializes method arguments into the database.
**Pass IDs, not Entities.**

```csharp
// ❌ Bad: Serializes the whole entity into SQL
BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync(userEntity));

// ✅ Good: Only serializes the Guid
BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync(userEntity.Id));
```

## 2. Cancellation Tokens
Hangfire supports cancellation. Long-running jobs should accept an `IJobCancellationToken` (or standard `CancellationToken`) and pass it down to database and network calls so jobs can be aborted gracefully.

## 3. Retries
By default, Hangfire retries failed jobs multiple times. If your job performs non-idempotent operations (like charging a credit card), you MUST design the job to check for previous completion or explicitly disable retries using `[AutomaticRetry(Attempts = 0)]`.
