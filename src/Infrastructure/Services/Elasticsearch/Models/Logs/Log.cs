using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Infrastructure.Services.Elasticsearch.Models.Logs;

public class Log<T>
{
    [KeywordField]
    public string? UserId { get; set; }

    public required DateTime DateTime { get; set; }

    [SearchField]
    [KeywordField]
    public required string RequestName { get; set; }

    [NestedField]
    public required T Request { get; set; }

    public required long ElapsedMilliseconds { get; set; }
}

