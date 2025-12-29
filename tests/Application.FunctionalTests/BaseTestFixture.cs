using Microsoft.Extensions.Time.Testing;

namespace SortedTunes.Application.FunctionalTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        TimeProviderInstance = new FakeTimeProvider();
        await ResetState();
        AuthTesting.ResetState();
    }
}
