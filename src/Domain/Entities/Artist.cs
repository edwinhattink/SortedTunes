using System.Collections.Generic;

namespace Music.Domain.Entities;

public class Artist: BaseAuditableEntity
{
	public required string Name { get; set; }

	public List<Contribution> Contributions { get; set; } = new List<Contribution>();

	public List<DiscContribution> DiscContributions { get; set; } = new List<DiscContribution>();
}
