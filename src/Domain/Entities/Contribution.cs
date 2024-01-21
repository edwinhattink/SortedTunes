using Music.Domain.Enums;

namespace Music.Domain.Entities;

public class Contribution : BaseAuditableEntity
{
    public required int TrackId { get; set; }
    public Track? Track { get; set; }

    public required int ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public required ContributionType ContributionType { get; set; }
}
