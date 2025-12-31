using SortedTunes.Application.Albums.Commands.DeleteAlbum;

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
        var deletedAlbum = await FindAsync<Album>(album.Id);
        deletedAlbum.Should().BeNull();
    }

    [Test]
    public async Task ShouldFailWhenAlbumNotFound()
    {
        // arrange
        var command = new DeleteAlbumCommand(999); // Non-existent album ID

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
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

        Func<Task> action = async () => await SendAsync(command); // Second deletion attempt

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
