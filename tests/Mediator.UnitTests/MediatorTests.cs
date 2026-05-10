using Microsoft.Extensions.DependencyInjection;
using SortedTunes.Mediator.Contracts;
using SortedTunes.Mediator.UnitTests.TestHelpers;

namespace SortedTunes.Mediator.UnitTests;

[TestFixture]
public class MediatorTests
{
    private IServiceProvider _serviceProvider = null!;

    [SetUp]
    public void Setup()
    {
        TestNotificationHandler.HandleCount = 0;
        SecondTestNotificationHandler.HandleCount = 0;
        TestPipelineBehavior<TestRequest, string>.WasCalled = false;
    }

    private IMediator CreateMediator(Action<MediatorServiceConfiguration>? configure = null)
    {
        var services = new ServiceCollection();
        services.AddMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly);
            configure?.Invoke(cfg);
        });

        _serviceProvider = services.BuildServiceProvider();
        return _serviceProvider.GetRequiredService<IMediator>();
    }

    [Test]
    public async Task Send_WithResponse_ReturnsHandlerResult()
    {
        // arrange
        var mediator = CreateMediator();

        // act
        var result = await mediator.Send(new TestRequest());

        // assert
        Assert.That(result, Is.EqualTo("test-response"));
    }

    [Test]
    public async Task Send_VoidRequest_CompletesSuccessfully()
    {
        // arrange
        var mediator = CreateMediator();

        // act & assert
        Assert.DoesNotThrowAsync(() => mediator.Send(new TestVoidRequest()));
    }

    [Test]
    public void Send_NullRequestWithResponse_ThrowsArgumentNullException()
    {
        // arrange
        var mediator = CreateMediator();

        // act & assert
        Assert.That(
            async () => await mediator.Send((IRequest<string>)null!),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Send_UnregisteredHandler_ThrowsInvalidOperationException()
    {
        // arrange
        var mediator = CreateMediator();

        // act & assert
        Assert.That(
            async () => await mediator.Send(new UnregisteredRequest()),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Send_UnregisteredVoidHandler_ThrowsInvalidOperationException()
    {
        // arrange
        var mediator = CreateMediator();

        // act & assert
        Assert.That(
            async () => await mediator.Send(new UnregisteredVoidRequest()),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Publish_NullNotification_ThrowsArgumentNullException()
    {
        // arrange
        var mediator = CreateMediator();

        // act & assert
        Assert.That(
            async () => await mediator.Publish((TestNotification)null!),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public async Task Publish_NotifiesAllHandlers()
    {
        // arrange
        var mediator = CreateMediator();

        // act
        await mediator.Publish(new TestNotification());

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(TestNotificationHandler.HandleCount, Is.EqualTo(1));
            Assert.That(SecondTestNotificationHandler.HandleCount, Is.EqualTo(1));
        }
    }

    [Test]
    public async Task Send_WithPipelineBehavior_ExecutesBehavior()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly);
            cfg.AddBehavior(
                typeof(IPipelineBehavior<,>),
                typeof(TestPipelineBehavior<,>));
        });
        var sp = services.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IMediator>();

        // act
        var result = await mediator.Send(new TestRequest());

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(TestPipelineBehavior<TestRequest, string>.WasCalled, Is.True);
            Assert.That(result, Is.EqualTo("test-response"));
        }
    }

    [Test]
    public async Task Send_CancellationTokenIsRespected()
    {
        // arrange
        var mediator = CreateMediator();
        using var cts = new CancellationTokenSource();

        // act
        var result = await mediator.Send(new TestRequest(), cts.Token);

        // assert
        Assert.That(result, Is.EqualTo("test-response"));
    }
}
