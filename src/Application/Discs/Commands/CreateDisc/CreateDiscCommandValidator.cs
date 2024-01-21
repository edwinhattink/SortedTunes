using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Discs.Commands.CreateDisc;

public class CreateDiscCommandValidator : AbstractValidator<CreateDiscCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateDiscCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Number)
            .GreaterThan(0)
            .WithMessage("Disc number must be greater than 0.");

        RuleFor(v => v.Title)
            .MaximumLength(200)
            .When(v => v.Title != null)
            .WithMessage("Disc name must not exceed 200 characters.");

        RuleFor(v => v.AlbumId)
            .NotEmpty()
            .MustAsync(ExistAlbum)
                .WithMessage("Album with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");
    }

    private async Task<bool> ExistAlbum(int albumId, CancellationToken cancellationToken)
    {
        return await _context.Albums.AnyAsync(a => a.Id == albumId, cancellationToken);
    }
}
