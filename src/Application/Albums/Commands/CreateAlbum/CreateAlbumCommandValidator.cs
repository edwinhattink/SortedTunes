using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.CreateAlbum;

public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateAlbumCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Albums
            .AllAsync(a => a.Title != title, cancellationToken);
    }
}
