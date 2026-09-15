# Global Error Handling

The application uses a **Global Exception Middleware** to intercept all unhandled exceptions. This guarantees that API responses are consistent and secure.

## Rule: Never use `try/catch` in Controllers

Controllers should execute the "Happy Path". If something goes wrong in the application, an exception should be thrown in the Service layer. The middleware intercepts it automatically. 

**DO NOT** do this:
```csharp
// BAD
try {
   await _service.DoWork();
} catch (Exception ex) {
   return StatusCode(500, ex.Message); // Exposes internal system details!
}
```

## Throwing Business Exceptions

If a business rule is violated in the Service layer, throw a custom exception. The Global Middleware will catch this and translate it into the appropriate HTTP status code (e.g., `400 Bad Request`, `404 Not Found`).

```csharp
// Inside Service Layer
if (timecard == null)
{
    // The middleware will catch this and return a structured 404 response
    throw new NotFoundException($"Timecard {timecardId} not found.");
}
```

## Standard Error Response Format

The middleware formats all errors into a standard JSON structure. AI should not attempt to build this structure manually in the Controller.

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred processing your request.",
  "traceId": "0HL12345ABCDE"
}
```
