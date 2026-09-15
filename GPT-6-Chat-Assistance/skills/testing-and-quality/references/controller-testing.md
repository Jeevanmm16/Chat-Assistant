# Controller Layer Testing

When testing Controllers, the primary goal is to validate **HTTP behavior**.

## Do Test:
- Status codes (e.g., returns `200 OK`, `400 BadRequest`, `404 NotFound`).
- Request validation (Does it reject bad inputs before calling the service?).
- Correct service invocations (Does it pass the right parameters to the Service?).

## Do Not Test:
- Business logic (This should be tested in the Service).
- Authorization/Authentication filters directly (unless writing Integration Tests).

## Example:
```csharp
[Test]
public async Task GetUser_WhenUserExists_ReturnsOkResult()
{
    // Arrange
    var userId = Guid.NewGuid();
    var userDto = new UserDto { Id = userId, Name = "Test" };
    _mockService.Setup(s => s.GetUserAsync(userId)).ReturnsAsync(userDto);

    // Act
    var result = await _controller.GetUser(userId);

    // Assert
    var okResult = result as OkObjectResult;
    Assert.IsNotNull(okResult);
    Assert.AreEqual(200, okResult.StatusCode);
    Assert.AreEqual(userDto, okResult.Value);
}
```
