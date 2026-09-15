# JWT Testing & Authentication Mocks

When testing Controllers that rely on JWT Claims (e.g., reading `UserId` from the token instead of the request body), you must mock the `HttpContext` to simulate an authenticated user.

## Example Context Mock

```csharp
[SetUp]
public void Setup()
{
    var userId = Guid.NewGuid().ToString();
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, "Admin")
    };
    
    var identity = new ClaimsIdentity(claims, "TestAuthType");
    var claimsPrincipal = new ClaimsPrincipal(identity);

    var httpContext = new DefaultHttpContext
    {
        User = claimsPrincipal
    };

    _controller = new UserController(_mockService.Object)
    {
        ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        }
    };
}
```
