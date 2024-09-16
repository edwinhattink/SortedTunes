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
            .NotEmpty()
            .MaximumLength(200)
            .When(v => v.FileName != null)
            .WithMessage("File name must not exceed 200 characters.");

        RuleFor(v => v.DiscId)
            .MustAsync(ExistDisc)
            .WithMessage("Disc with Id {PropertyValue} does not exist.")
            .WithErrorCode("Exists");

        RuleFor(v => v.GenreId)
            .MustAsync(ExistGenre)
            .WithMessage("Genre with Id {PropertyValue} does not exist.")
            .WithErrorCode("Exists");
    }

    private async Task<bool> ExistDisc(int discId, CancellationToken cancellationToken)
    {
        return await _context.Discs.AnyAsync(d => d.Id == discId, cancellationToken);
    }

    private async Task<bool> ExistGenre(int genreId, CancellationToken cancellationToken)
    {
        return await _context.Genres.AnyAsync(g => g.Id == genreId, cancellationToken);
    }
}
