# Async Programming Standards

In a highly concurrent web application, thread starvation is the leading cause of application hangs. 

## The Golden Rule
**Async all the way down.** If a method calls an async method, it must also be async.

### ❌ Bad (Thread Blocking)
```csharp
public UserDto GetUser(Guid id)
{
    // This locks a thread from the ThreadPool while waiting for the DB
    var user = _repository.GetByIdAsync(id).Result; 
    return Map(user);
}
```

### ✅ Good (Non-Blocking)
```csharp
public async Task<UserDto> GetUserAsync(Guid id)
{
    // This releases the thread back to the ThreadPool while waiting for the DB
    var user = await _repository.GetByIdAsync(id);
    return Map(user);
}
```

## `Task.Run` Warning
Do not use `Task.Run()` inside ASP.NET Core controllers or services just to "make it async". `Task.Run()` queues work on the ThreadPool, which does not provide any scalability benefits in a web server context (where requests are already running on the ThreadPool). Only use `Task.Run()` for true CPU-bound background work.
