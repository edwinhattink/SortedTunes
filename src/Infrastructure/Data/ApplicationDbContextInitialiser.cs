using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SortedTunes.Domain.Constants;
using SortedTunes.Domain.Entities;
using SortedTunes.Domain.Enums;
using SortedTunes.Infrastructure.Identity;

namespace SortedTunes.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Look for any albums.
        if (_context.Albums.Any())
        {
            return; // DB has been seeded
        }

        var theFirstDose = new Album { Title = "The First Dose", ReleaseYear = 2020 };
        _context.AddRange(theFirstDose);
        await _context.SaveChangesAsync();

        var rebelion = new Artist { Name = "Rebelion" };
        var artists = new Artist[]
        {
                rebelion,
                new Artist{Name="Radical Redemption"},
        };
        _context.AddRange(artists);

        var hardstyle = new Genre { Name = "Hardstyle" };
        var rawHardstyle = new Genre { Name = "Raw Hardstyle", ParentGenre = hardstyle };
        var euphoricHardstyle = new Genre { Name = "Euphoric Hardstyle", ParentGenre = hardstyle };
        _context.Genres.Add(rawHardstyle);
        _context.Genres.Add(euphoricHardstyle);
        await _context.SaveChangesAsync();

        var theFirstDoseDiscOne = new Disc { Album = theFirstDose, Number = 1, Title = "Disc One" };
        _context.Discs.Add(theFirstDoseDiscOne);
        await _context.SaveChangesAsync();

        _context.DiscContributions.Add(new DiscContribution { Artist = rebelion, Disc = theFirstDoseDiscOne });
        await _context.SaveChangesAsync();

        _context.Tracks.Add(new Track { Disc = theFirstDoseDiscOne, Genre = rawHardstyle, Title = "Hardest Baddest Motherfucker", Number = 2 });
        _context.Tracks.Add(new Track { Disc = theFirstDoseDiscOne, Genre = rawHardstyle, Title = "Modulate", Number = 6 });
        _context.Tracks.Add(new Track { Disc = theFirstDoseDiscOne, Genre = rawHardstyle, Title = "Sydiket", Number = 7 });
        _context.Tracks.Add(new Track { Disc = theFirstDoseDiscOne, Genre = rawHardstyle, Title = "This Is Not A Test", Number = 8 });
        await _context.SaveChangesAsync();

        var tracks = _context.Tracks.ToList();
        _context.Contributions.Add(new Contribution { ContributionType = ContributionType.Main, Artist = rebelion, Track = tracks[0] });
        _context.Contributions.Add(new Contribution { ContributionType = ContributionType.Main, Artist = rebelion, Track = tracks[1] });
        _context.Contributions.Add(new Contribution { ContributionType = ContributionType.Main, Artist = rebelion, Track = tracks[2] });
        _context.Contributions.Add(new Contribution { ContributionType = ContributionType.Main, Artist = rebelion, Track = tracks[3] });
        await _context.SaveChangesAsync();

    }
}
