using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.DiscContributions.Commands.CreateDiscContribution;

public class CreateDiscContributionCommandValidator : AbstractValidator<CreateDiscContributionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateDiscContributionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.DiscId)
            .NotEmpty()
            .MustAsync(ExistDisc)
                .WithMessage("Disc with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");

        RuleFor(v => v.ArtistId)
            .NotEmpty()
            .MustAsync(ExistArtist)
                .WithMessage("Artist with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");
    }

    private async Task<bool> ExistDisc(int discId, CancellationToken cancellationToken)
    {
        return await _context.Discs.AnyAsync(d => d.Id == discId, cancellationToken);
    }

    private async Task<bool> ExistArtist(int artistId, CancellationToken cancellationToken)
    {
        return await _context.Artists.AnyAsync(a => a.Id == artistId, cancellationToken);
    }
}
