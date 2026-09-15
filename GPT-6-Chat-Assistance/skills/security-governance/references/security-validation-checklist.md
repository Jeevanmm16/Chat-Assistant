# Security Validation Checklist

Before committing any security-related code or API endpoints, verify the following:

- [ ] No hardcoded secrets exist in the source code.
- [ ] JWT configuration is valid and pulls keys from `IConfiguration`.
- [ ] Password hashing is implemented using `BCrypt` (not the Encryption helper).
- [ ] Authorization attributes (`[Authorize]`) are applied to endpoints properly.
- [ ] Sensitive data (passwords, tokens, keys) is NOT being logged.
- [ ] `UserId` is pulled from claims, not trusted from the client payload.
- [ ] HTTPS is enforced.
- [ ] Detailed exceptions are hidden from the client response.
- [ ] Security checklist completed.
