# JWT Troubleshooting Guide

Review this guide when investigating Authentication (401) or Authorization (403) failures in the `<ProjectName>Core` application.

## 1. Invalid Token Signature
**Symptoms**: Application throws `SecurityTokenInvalidSignatureException`.
**Verify**:
- **Secret Key**: Ensure the `CoreLayerSecretKey` matches the key used to generate the token.
- **Issuer / Audience**: Ensure `ValidateAudience` and `ValidateIssuer` parameters match the generation logic.
- **Algorithm**: Ensure both generation and validation use `SecurityAlgorithms.HmacSha256`.

## 2. Expired Token
**Symptoms**: Application throws `SecurityTokenExpiredException`.
**Verify**:
- Check `token.ValidTo`.
- Verify the server's UTC time (`DateTime.UtcNow`). JWTs rely on strictly synchronized server clocks.

## 3. Missing Claims
**Symptoms**: User is authenticated but context mapping fails.
**Verify**:
- Ensure `new Claim(JwtClaimTypes.Subject, userId.ToString())` (or `ClaimTypes.NameIdentifier`) exists when the token is generated.
- Ensure `new Claim(JwtClaimTypes.Role, roleName)` is added for every distinct role the user possesses.

## 4. Unauthorized (401)
**Symptoms**: All protected endpoints reject requests.
**Verify**:
- `builder.Services.AddAuthentication()` is properly registered in `Program.cs`.
- The `Authorization` header is correctly formatted (e.g., `Bearer <token>`).
- The `TokenAuthorizationFilterAttribute` is correctly extracting the token from headers and assigning context items (`Constants.UserId`, etc.).

## 5. Forbidden (403)
**Symptoms**: User can log in, but receives 403 on specific endpoints.
**Verify**:
- Check `[Authorize(Roles = "Admin")]`.
- Verify the user's decoded JWT actually contains the matching `role` claim. (You can use jwt.io to safely decode and inspect non-production tokens).

## 6. Authentication Pipeline Execution
**Symptoms**: Intermittent or silent auth failures.
**Verify**:
- Ensure middleware executes in the exact sequence in `Program.cs`:
  ```csharp
  app.UseAuthentication();
  app.UseAuthorization();
  ```
  **Authentication must ALWAYS execute before Authorization.**
