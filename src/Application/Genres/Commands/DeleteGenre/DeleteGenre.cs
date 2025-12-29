using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(int Id) : IRequest;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteGenreCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Genres
            .Where(g => g.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Genres.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
