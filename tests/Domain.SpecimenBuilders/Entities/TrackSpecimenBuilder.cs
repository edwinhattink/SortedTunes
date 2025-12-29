namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class TrackSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Track))
        {
            return new Track()
            {
                Number = context.CreateInt(1, 50),
                Title = context.Create<string>(),
                FileName = context.Create<string>(),
                DiscId = context.Create<int>(),
                Disc = null,
                GenreId = context.Create<int>(),
                Genre = null,
                Contributions = new List<Contribution>()
            };
        }

        return new NoSpecimen();
    }
}
