using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.DeleteAlbum;

public record DeleteAlbumCommand(int Id) : IRequest;

public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAlbumCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Albums
            .Where(a => a.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Albums.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
