using SortedTunes.Application.Artists.Commands.CreateArtist;

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

        artist.Should().NotBeNull();
        artist!.Name.Should().Be(command.Name);
    }

    [Test]
    public async Task ShouldRequireName()
    {
        // arrange
        var command = new CreateArtistCommand() { Name = "" };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldFailWhenNameIsTooLong()
    {
        // arrange
        var command = new CreateArtistCommand
        {
            Name = new string('A', 201) // 201 characters long
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Name", "Name must be unique.");
    }
}
