namespace SortedTunes.Application.Common.Models;

public abstract class PaginatedQuery
{
    /// <summary>
    /// Optional page number, default 1
    /// </summary>
    public int? PageNumber { private get; init; }
    /// <summary>
    /// Optional page size, default: 10
    /// </summary>
    public int? PageSize { private get; init; }

    internal const int DefaultPageNumber = 1;
    internal const int DefaultPageSize = 10;

    public int GetPageNumber() => PageNumber ?? DefaultPageNumber;
    public int GetPageSize() => PageSize ?? DefaultPageSize;
}
