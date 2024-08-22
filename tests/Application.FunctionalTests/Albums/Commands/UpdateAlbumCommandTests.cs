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

        updatedAlbum.Should().NotBeNull();
        updatedAlbum!.Title.Should().Be(command.Title);
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Title", "Title must be unique.");
    }

    [Test]
    public async Task ShouldFailWhenAlbumNotFound()
    {
        // arrange
        var command = new UpdateAlbumCommand
        {
            Id = 999, // Non-existent album ID
            Title = "New Title"
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
