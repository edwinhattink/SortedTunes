using SortedTunes.Application.Albums.Commands.UpdateAlbum;

namespace SortedTunes.Application.FunctionalTests.Albums.Commands;

using static Testing;

public class UpdateAlbumCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateAlbum()
    {
        // arrange
        var album = new Album { Title = "Original Title" };
        await AddAsync(album);

        var command = new UpdateAlbumCommand
        {
            Id = album.Id,
            Title = "Updated Title"
        };

        // act
        await SendAsync(command);

        // assert
        var updatedAlbum = await FindAsync<Album>(album.Id);

        Assert.That(updatedAlbum, Is.Not.Null);
        Assert.That(updatedAlbum!.Title, Is.EqualTo(command.Title));
    }

    [Test]
    public async Task ShouldRequireTitle()
    {
        // arrange
        var album = new Album { Title = "Original Title" };
        await AddAsync(album);

        var command = new UpdateAlbumCommand
        {
            Id = album.Id,
            Title = ""
        };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
    }

    [Test]
    public async Task ShouldFailWhenTitleIsTooLong()
    {
        // arrange
        var album = new Album { Title = "Original Title" };
        await AddAsync(album);

        var command = new UpdateAlbumCommand
        {
            Id = album.Id,
            Title = new string('A', 201) // 201 characters long
        };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
    }

    [Test]
    public async Task ShouldFailWhenTitleIsNotUnique()
    {
        // arrange
        var album1 = new Album { Title = "Album 1" };
        var album2 = new Album { Title = "Album 2" };
        await AddAsync(album1);
        await AddAsync(album2);

        var command = new UpdateAlbumCommand
        {
            Id = album1.Id,
            Title = "Album 2"
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Title must be unique."));
    }

    [Test]
    public void ShouldFailWhenAlbumNotFound()
    {
        // arrange
        var command = new UpdateAlbumCommand
        {
            Id = 999, // Non-existent album ID
            Title = "New Title"
        };

        // act & assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command));
    }
}
