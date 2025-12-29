namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class DiscSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Disc))
        {
            return new Disc()
            {
                Number = context.CreateInt(1, 10),
                Title = context.Create<string>(),
                AlbumId = context.Create<int>(),
                Album = null,
                DiscContributions = new List<DiscContribution>(),
                Tracks = new List<Track>()
            };
        }

        return new NoSpecimen();
    }
}
