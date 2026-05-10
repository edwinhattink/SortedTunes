using SortedTunes.Mediator.Contracts;

namespace SortedTunes.Mediator;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}
