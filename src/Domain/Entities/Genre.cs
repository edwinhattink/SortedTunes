namespace SortedTunes.Domain.Entities;

public class Genre : BaseAuditableEntity
{
    public required string Name { get; set; }

    public int? ParentGenreId { get; set; }
    public Genre? ParentGenre { get; set; }

    public List<Track> Tracks { get; set; } = [];

    public List<Genre> Genres { get; set; } = [];

}
