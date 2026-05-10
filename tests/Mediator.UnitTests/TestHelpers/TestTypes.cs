using SortedTunes.Mediator.Contracts;

namespace SortedTunes.Mediator.UnitTests.TestHelpers;

internal class TestRequest : IRequest<string>;

internal class TestRequestHandler : IRequestHandler<TestRequest, string>
{
    public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        => Task.FromResult("test-response");
}

internal class TestVoidRequest : IRequest;

internal class TestVoidRequestHandler : IRequestHandler<TestVoidRequest>
{
    public Task Handle(TestVoidRequest request, CancellationToken cancellationToken)
        => Task.CompletedTask;
}

internal class TestNotification : INotification;

internal class TestNotificationHandler : INotificationHandler<TestNotification>
{
    public static int HandleCount;

    public Task Handle(TestNotification notification, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref HandleCount);
        return Task.CompletedTask;
    }
}

internal class SecondTestNotificationHandler : INotificationHandler<TestNotification>
{
    public static int HandleCount;

    public Task Handle(TestNotification notification, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref HandleCount);
        return Task.CompletedTask;
    }
}

internal class TestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public static bool WasCalled;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        WasCalled = true;
        return await next(cancellationToken);
    }
}

internal class UnregisteredRequest : IRequest<string>;
internal class UnregisteredVoidRequest : IRequest;
