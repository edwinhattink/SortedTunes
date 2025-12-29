namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class ArtistSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Artist))
        {
            return new Artist()
            {
                Name = context.Create<string>(),
                Contributions = new List<Contribution>(),
                DiscContributions = new List<DiscContribution>()
            };
        }

        return new NoSpecimen();
    }
}
