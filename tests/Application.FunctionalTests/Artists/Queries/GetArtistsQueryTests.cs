using SortedTunes.Application.Artists.Queries.GetArtists;
using SortedTunes.Domain.Entities;

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
        result.TotalCount.Should().Be(20);
        result.Items.Should().HaveCount(10);
    }
}
