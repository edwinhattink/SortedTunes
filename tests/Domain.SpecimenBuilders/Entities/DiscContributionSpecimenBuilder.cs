namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class DiscContributionSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(DiscContribution))
        {
            return new DiscContribution()
            {
                Disc = context.Create<Disc>(),
                Artist = context.Create<Artist>()
            };
        }

        return new NoSpecimen();
    }
}
