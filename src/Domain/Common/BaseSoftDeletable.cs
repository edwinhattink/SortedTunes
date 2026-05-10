using System.ComponentModel.DataAnnotations.Schema;

namespace SortedTunes.Domain.Common;

public abstract class BaseSoftDeletable : BaseAuditableEntity
{
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public void UndoDelete()
    {
        DeletedBy = null;
        DeletedAt = null;
    }

    [NotMapped]
    public bool IsPermanentlyDeleted { get; set; } = false;
}
