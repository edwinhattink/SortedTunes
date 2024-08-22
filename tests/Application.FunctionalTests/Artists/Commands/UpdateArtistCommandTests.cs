using SortedTunes.Application.Artists.Commands.UpdateArtist;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class UpdateArtistCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateArtist()
    {
        // arrange
        var artist = new Artist { Name = "Original Name" };
        await AddAsync(artist);

        var command = new UpdateArtistCommand
        {
            Id = artist.Id,
            Name = "Updated Name"
        };

        // act
        await SendAsync(command);

        // assert
        var updatedArtist = await FindAsync<Artist>(artist.Id);

        updatedArtist.Should().NotBeNull();
        updatedArtist!.Name.Should().Be(command.Name);
    }

    [Test]
    public async Task ShouldRequireName()
    {
        // arrange
        var artist = new Artist { Name = "Original Name" };
        await AddAsync(artist);

        var command = new UpdateArtistCommand
        {
            Id = artist.Id,
            Name = ""
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldFailWhenNameIsTooLong()
    {
        // arrange
        var artist = new Artist { Name = "Original Name" };
        await AddAsync(artist);

        var command = new UpdateArtistCommand
        {
            Id = artist.Id,
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
        var artist1 = new Artist { Name = "Artist 1" };
        var artist2 = new Artist { Name = "Artist 2" };
        await AddAsync(artist1);
        await AddAsync(artist2);

        var command = new UpdateArtistCommand
        {
            Id = artist1.Id,
            Name = "Artist 2"
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Name", "Name must be unique.");
    }

    [Test]
    public async Task ShouldFailWhenArtistNotFound()
    {
        // arrange
        var command = new UpdateArtistCommand
        {
            Id = 999, // Non-existent artist ID
            Name = "New Name"
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
