using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Tracks.Commands.CreateTrack;

public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTrackCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Number)
            .GreaterThanOrEqualTo(1)
            .When(v => v.Number.HasValue)
            .WithMessage("Track number must be at least 1.");

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Track title must not exceed 200 characters.");

        RuleFor(v => v.FileName)
            .MaximumLength(200)
            .When(v => v.FileName != null)
            .WithMessage("File name must not exceed 200 characters.");

        RuleFor(v => v.DiscId)
            .MustAsync(ExistDisc)
            .When(v => v.DiscId.HasValue)
            .WithMessage("Disc with Id '{PropertyValue}' does not exist.")
            .WithErrorCode("Exists");

        RuleFor(v => v.GenreId)
            .MustAsync(ExistGenre)
            .When(v => v.GenreId.HasValue)
            .WithMessage("Genre with Id '{PropertyValue}' does not exist.")
            .WithErrorCode("Exists");
    }

    private async Task<bool> ExistDisc(int? discId, CancellationToken cancellationToken)
    {
        if (!discId.HasValue)
        {
            return true; // Skip validation if DiscId is null.
        }

        return await _context.Discs.AnyAsync(d => d.Id == discId.Value, cancellationToken);
    }

    private async Task<bool> ExistGenre(int? genreId, CancellationToken cancellationToken)
    {
        if (!genreId.HasValue)
        {
            return true; // Skip validation if GenreId is null.
        }

        return await _context.Genres.AnyAsync(g => g.Id == genreId.Value, cancellationToken);
    }
}
