namespace SortedTunes.Domain.Entities;

public class Artist : BaseAuditableEntity
{
    public required string Name { get; set; }

    public List<Contribution> Contributions { get; set; } = [];
    public List<DiscContribution> DiscContributions { get; set; } = [];
}
