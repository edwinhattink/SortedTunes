using SortedTunes.Domain.Enums;

namespace SortedTunes.Domain.SpecimenBuilders.Entities;

public class ContributionSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Contribution))
        {
            return new Contribution()
            {
                Track = context.Create<Track>(),
                Artist = context.Create<Artist>(),
                ContributionType = context.Create<ContributionType>()
            };
        }

        return new NoSpecimen();
    }
}
