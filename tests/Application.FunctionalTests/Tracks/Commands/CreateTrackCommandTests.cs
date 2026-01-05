using SortedTunes.Application.Tracks.Commands.CreateTrack;

namespace SortedTunes.Application.FunctionalTests.Tracks.Commands;

using static Testing;

public class CreateTrackCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateTrack()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(genre);
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = "New Track",
            FileName = "track.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act
        var trackId = await SendAsync(command);

        // assert
        var track = await FindAsync<Track>(trackId);

        Assert.That(track, Is.Not.Null);
        Assert.That(track!.Title, Is.EqualTo(command.Title));
        Assert.That(track.Number, Is.EqualTo(command.Number));
        Assert.That(track.FileName, Is.EqualTo(command.FileName));
        Assert.That(track.DiscId, Is.EqualTo(command.DiscId));
        Assert.That(track.GenreId, Is.EqualTo(command.GenreId));
    }

    [Test]
    public async Task ShouldRequireTitle()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(genre);
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = "",
            FileName = "track.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Track title must not exceed 200 characters."));
    }

    [Test]
    public async Task ShouldFailWhenTitleIsTooLong()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(genre);
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = new string('A', 201), // 201 characters long
            FileName = "track.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Track title must not exceed 200 characters."));
    }

    [Test]
    public async Task ShouldFailWhenFileNameIsTooLong()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(genre);
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = "New Track",
            FileName = new string('A', 201), // 201 characters long
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("File name must not exceed 200 characters."));
    }

    [Test]
    public async Task ShouldFailWhenDiscDoesNotExist()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        await AddAsync(genre);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = "New Track",
            FileName = "track.mp3",
            DiscId = 999, // Non-existent disc ID
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Disc with Id '999' does not exist."));
    }

    [Test]
    public async Task ShouldFailWhenGenreDoesNotExist()
    {
        // arrange
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 1,
            Title = "New Track",
            FileName = "track.mp3",
            DiscId = disc.Id,
            GenreId = 999 // Non-existent genre ID
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Genre with Id '999' does not exist."));
    }

    [Test]
    public async Task ShouldFailWhenNumberIsLessThanOne()
    {
        // arrange
        var genre = new Genre { Name = "Genre" };
        var disc = new Disc { Title = "Disc", Number = 1 };
        await AddAsync(genre);
        await AddAsync(disc);

        var command = new CreateTrackCommand
        {
            Number = 0, // Invalid track number
            Title = "New Track",
            FileName = "track.mp3",
            DiscId = disc.Id,
            GenreId = genre.Id
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Track number must be at least 1."));
    }
}
