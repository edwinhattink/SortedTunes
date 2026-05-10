namespace SortedTunes.Domain.Common;

public abstract class BaseAdjustableEntity : BaseAuditableEntity
{
    public required decimal ChangeByAmount { get; set; }
    public required decimal ChangeByPercentage { get; set; }
    public required decimal Waste { get; set; }
}
