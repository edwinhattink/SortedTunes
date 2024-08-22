using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandValidator : AbstractValidator<UpdateAlbumCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAlbumCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("{PropertyName} must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(UpdateAlbumCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Albums
            .Where(a => a.Id != model.Id)
            .AllAsync(a => a.Title != title, cancellationToken);
    }
}
