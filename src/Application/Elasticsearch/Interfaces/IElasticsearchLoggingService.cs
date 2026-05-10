namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchLoggingService
{
    Task AddSearchLogging<T>(T request, int? resultsCount, CancellationToken cancellationToken);
    Task AddLogging<T>(T request, long elapsedMilliseconds, CancellationToken cancellationToken);
    Task ExceptionLogging<T>(T request, Exception exception, CancellationToken cancellationToken);
    Task SetupLoggingSettings(CancellationToken cancellationToken);
    Task ResetLoggingIndices(CancellationToken cancellationToken);
}
