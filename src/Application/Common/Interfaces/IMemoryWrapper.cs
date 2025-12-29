namespace SortedTunes.Application.Common.Interfaces;
public interface IMemoryWrapper
{
    Task<T> GetCachedResponse<T>(string memoryKey, Task<T> task) where T : class;
}
