using SortedTunes.Application.Genres.Commands.UpdateGenre;

namespace SortedTunes.Application.FunctionalTests.Genres.Commands;

using static Testing;

public class UpdateGenreCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateGenre()
    {
        // arrange
        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(genre);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = "Updated Genre"
        };

        // act
        await SendAsync(command);

        // assert
        var updatedGenre = await FindAsync<Genre>(genre.Id);

        updatedGenre.Should().NotBeNull();
        updatedGenre!.Name.Should().Be(command.Name);
        updatedGenre.ParentGenreId.Should().BeNull();
    }

    [Test]
    public async Task ShouldUpdateGenreWithParent()
    {
        // arrange
        var parentGenre = new Genre { Name = "Parent Genre" };
        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(parentGenre);
        await AddAsync(genre);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = "Updated Genre",
            ParentGenreId = parentGenre.Id
        };

        // act
        await SendAsync(command);

        // assert
        var updatedGenre = await FindAsync<Genre>(genre.Id);

        updatedGenre.Should().NotBeNull();
        updatedGenre!.Name.Should().Be(command.Name);
        updatedGenre.ParentGenreId.Should().Be(parentGenre.Id);
    }

    [Test]
    public async Task ShouldRequireName()
    {
        // arrange
        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(genre);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = ""
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Name", "'Name' must not be empty.");

    }

    [Test]
    public async Task ShouldFailWhenNameIsTooLong()
    {
        // arrange
        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(genre);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = new string('A', 201) // 201 characters long
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("Name", "Genre name must not exceed 200 characters.");
    }

    [Test]
    public async Task ShouldFailWhenParentGenreDoesNotExist()
    {
        // arrange
        var genre = new Genre { Name = "Original Genre" };
        await AddAsync(genre);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = "Updated Genre",
            ParentGenreId = 999 // Non-existent parent genre ID
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("ParentGenreId", "Parent genre with Id '999' does not exist.");
    }

    [Test]
    public async Task ShouldFailWhenGenreNotFound()
    {
        // arrange
        var command = new UpdateGenreCommand
        {
            Id = 999, // Non-existent genre ID
            Name = "New Genre"
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
