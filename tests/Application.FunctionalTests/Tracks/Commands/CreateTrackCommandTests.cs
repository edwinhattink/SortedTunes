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

        track.Should().NotBeNull();
        track!.Title.Should().Be(command.Title);
        track.Number.Should().Be(command.Number);
        track.FileName.Should().Be(command.FileName);
        track.DiscId.Should().Be(command.DiscId);
        track.GenreId.Should().Be(command.GenreId);
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Track title must not exceed 200 characters.");
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Track title must not exceed 200 characters.");
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("File name must not exceed 200 characters.");
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("DiscId", "Disc with Id '999' does not exist.");
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("GenreId", "Genre with Id '999' does not exist.");
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

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Track number must be at least 1.");
    }
}
