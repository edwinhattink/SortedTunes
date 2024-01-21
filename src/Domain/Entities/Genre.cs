using System.Collections.Generic;

namespace Music.Domain.Entities;

public class Genre: BaseAuditableEntity
{
	public required string Name { get; set; }

	public int? ParentGenreId { get; set; }
	public Genre? ParentGenre { get; set; }

	public List<Track> Tracks { get; set; } = new List<Track>();

	public List<Genre> Genres { get; set; } = new List<Genre>();

}
