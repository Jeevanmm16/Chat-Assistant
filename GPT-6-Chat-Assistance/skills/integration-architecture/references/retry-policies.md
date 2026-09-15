# Retry Policies with Polly

When configuring external HTTP clients in `Program.cs`, attach a Polly retry policy to handle transient network errors (like 503 Service Unavailable or 408 Request Timeout).

## Registration Example

```csharp
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApi:BaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddTransientHttpErrorPolicy(policyBuilder =>
    policyBuilder.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
);
```

**Never** implement try/catch loops with `Task.Delay()` manually inside the Service class for HTTP retries. Use Polly in the DI container.
