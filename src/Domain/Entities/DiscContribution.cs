namespace Music.Domain.Entities;

public class DiscContribution: BaseAuditableEntity
{

    public required int DiscId { get; set; }
    public Disc? Disc { get; set; }

    public required int ArtistId { get; set; }
    public Artist? Artist { get; set; }
}
