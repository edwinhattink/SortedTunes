using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Infrastructure.Services.Elasticsearch.Models.Logs;

public class ExceptionLog<T>
{
    [KeywordField]
    public string? UserId { get; set; }

    public required DateTime DateTime { get; set; }

    [SearchField]
    [KeywordField]
    public required string RequestName { get; set; }

    [SearchField]
    [KeywordField]
    public required string ExceptionType { get; set; }

    [NestedField]
    public required T Request { get; set; }

    [SearchField]
    public string? Message { get; set; }

    [SearchField]
    public string? Stacktrace { get; set; }
}
