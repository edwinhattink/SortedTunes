namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class DiscContributionSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(DiscContribution))
        {
            return new DiscContribution()
            {
                DiscId = context.Create<int>(),
                Disc = null,
                ArtistId = context.Create<int>(),
                Artist = null
            };
        }

        return new NoSpecimen();
    }
}
