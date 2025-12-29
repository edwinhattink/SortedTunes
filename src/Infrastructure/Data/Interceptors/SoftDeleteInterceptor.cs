using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SortedTunes.Domain.Common;

namespace SortedTunes.Infrastructure.Data.Interceptors;

public class SoftDeleteInterceptor(TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SoftDeleteEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SoftDeleteEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void SoftDeleteEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: BaseSoftDeletable delete } || delete.IsPermanentlyDeleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;

            //TODO:
            delete.DeletedBy = "Anonymous";
            delete.DeletedAt = dateTime.GetUtcNow();
        }

        return;
    }
}
