namespace SortedTunes.Application.FunctionalTests.Artists.Queries;

using SortedTunes.Application.Artists.Queries.GetArtists;
using SortedTunes.Domain.Entities;
using static Testing;

public class GetArtistsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllArtists()
    {
        await RunAsDefaultUserAsync();

        // Assuming Artist entity doesn't have complex nested structures like TodoList
        await AddAsync(new Artist { Name = "Artist One" });
        await AddAsync(new Artist { Name = "Artist Two" });
        await AddAsync(new Artist { Name = "Artist Three" });

        var query = new GetArtistsQuery();

        var result = await SendAsync(query);

        result.Items.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetArtistsQuery();

        var action = () => SendAsync(query);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
