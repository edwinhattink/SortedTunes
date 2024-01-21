using System.Collections.Generic;

namespace Music.Domain.Entities;

public class Disc: BaseAuditableEntity
{
	public required int Number { get; set; }
	public string? Name { get; set; }

    public required int AlbumId { get; set; }
	public Album? Album { get; set; }

	public List<DiscContribution> DiscContributions { get; set; } = new List<DiscContribution>();

	public List<Track> Tracks { get; set; } = new List<Track>();
}
