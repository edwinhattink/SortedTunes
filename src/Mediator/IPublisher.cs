using SortedTunes.Mediator.Contracts;

namespace SortedTunes.Mediator;

public interface IPublisher
{
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
}
