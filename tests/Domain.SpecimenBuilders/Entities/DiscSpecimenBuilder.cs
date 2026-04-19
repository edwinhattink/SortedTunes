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
                Album = context.Create<Album>(),
                DiscContributions = [],
                Tracks = []
            };
        }

        return new NoSpecimen();
    }
}
