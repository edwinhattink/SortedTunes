namespace SortedTunes.Application.Common.Models;

public abstract record BaseAdjustableCommand
{
    public required decimal ChangeByAmount { get; set; }
    public required decimal ChangeByPercentage { get; set; }
    public required decimal Waste { get; set; }
}
