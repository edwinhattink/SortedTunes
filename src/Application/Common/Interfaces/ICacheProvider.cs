using Microsoft.Extensions.Caching.Memory;

namespace SortedTunes.Application.Common.Interfaces;

public interface ICacheProvider
{
    T? GetFromCache<T>(string key) where T : class;
    void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class;
}
