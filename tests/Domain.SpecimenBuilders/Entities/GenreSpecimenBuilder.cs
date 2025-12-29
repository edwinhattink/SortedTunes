namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class GenreSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Genre))
        {
            return new Genre()
            {
                Name = context.Create<string>(),
                ParentGenreId = null,
                ParentGenre = null,
                Tracks = new List<Track>(),
                Genres = new List<Genre>()
            };
        }

        return new NoSpecimen();
    }
}
