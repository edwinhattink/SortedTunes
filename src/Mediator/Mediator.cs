using System.Collections.Concurrent;
using SortedTunes.Mediator.Contracts;
using SortedTunes.Mediator.Internal;

namespace SortedTunes.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> RequestHandlers = new();
    private static readonly ConcurrentDictionary<Type, NotificationHandlerWrapper> NotificationHandlers = new();

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var handler = (RequestHandlerWrapper<TResponse>)RequestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse));
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        return handler.Handle(request, serviceProvider, cancellationToken);
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        ArgumentNullException.ThrowIfNull(request);

        var handler = (RequestHandlerWrapper)RequestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestHandlerWrapperImpl<>).MakeGenericType(requestType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        return handler.Handle(request, serviceProvider, cancellationToken);
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);
        return PublishNotification(notification, cancellationToken);
    }

    private async Task PublishNotification(INotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();

        var wrapper = NotificationHandlers.GetOrAdd(notificationType, static type =>
        {
            var wrapperType = typeof(NotificationHandlerWrapperImpl<>).MakeGenericType(type);
            return (NotificationHandlerWrapper)Activator.CreateInstance(wrapperType)!;
        });

        await wrapper.Handle(notification, serviceProvider, cancellationToken);
    }
}
