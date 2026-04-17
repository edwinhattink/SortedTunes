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

        Assert.That(updatedGenre, Is.Not.Null);
        Assert.That(updatedGenre!.Name, Is.EqualTo(command.Name));
        Assert.That(updatedGenre.ParentGenreId, Is.Null);
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

        Assert.That(updatedGenre, Is.Not.Null);
        Assert.That(updatedGenre!.Name, Is.EqualTo(command.Name));
        Assert.That(updatedGenre.ParentGenreId, Is.EqualTo(parentGenre.Id));
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

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Name"));
        Assert.That(ex.Errors["Name"].Any(e => e.Error == "Name must not be empty."));
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

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("Name"));
        Assert.That(ex.Errors["Name"].Any(e => e.Error == "Genre name must not exceed 200 characters."));
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

        // act & assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await SendAsync(command));
        Assert.That(ex.Errors, Does.ContainKey("ParentGenreId"));
        Assert.That(ex.Errors["ParentGenreId"].Any(e => e.Error == "Parent genre with Id '999' does not exist."));
    }

    [Test]
    public void ShouldFailWhenGenreNotFound()
    {
        // arrange
        var command = new UpdateGenreCommand
        {
            Id = 999, // Non-existent genre ID
            Name = "New Genre"
        };

        // act & assert
        Assert.ThrowsAsync<NotFoundException>(async () => await SendAsync(command));
    }
}
