# Service Layer Testing

When testing Services, the primary goal is to validate **Business Logic**.

## Do Test:
- Business rules (e.g., preventing a Timecard submission if it's already approved).
- Conditional flows (e.g., mapping properties conditionally).
- Exception handling (e.g., does it throw a `NotFoundException` if the entity doesn't exist?).
- Repository interactions (Verify `repository.AddAsync()` is called once using `Mock.Verify`).

## Do Not Test:
- Entity Framework operations. (Mock the Repository instead).
- HTTP Status Codes (This belongs in the Controller test).

## Example:
```csharp
[Test]
public async Task ProcessTimecard_WhenTimecardNotFound_ThrowsNotFoundException()
{
    // Arrange
    var timecardId = Guid.NewGuid();
    _mockRepository.Setup(x => x.GetByIdAsync(timecardId)).ReturnsAsync((Timecard)null);

    // Act & Assert
    var ex = Assert.ThrowsAsync<NotFoundException>(() => _service.ProcessTimecard(timecardId));
    Assert.That(ex.Message, Does.Contain(timecardId.ToString()));
}
```
