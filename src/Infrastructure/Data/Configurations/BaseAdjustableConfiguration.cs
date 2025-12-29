using SortedTunes.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SortedTunes.Infrastructure.Data.Configurations;

public abstract class BaseAdjustableConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseAdjustableEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.ChangeByAmount)
               .HasPrecision(18, 3);

        builder.Property(e => e.ChangeByPercentage)
               .HasPrecision(18, 2);

        builder.Property(e => e.Waste)
               .HasPrecision(18, 2);

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
