using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Infrastructure.Data.Configurations;

public class DiscContributionConfiguration : IEntityTypeConfiguration<DiscContribution>
{
    public void Configure(EntityTypeBuilder<DiscContribution> builder)
    {
    }
}
