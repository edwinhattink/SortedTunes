using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Artists.Commands.Create;

public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateArtistCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} must not be empty.")
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("{PropertyName} must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Artists
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
