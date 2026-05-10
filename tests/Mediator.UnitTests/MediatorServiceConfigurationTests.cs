using System.Reflection;

namespace SortedTunes.Mediator.UnitTests;

[TestFixture]
public class MediatorServiceConfigurationTests
{
    [Test]
    public void RegisterServicesFromAssembly_AddsAssembly()
    {
        // arrange
        var config = new MediatorServiceConfiguration();
        var assembly = Assembly.GetExecutingAssembly();

        // act
        var result = config.RegisterServicesFromAssembly(assembly);

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(result, Is.SameAs(config), "should return the same configuration instance for chaining");
            Assert.That(config.AssembliesToRegister, Has.Count.EqualTo(1));
            Assert.That(config.AssembliesToRegister[0], Is.EqualTo(assembly));
        }
    }

    [Test]
    public void RegisterServicesFromAssembly_SupportsMultipleAssemblies()
    {
        // arrange
        var config = new MediatorServiceConfiguration();
        var assembly1 = Assembly.GetExecutingAssembly();
        var assembly2 = typeof(Mediator).Assembly;

        // act
        config.RegisterServicesFromAssembly(assembly1)
              .RegisterServicesFromAssembly(assembly2);

        // assert
        Assert.That(config.AssembliesToRegister, Has.Count.EqualTo(2));
    }

    [Test]
    public void AddBehavior_RegistersBehavior()
    {
        // arrange
        var config = new MediatorServiceConfiguration();
        var serviceType = typeof(IPipelineBehavior<,>);
        var implType = typeof(object);

        // act
        var result = config.AddBehavior(serviceType, implType);

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(result, Is.SameAs(config), "should return the same configuration instance for chaining");
            Assert.That(config.Behaviors, Has.Count.EqualTo(1));
            Assert.That(config.Behaviors[0].ServiceType, Is.EqualTo(serviceType));
            Assert.That(config.Behaviors[0].ImplementationType, Is.EqualTo(implType));
        }
    }

    [Test]
    public void NewConfiguration_HasEmptyCollections()
    {
        // arrange & act
        var config = new MediatorServiceConfiguration();

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(config.AssembliesToRegister, Is.Empty);
            Assert.That(config.Behaviors, Is.Empty);
        }
    }
}
