using SortedTunes.Application.Genres.Commands.DeleteGenre;

namespace SortedTunes.Application.FunctionalTests.Genres.Commands;

using static Testing;

public class DeleteGenreCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteGenre()
    {
        // arrange
        var genre = new Genre { Name = "Genre to Delete" };
        await AddAsync(genre);

        var command = new DeleteGenreCommand(genre.Id);

        // act
        await SendAsync(command);

        // assert
        var deletedGenre = await FindAsync<Genre>(genre.Id);
        deletedGenre.Should().BeNull();
    }

    [Test]
    public async Task ShouldFailWhenGenreNotFound()
    {
        // arrange
        var command = new DeleteGenreCommand(999); // Non-existent genre ID

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldFailWhenDeletingAlreadyDeletedGenre()
    {
        // arrange
        var genre = new Genre { Name = "Genre to Delete Twice" };
        await AddAsync(genre);

        var command = new DeleteGenreCommand(genre.Id);

        // act
        await SendAsync(command); // First deletion

        Func<Task> action = async () => await SendAsync(command); // Second deletion attempt

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
