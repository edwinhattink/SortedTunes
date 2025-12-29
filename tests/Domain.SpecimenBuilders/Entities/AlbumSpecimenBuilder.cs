namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class AlbumSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Album))
        {
            return new Album()
            {
                Title = context.Create<string>(),
                ReleaseYear = context.CreateInt(1900, 2024),
                Image = context.Create<string>(),
                Discs = new List<Disc>()
            };
        }

        return new NoSpecimen();
    }
}
