# Caching Strategies

Repeatedly querying the database for static data degrades performance.

## Rules for Caching
1. **Never Cache User-Specific PII**: Unless explicitly using distributed sessions with encryption.
2. **Always Use Expiration**: Infinite caches lead to memory leaks. Use Absolute or Sliding Expiration.

### Example: IMemoryCache
```csharp
public async Task<List<LookupDto>> GetLookupsAsync()
{
    const string cacheKey = "SystemLookups";

    if (!_cache.TryGetValue(cacheKey, out List<LookupDto> lookups))
    {
        lookups = await _repository.GetLookupsAsync();
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        _cache.Set(cacheKey, lookups, cacheEntryOptions);
    }

    return lookups;
}
```
