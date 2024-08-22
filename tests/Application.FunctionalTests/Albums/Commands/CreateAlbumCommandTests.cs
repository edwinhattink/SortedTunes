using SortedTunes.Application.Albums.Commands.CreateAlbum;

namespace SortedTunes.Application.FunctionalTests.Albums.Commands;

using static Testing;

public class CreateAlbumCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateAlbum()
    {
        // arrange
        var command = new CreateAlbumCommand
        {
            Title = "New Album"
        };

        // act
        var albumId = await SendAsync(command);

        // assert
        var album = await FindAsync<Album>(albumId);

        album.Should().NotBeNull();
        album!.Title.Should().Be(command.Title);
    }

    [Test]
    public async Task ShouldRequireTitle()
    {
        // arrange
        var command = new CreateAlbumCommand() { Title = "" };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldFailWhenTitleIsTooLong()
    {
        // arrange
        var command = new CreateAlbumCommand
        {
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
        var existingAlbum = new Album { Title = "Existing Album" };
        await AddAsync(existingAlbum);

        var command = new CreateAlbumCommand
        {
            Title = "Existing Album"
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Title", "Title must be unique.");
    }
}
