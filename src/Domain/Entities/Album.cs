using System.Collections.Generic;

namespace Music.Domain.Entities;

public class Album: BaseAuditableEntity
{
	public required string Name { get; set; }

	public int ReleaseYear { get; set; }
	public string? Image { get; set; }

	public List<Disc> Discs { get; set; } = new List<Disc>();
}
