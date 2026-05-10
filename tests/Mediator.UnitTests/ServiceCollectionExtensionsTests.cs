using Microsoft.Extensions.DependencyInjection;
using SortedTunes.Mediator.UnitTests.TestHelpers;

namespace SortedTunes.Mediator.UnitTests;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddMediator_RegistersIMediatorAsSender()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var sender = sp.GetService<ISender>();
        Assert.That(sender, Is.Not.Null);
        Assert.That(sender, Is.InstanceOf<Mediator>());
    }

    [Test]
    public void AddMediator_RegistersIMediatorAsPublisher()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var publisher = sp.GetService<IPublisher>();
        Assert.That(publisher, Is.Not.Null);
        Assert.That(publisher, Is.InstanceOf<Mediator>());
    }

    [Test]
    public void AddMediator_RegistersIMediator()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var mediator = sp.GetService<IMediator>();
        Assert.That(mediator, Is.Not.Null);
        Assert.That(mediator, Is.InstanceOf<Mediator>());
    }

    [Test]
    public void AddMediator_RegistersRequestHandlers()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var handler = sp.GetService<IRequestHandler<TestRequest, string>>();
        Assert.That(handler, Is.Not.Null);
        Assert.That(handler, Is.InstanceOf<TestRequestHandler>());
    }

    [Test]
    public void AddMediator_RegistersVoidRequestHandlers()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var handler = sp.GetService<IRequestHandler<TestVoidRequest>>();
        Assert.That(handler, Is.Not.Null);
        Assert.That(handler, Is.InstanceOf<TestVoidRequestHandler>());
    }

    [Test]
    public void AddMediator_RegistersNotificationHandlers()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly));
        var sp = services.BuildServiceProvider();

        // assert
        var handlers = sp.GetServices<INotificationHandler<TestNotification>>().ToList();
        Assert.That(handlers, Has.Count.EqualTo(2));
    }

    [Test]
    public void AddMediator_RegistersBehaviors()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TestRequest).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TestPipelineBehavior<,>));
        });
        var sp = services.BuildServiceProvider();

        // assert
        var behaviors = sp.GetServices<IPipelineBehavior<TestRequest, string>>().ToList();
        Assert.That(behaviors, Has.Count.EqualTo(1));
        Assert.That(behaviors[0], Is.InstanceOf<TestPipelineBehavior<TestRequest, string>>());
    }
}
