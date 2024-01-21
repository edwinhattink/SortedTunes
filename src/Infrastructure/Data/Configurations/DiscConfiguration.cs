using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Infrastructure.Data.Configurations;

public class DiscConfiguration : IEntityTypeConfiguration<Disc>
{
    public void Configure(EntityTypeBuilder<Disc> builder)
    {
        builder.Property(d => d.Number)
            .IsRequired();

        builder.Property(d => d.Title)
            .HasMaxLength(200);
    }
}
