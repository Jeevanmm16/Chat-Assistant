# Soft Delete Testing

The project uses Soft Deletes (`IsDeleted = true`) instead of hard deletions (`DbContext.Remove`).

When testing delete endpoints or services, verify that:
1. The repository's `Update` method is called (not `Delete` or `Remove`).
2. The entity's `IsDeleted` property is explicitly checked and confirmed to be `true`.

## Example
```csharp
[Test]
public async Task DeleteUser_ValidRequest_SetsIsDeletedTrue()
{
    // Arrange
    var user = new User { Id = Guid.NewGuid(), IsDeleted = false };
    _mockRepo.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

    // Act
    await _service.DeleteUserAsync(user.Id);

    // Assert
    Assert.IsTrue(user.IsDeleted);
    _mockRepo.Verify(x => x.Update(user), Times.Once);
}
```
