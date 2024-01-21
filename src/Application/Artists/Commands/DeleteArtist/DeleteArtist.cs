using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Artists.Commands.DeleteArtist;

public record DeleteArtistCommand(int Id) : IRequest;

public class DeleteArtistCommandHandler : IRequestHandler<DeleteArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Artists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Artists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
