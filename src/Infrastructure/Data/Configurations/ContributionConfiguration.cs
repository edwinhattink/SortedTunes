using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Infrastructure.Data.Configurations;

public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
{
    public void Configure(EntityTypeBuilder<Contribution> builder)
    {
        builder.Property(c => c.ContributionType)
            .IsRequired();
    }
}
