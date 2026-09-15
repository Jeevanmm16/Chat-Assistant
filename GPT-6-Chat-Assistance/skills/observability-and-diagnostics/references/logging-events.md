# Logging Event Standards

When writing `_logger` statements, use **Structured Logging**. 

**❌ Bad (String Interpolation):**
```csharp
_logger.LogInformation($"User {userId} logged in at {DateTime.UtcNow}");
```

**✅ Good (Structured Logging):**
```csharp
_logger.LogInformation("User {UserId} logged in at {LoginTime}", userId, DateTime.UtcNow);
```

### Event Triggers

1. **API Entry Points**: Log high-level requests entering the system (usually handled by middleware, but log custom DTO attributes if necessary).
2. **Business Events**: Important state changes (e.g., "Timecard Approved", "User Registered").
3. **Warnings**: Situations that are unexpected but do not stop execution.
