---
name: integration-architecture
description: Defines external system integrations, REST API consumption, webhooks, third-party services, retry policies, and integration patterns.
---

## Purpose
Ensure robust, fault-tolerant, and standardized communication between `<ProjectName>Core` and external APIs or third-party services.

## 1. HTTP Client Rules
- **Never instantiate `HttpClient` directly using `new HttpClient()`**. This leads to socket exhaustion.
- Always use `IHttpClientFactory` or Typed Clients registered in `Program.cs`.

*(See `assets/HttpClientTemplate.cs` for the proper Typed Client implementation).*

## 2. Resilience and Retry Policies
- External APIs fail. You must assume network instability.
- Implement Retry Policies using **Polly** (e.g., Retry 3 times with exponential backoff).
- Do not write custom `Thread.Sleep` retry loops. Use `Microsoft.Extensions.Http.Polly`.

*(See `references/retry-policies.md`)*

## 3. Timeout Configuration
- Every external HTTP call must have an explicit timeout configured (default 30 seconds). Never leave a client waiting infinitely.

## 4. Webhooks
- Webhook endpoints receiving data from external services must validate the incoming signature/HMAC before processing.
- Offload processing to a background job if the webhook payload takes longer than 3 seconds to process.

---

## Validation Workflow

### Gate 1: HttpClient Audit
Run the automated verification script to ensure `new HttpClient()` is not used.
```powershell
scripts/verify-httpclient.ps1
```

---

## References & Assets

- `references/retry-policies.md`
- `assets/HttpClientTemplate.cs`
