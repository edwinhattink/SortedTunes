using SortedTunes.Application.Artists.Queries.GetArtists;

namespace SortedTunes.Application.FunctionalTests.Artists.Queries;

using static Testing;

public class GetArtistsQueryTests : BaseTestFixture
{
    [Test]
    public async Task CanGetArtists()
    {
        // arrange
        var artists = new List<Artist>();
        for (int i = 0; i < 20; i++)
        {
            artists.Add(new Artist()
            {
                Name = $"Artist {i}"
            });
        }

        await AddRangeAsync(artists);

        var query = new GetArtistsQuery();

        // act
        var result = await SendAsync(query);

        // assert
        Assert.That(result.TotalCount, Is.EqualTo(20));
        Assert.That(result.Items, Has.Count.EqualTo(10));
    }
}
