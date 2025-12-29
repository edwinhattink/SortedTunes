using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Contributions.Commands.CreateContribution;

public class CreateContributionCommandValidator : AbstractValidator<CreateContributionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateContributionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.ArtistId)
            .NotEmpty()
            .MustAsync(ExistArtist)
                .WithMessage("Artist with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");

        RuleFor(v => v.TrackId)
            .NotEmpty()
            .MustAsync(ExistTrack)
                .WithMessage("Track with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");

        RuleFor(v => v.ContributionType)
            .IsInEnum()
            .WithMessage("Invalid contribution type.");
    }

    private async Task<bool> ExistArtist(int artistId, CancellationToken cancellationToken)
    {
        return await _context.Artists.AnyAsync(a => a.Id == artistId, cancellationToken);
    }

    private async Task<bool> ExistTrack(int trackId, CancellationToken cancellationToken)
    {
        return await _context.Tracks.AnyAsync(t => t.Id == trackId, cancellationToken);
    }
}
