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
                TrackId = context.Create<int>(),
                Track = null,
                ArtistId = context.Create<int>(),
                Artist = null,
                ContributionType = context.Create<ContributionType>()
            };
        }

        return new NoSpecimen();
    }
}
