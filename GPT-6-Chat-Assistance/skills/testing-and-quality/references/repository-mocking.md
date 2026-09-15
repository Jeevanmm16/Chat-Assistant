# Repository Mocking

When unit testing Services, you MUST mock external dependencies, including Repositories. **Never attempt to create an actual `DbContext` or connect to SQL Server during Unit Tests.**

## Best Practices
1. Mock the **Repository Interface**, not the DbContext.
2. Use `Moq` to set up expected behaviors and return values.

## Example
```csharp
// Arrange
var mockUserRepository = new Mock<IUserRepository>();

// Setup a mock to return a specific user when GetByIdAsync is called
mockUserRepository
    .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
    .ReturnsAsync(new User { Id = Guid.NewGuid(), Name = "Mocked User" });

// Setup a mock to just return an empty list
mockUserRepository
    .Setup(repo => repo.GetAllActiveAsync())
    .ReturnsAsync(new List<User>());

// Verify a method was called
mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
```
