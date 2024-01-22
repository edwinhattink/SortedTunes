namespace SortedTunes.Domain.Entities;

public class Contribution : BaseAuditableEntity
{
    public int TrackId { get; set; }
    public Track? Track { get; set; }

    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public required ContributionType ContributionType { get; set; }
}
