using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Mediator;

namespace SortedTunes.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    IElasticsearchLoggingService elasticsearchLoggingService
) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next(cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        var requestName = typeof(TRequest).Name;

        if (elapsedMilliseconds > 500)
        {
            var userId = "Anonymous";
            //TODO: var userId = currentUserService.IsLoggedIn ? currentUserService.GetUser().AuthId : "";

            logger.LogWarning("SortedTunes Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                requestName, elapsedMilliseconds, userId, request);
        }

        await elasticsearchLoggingService.AddLogging(request, elapsedMilliseconds, cancellationToken);

        return response;
    }
}
