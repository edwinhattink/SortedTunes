using SortedTunes.Application.Albums.Commands.Delete;

namespace SortedTunes.Application.FunctionalTests.Albums.Commands;

using static Testing;

public class DeleteAlbumCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteAlbum()
    {
        // arrange
        var album = new Album { Title = "Album to Delete" };
        await AddAsync(album);

        var command = new DeleteAlbumCommand(album.Id);

        // act
        await SendAsync(command);

        // assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await FindAsync<Album>(album.Id));
    }

    [Test]
    public void ShouldFailWhenAlbumNotFound()
    {
        // arrange
        var command = new DeleteAlbumCommand(999); // Non-existent album ID

        // act & assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command));
    }

    [Test]
    public async Task ShouldFailWhenDeletingAlreadyDeletedAlbum()
    {
        // arrange
        var album = new Album { Title = "Album to Delete Twice" };
        await AddAsync(album);

        var command = new DeleteAlbumCommand(album.Id);

        // act
        await SendAsync(command); // First deletion

        // assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command)); // Second deletion attempt
    }
}
