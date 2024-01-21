namespace SortedTunes.Domain.Entities;

public class DiscContribution : BaseAuditableEntity
{

    public int? DiscId { get; set; }
    public Disc? Disc { get; set; }

    public int? ArtistId { get; set; }
    public Artist? Artist { get; set; }
}
