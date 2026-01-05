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

        Assert.That(album, Is.Not.Null);
        Assert.That(album!.Title, Is.EqualTo(command.Title));
    }

    [Test]
    public void ShouldRequireTitle()
    {
        // arrange
        var command = new CreateAlbumCommand() { Title = "" };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
    }

    [Test]
    public void ShouldFailWhenTitleIsTooLong()
    {
        // arrange
        var command = new CreateAlbumCommand
        {
            Title = new string('A', 201) // 201 characters long
        };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
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

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Title must be unique."));
    }
}
