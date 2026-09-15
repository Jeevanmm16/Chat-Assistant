---
name: security-governance
description: Enforce authentication, authorization, secret management, secure coding practices, JWT standards, and security validation requirements. Use before implementing authentication, authorization, secret management, or secure logging.
---

## Purpose
Enforce authentication, authorization, secret management, secure coding practices, JWT standards, and security validation requirements across the `<ProjectName>Core` architecture.

---

## 1. Authentication Pipeline

In `Program.cs`, the pipeline must be registered and executed in the correct order.

**Registration:**
```csharp
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
```

**Middleware Execution Order:**
```csharp
app.UseAuthentication();
app.UseAuthorization();
```
*(Gotcha: Placing `UseAuthorization` before `UseAuthentication` causes all requests to appear anonymous.)*

---

## 2. Password Standards

- **Passwords must never be stored in plain text.**
- **Passwords must never be encrypted.** They must be one-way hashed.
- Use `BCrypt` for password hashing.
- **Passwords or hashes must never be logged.**

**Example:**
```csharp
var hash = BCrypt.Net.BCrypt.HashPassword(password);
var valid = BCrypt.Net.BCrypt.Verify(password, hash);
```

---

## 3. Token & Session Management

- Access tokens must be short-lived.
- Refresh tokens must be stored securely.
- Refresh tokens must never be logged.
- Expired access tokens must not be reused.

---

## 4. Secrets & Credentials Management

- **Development**: Use `appsettings.Development.json` or User Secrets.
- **Production**: Use Azure Key Vault or Environment Variables.

**❌ Forbidden**:
- Hardcoded secrets in source code.
- Committing production secrets into source control (`appsettings.json`).
- Storing secrets inside Controllers or Services directly.

### External Integrations
- API keys, OAuth secrets, connection strings, and webhook secrets must be loaded from configuration providers (e.g. `appsettings.json` or environment variables).
- **Never hardcode third-party credentials.**

---

## 5. Authorization & Claims Rules

- **Authentication** verifies identity. **Authorization** verifies permissions.

### Claims Standards
**Required Claims:**
- `NameIdentifier` (UserId)
- `Role`
- `Email` (if applicable)

Services must read claims through the authenticated principal and not from request payloads. This prevents AI from inventing custom claim names or bypassing validation.

### Authorization Approach
- Prefer **Authorization Policies** for complex permissions (e.g. `[Authorize(Policy = "CanManageTimecards")]`).
- Use **Roles** only for coarse-grained access control (e.g. `[Authorize(Roles = "Admin")]`).
- **Identity Integrity**: Service methods must **not** trust user input for `UserId`.
  - The `UserId` must ALWAYS be obtained from the authenticated JWT claims or `HttpContext.Items`, never from a DTO payload.

---

## 6. Cryptography & Encryption

When encrypting sensitive application data (PII, configuration values, external secrets), **MUST use the project's approved Encryption helper**.
- **Do not write custom `Aes.Create()` logic in services.**
- The encryption helper should be exposed through an abstraction (e.g. interface) accessible to the consuming layer to prevent layer reference violations (e.g. Service layer referencing the API layer).

*(Note: Passwords are NOT encrypted. Passwords must be hashed using BCrypt.)*

---

## 7. Secure Logging

**❌ Never Log:**
- Passwords
- JWT tokens or Refresh tokens
- Connection strings
- Secret keys
- PII (unless explicitly required and masked)

**✅ Allowed Logging:**
- `UserId` (when tracing actions)
- CorrelationId
- Request path
- Exception details may be logged internally through the centralized logging pipeline. **Exception messages must not be returned directly to API consumers.**

---

## 8. API Security & Error Responses

- Validate all incoming DTOs.
- **HTTPS Enforcement**: Use HTTPS only. Do not generate HTTP endpoints. Redirect HTTP traffic to HTTPS in production.
- Use DTOs instead of Entities to prevent Mass Assignment attacks.
- Sanitize user input before persistence.

**Error Response Standards:**
- Authentication failures → `401 Unauthorized`
- Authorization failures → `403 Forbidden`
- Validation failures → `400 Bad Request`
- Resource not found → `404 Not Found`
- Unhandled exceptions → Handled by Exception Middleware.

**Never return:** Stack traces, connection strings, or internal exception details.

---

## 9. Package Governance

**✅ Allowed:**
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `BCrypt.Net-Next`
- `Microsoft.IdentityModel.Tokens`

**❌ Forbidden:**
- Custom cryptography libraries without approval.
- Deprecated JWT packages.

---

## 10. Security Workflow

1. Define authentication requirements.
2. Define authorization requirements (role vs. policy).
3. Configure claims and policies.
4. Validate DTO inputs.
5. Implement business logic.
6. Verify secure logging.
7. Run Gate 1.
8. Run Gate 2.

---

## Gates

### Gate 1: Security Audit
Run the automated linters to check for hardcoded secrets, pipeline ordering, and missing authorization filters:
```powershell
scripts/verify-security-standards.ps1
scripts/verify-controller-authorization.ps1
```
- **verify-security-standards.ps1**: Flags inline AES implementation, pipeline middleware misordering, and hardcoded `GetBytes()` secrets.
- **verify-controller-authorization.ps1**: Flags any Controller endpoint missing `[Authorize]`, `[AllowAnonymous]`, or the custom `TokenAuthorizationFilterAttribute` decorator.

### Gate 2: Security Validation Checklist
Before finalizing code, verify:
- [ ] No hardcoded secrets exist.
- [ ] Passwords hashed using BCrypt.
- [ ] Authorization attributes applied (policies preferred for complex logic).
- [ ] `UserId` sourced from claims, not payload.
- [ ] Sensitive data not logged.
- [ ] DTO validation implemented.
- [ ] HTTPS enforced.
- [ ] Security validation checklist executed.

---

## Gotchas

- **Payload UserId** — The most common AI mistake. `UserId` should come from JWT claims, NOT request payloads.
- **Password Encryption** — Passwords use BCrypt, NOT the Encryption helper.
- **JWT Configuration** — JWT expiration must always be configured.
- **Missing Role Claims** — Causes authorization failures. Ensure claims are attached during token generation.
- **Middleware Order** — Must remain: `UseAuthentication()` then `UseAuthorization()`.
- **Exception Leaks** — Never return detailed exception messages to clients.
- **Layer Violations (API Reference)** — Do not reference API namespace or helpers directly from the Service layer. Use proper abstractions/interfaces.

---

## References

- `references/jwt-troubleshooting.md`
  Use this guide to diagnose 401s, 403s, invalid signatures, and missing claims.
