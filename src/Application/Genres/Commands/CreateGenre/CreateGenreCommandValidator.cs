using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGenreCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Genre name must not exceed 200 characters.");

        RuleFor(v => v.ParentGenreId)
            .MustAsync(ExistGenre)
                .When(v => v.ParentGenreId.HasValue)
                .WithMessage("Parent genre with Id {PropertyValue} does not exist.")
                .WithErrorCode("Exists");
    }

    private async Task<bool> ExistGenre(int? parentGenreId, CancellationToken cancellationToken)
    {
        if (!parentGenreId.HasValue)
        {
            return true; // If ParentGenreId is null, skip the validation.
        }

        return await _context.Genres.AnyAsync(g => g.Id == parentGenreId.Value, cancellationToken);
    }
}
