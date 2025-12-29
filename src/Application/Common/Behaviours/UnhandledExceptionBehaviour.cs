using SortedTunes.Application.Elasticsearch.Interfaces;
using Microsoft.Extensions.Logging;

namespace SortedTunes.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    IElasticsearchLoggingService elasticsearchLoggingService
)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var name = typeof(TRequest).Name;

            logger.LogError(ex, "SortedTunes Request: Unhandled Exception for Request {Name} {@Request}", name, request);
            await elasticsearchLoggingService.ExceptionLogging(request, ex, cancellationToken);

            throw;
        }
    }
}
