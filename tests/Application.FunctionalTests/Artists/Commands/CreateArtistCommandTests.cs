using SortedTunes.Application.Artists.Commands.Create;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class CreateArtistCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateArtist()
    {
        // arrange
        var command = new CreateArtistCommand
        {
            Name = "New Artist"
        };

        // act
        var artistId = await SendAsync(command);

        // assert
        var artist = await FindAsync<Artist>(artistId);

        Assert.That(artist, Is.Not.Null);
        Assert.That(artist!.Name, Is.EqualTo(command.Name));
    }

    [Test]
    public void ShouldRequireName()
    {
        // arrange
        var command = new CreateArtistCommand() { Name = "" };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
    }

    [Test]
    public void ShouldFailWhenNameIsTooLong()
    {
        // arrange
        var command = new CreateArtistCommand
        {
            Name = new string('A', 201) // 201 characters long
        };

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
    }

    [Test]
    public async Task ShouldFailWhenNameIsNotUnique()
    {
        // arrange
        var existingArtist = new Artist { Name = "Existing Artist" };
        await AddAsync(existingArtist);

        var command = new CreateArtistCommand
        {
            Name = "Existing Artist"
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Name must be unique."));
    }
}
