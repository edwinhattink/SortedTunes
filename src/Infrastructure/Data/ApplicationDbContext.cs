using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Contribution> Contributions => Set<Contribution>();
    public DbSet<Disc> Discs => Set<Disc>();
    public DbSet<DiscContribution> DiscContributions => Set<DiscContribution>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Track> Tracks => Set<Track>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Set default query splitting behavior for all queries in this context
        optionsBuilder.UseSqlServer(options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    }
}
