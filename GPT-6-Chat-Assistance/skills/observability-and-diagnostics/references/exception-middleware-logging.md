# Exception Middleware Logging

Do not log exceptions manually in the Controller layer using `try/catch`. 

The project uses a **Global Exception Middleware** that automatically logs the stack trace, HTTP context, and exception message before returning a sanitized 500 error to the client.

If you are in the **Service Layer** and catch a specific exception to perform a retry or fallback, you may log it there:
```csharp
try {
    await _externalApi.SubmitAsync(data);
} catch (HttpRequestException ex) {
    _logger.LogError(ex, "Failed to submit data to external API for user {UserId}", userId);
    throw; // Rethrow to let the middleware handle the HTTP response
}
```
