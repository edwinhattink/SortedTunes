using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGenreCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Genre name must not exceed 200 characters.");

        RuleFor(v => v.ParentGenreId)
            .MustAsync(ExistGenre)
                .When(v => v.ParentGenreId.HasValue)
                .WithMessage("Parent genre with Id '{PropertyValue}' does not exist.")
                .WithErrorCode("Exists");
    }

    private async Task<bool> ExistGenre(UpdateGenreCommand model, int? parentGenreId, CancellationToken cancellationToken)
    {
        if (!parentGenreId.HasValue)
        {
            return true; // If ParentGenreId is null, skip the validation.
        }

        return await _context.Genres
            .Where(g => g.Id != model.Id) // Ensure the parent genre is not the genre being updated
            .AnyAsync(g => g.Id == parentGenreId.Value, cancellationToken);
    }
}
