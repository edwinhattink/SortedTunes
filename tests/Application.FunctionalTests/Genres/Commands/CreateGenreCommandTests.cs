using SortedTunes.Application.Genres.Commands.CreateGenre;

namespace SortedTunes.Application.FunctionalTests.Genres.Commands;

using static Testing;

public class CreateGenreCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateGenre()
    {
        // arrange
        var command = new CreateGenreCommand
        {
            Name = "New Genre"
        };

        // act
        var genreId = await SendAsync(command);

        // assert
        var genre = await FindAsync<Genre>(genreId);

        genre.Should().NotBeNull();
        genre!.Name.Should().Be(command.Name);
        genre.ParentGenreId.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateGenreWithParent()
    {
        // arrange
        var parentGenre = new Genre { Name = "Parent Genre" };
        await AddAsync(parentGenre);

        var command = new CreateGenreCommand
        {
            Name = "New Subgenre",
            ParentGenreId = parentGenre.Id
        };

        // act
        var genreId = await SendAsync(command);

        // assert
        var genre = await FindAsync<Genre>(genreId);

        genre.Should().NotBeNull();
        genre!.Name.Should().Be(command.Name);
        genre.ParentGenreId.Should().Be(parentGenre.Id);
    }

    [Test]
    public async Task ShouldRequireName()
    {
        // arrange
        var command = new CreateGenreCommand { Name = "" };

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
        var command = new CreateGenreCommand
        {
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
        var command = new CreateGenreCommand
        {
            Name = "New Genre",
            ParentGenreId = 999 // Non-existent parent genre ID
        };

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithErrorOnProperty("ParentGenreId", "Parent genre with Id 999 does not exist.");
    }
}
