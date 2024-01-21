﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Infrastructure.Data.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.FileName)
            .HasMaxLength(200);

        builder.HasOne(t => t.Disc)
           .WithMany()
           .HasForeignKey(t => t.DiscId)
           .IsRequired();

        builder.HasOne(t => t.Genre)
            .WithMany()
            .HasForeignKey(t => t.GenreId)
            .IsRequired();
    }
}
