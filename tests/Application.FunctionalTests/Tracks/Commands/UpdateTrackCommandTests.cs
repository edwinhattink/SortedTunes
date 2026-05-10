using SortedTunes.Application.Tracks.Commands.Update;
using SortedTunes.Domain.Common;

namespace SortedTunes.Application.FunctionalTests.Tracks.Commands;

using static Testing;

public class UpdateTrackCommandTests : BaseTestFixture
{
    [Test, ApplicationAutoData]
    public async Task ShouldUpdateTrack(Genre genre, Disc disc)
    {
        // arrange
        await AddAsync(genre);
        await AddAsync(disc);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 2,
            Title = "Updated Track",
            FileName = "updated.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act
        await SendAsync(command);

        // assert
        var updatedTrack = await FindAsync<Track>(track.Id);

        Assert.That(updatedTrack, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(updatedTrack!.Title, Is.EqualTo(command.Title));
            Assert.That(updatedTrack.Number, Is.EqualTo(command.Number));
            Assert.That(updatedTrack.FileName, Is.EqualTo(command.FileName));
            Assert.That(updatedTrack.DiscId, Is.EqualTo(command.DiscId));
            Assert.That(updatedTrack.GenreId, Is.EqualTo(command.GenreId));
        }
    }

    [Test, ApplicationAutoData]
    public async Task ShouldRequireTitle(Genre genre, Disc disc)
    {
        // arrange
        await AddAsync(genre);
        await AddAsync(disc);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 1,
            Title = "",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Title"));
        Assert.That(ex.Errors["Title"].Any(e => e.Error == "'Title' must not be empty."));
    }

    [Test, ApplicationAutoData]
    public async Task ShouldFailWhenTitleIsTooLong(Genre genre, Disc disc)
    {
        // arrange
        await AddAsync(genre);
        await AddAsync(disc);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 1,
            Title = new string('A', 201), // 201 characters long
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Title"));
        Assert.That(ex.Errors["Title"].Any(e => e.Error == "Track title must not exceed 200 characters."));
    }

    [Test, ApplicationAutoData]
    public async Task ShouldFailWhenFileNameIsTooLong(Genre genre, Disc disc)
    {
        // arrange
        await AddAsync(genre);
        await AddAsync(disc);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 1,
            Title = "Original Track",
            FileName = new string('A', 201), // 201 characters long
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("FileName"));
        Assert.That(ex.Errors["FileName"].Any(e => e.Error == "File name must not exceed 200 characters."));
    }

    [Test, ApplicationAutoData]
    public async Task ShouldFailWhenDiscDoesNotExist(Genre genre, Disc disc)
    {
        // arrange
        await AddAsync(genre);
        await AddAsync(disc);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = 999, // Non-existent disc ID
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("DiscId"));
        Assert.That(ex.Errors["DiscId"].Any(e => e.Error == "Disc with Id 999 does not exist."));
    }

    [Test, ApplicationAutoData]
    public async Task ShouldFailWhenGenreDoesNotExist(Disc disc)
    {
        // arrange
        await AddAsync(disc);

        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(genre);

        var track = new Track
        {
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };
        await AddAsync(track);

        var command = new UpdateTrackCommand
        {
            Id = track.Id,
            Number = 1,
            Title = "Original Track",
            FileName = "original.mp3",
            DiscId = disc.Id,
            GenreId = 999 // Non-existent genre ID
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));

        Assert.That(ex.Errors, Does.ContainKey("GenreId"));
        Assert.That(ex.Errors["GenreId"].Any(e => e.Error == "Genre with Id 999 does not exist."));
    }

    [Test, ApplicationAutoData]
    public async Task ShouldFailWhenTrackNotFound(Disc disc, Genre genre)
    {
        await AddRangeAsync<BaseEntity>([disc, genre]);

        // arrange
        var command = new UpdateTrackCommand
        {
            Id = 999, // Non-existent track ID
            Number = 1,
            Title = "Updated Track",
            FileName = "updated.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command));
    }
}
