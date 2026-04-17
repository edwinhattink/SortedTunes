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

        Assert.That(updatedArtist, Is.Not.Null);
        Assert.That(updatedArtist!.Name, Is.EqualTo(command.Name));
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

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
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

        // act & assert
        Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
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

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Name"));
        Assert.That(ex.Errors["Name"].Any(e => e.Error == "Name must be unique."));
    }

    [Test]
    public void ShouldFailWhenArtistNotFound()
    {
        // arrange
        var command = new UpdateArtistCommand
        {
            Id = 999, // Non-existent artist ID
            Name = "New Name"
        };

        // act & assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command));
    }
}
