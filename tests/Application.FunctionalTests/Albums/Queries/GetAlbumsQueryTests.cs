using SortedTunes.Application.Albums.Queries.GetAlbums;

namespace SortedTunes.Application.FunctionalTests.Albums.Queries;

using static Testing;

public class GetAlbumsQueryTests : BaseTestFixture
{
    [Test]
    public async Task CanGetAlbums()
    {
        // arrange
        var albums = new List<Album>();
        for (int i = 0; i < 20; i++)
        {
            albums.Add(new Album()
            {
                Title = $"Album {i}",
                ReleaseYear = 2000 + i
            });
        }

        await AddRangeAsync(albums);

        var query = new GetAlbumsQuery();

        // act
        var result = await SendAsync(query);

        // assert
        result.TotalCount.Should().Be(20);
        result.Items.Should().HaveCount(10);
    }
}
