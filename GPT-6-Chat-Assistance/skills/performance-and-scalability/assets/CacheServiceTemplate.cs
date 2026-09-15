using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace <ProjectName>Core.Service
{
    public class CacheServiceTemplate
    {
        private readonly IMemoryCache _cache;

        public CacheServiceTemplate(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetCachedDataAsync(string key, Func<Task<string>> fetchFunction)
        {
            if (!_cache.TryGetValue(key, out string cachedValue))
            {
                // Key not in cache, so fetch data.
                cachedValue = await fetchFunction();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    // Absolute expiration to force a refresh eventually.
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                // Save data in cache.
                _cache.Set(key, cachedValue, cacheEntryOptions);
            }

            return cachedValue;
        }
    }
}
