using SortedTunes.Application.Genres.Commands.Create;

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

        Assert.That(genre, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(genre!.Name, Is.EqualTo(command.Name));
            Assert.That(genre.ParentGenreId, Is.Null);
        }
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

        Assert.That(genre, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(genre!.Name, Is.EqualTo(command.Name));
            Assert.That(genre.ParentGenreId, Is.EqualTo(parentGenre.Id));
        }
    }

    [Test]
    public void ShouldRequireName()
    {
        // arrange
        var command = new CreateGenreCommand { Name = "" };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Name"));
        Assert.That(ex.Errors["Name"].Any(e => e.Error == "Name must not be empty."));
    }

    [Test]
    public void ShouldFailWhenNameIsTooLong()
    {
        // arrange
        var command = new CreateGenreCommand
        {
            Name = new string('A', 201) // 201 characters long
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Genre name must not exceed 200 characters."));
    }

    [Test]
    public void ShouldFailWhenParentGenreDoesNotExist()
    {
        // arrange
        var command = new CreateGenreCommand
        {
            Name = "New Genre",
            ParentGenreId = 999 // Non-existent parent genre ID
        };

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex?.Message, Does.Contain("Parent genre with Id 999 does not exist."));
    }
}
