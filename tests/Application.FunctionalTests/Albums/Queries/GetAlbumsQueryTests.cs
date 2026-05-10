using SortedTunes.Application.Albums.Queries;

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
        Assert.That(result.TotalCount, Is.EqualTo(20));
        Assert.That(result.Items, Has.Count.EqualTo(10));
    }
}
