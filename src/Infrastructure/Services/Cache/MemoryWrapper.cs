using SortedTunes.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace SortedTunes.Infrastructure.Services.Cache;

public class MemoryWrapper(ICacheProvider provider) : IMemoryWrapper
{
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromDays(1),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
    };

    public async Task<T> GetCachedResponse<T>(string memoryKey, Task<T> task) where T : class
    {
        var cachedData = provider.GetFromCache<T>(memoryKey);
        if (cachedData == null)
        {
            var newData = await task;
            provider.SetCache(memoryKey, newData, _memoryCacheEntryOptions);

            return newData;
        }

        return cachedData;
    }
}
