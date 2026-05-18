using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Infrastructure.Services.Elasticsearch.Models.Logs;

public class SearchLog<T>
{
    [KeywordField]
    public string? UserId { get; set; }

    public DateTime DateTime { get; set; }

    [SearchField]
    [KeywordField]
    public required string RequestName { get; set; }

    [NestedField]
    public required T Request { get; set; }

    public int? ResultsCount { get; set; }
}
