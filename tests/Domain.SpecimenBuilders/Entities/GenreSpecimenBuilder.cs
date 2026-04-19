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
                Tracks = [],
                Genres = []
            };
        }

        return new NoSpecimen();
    }
}
