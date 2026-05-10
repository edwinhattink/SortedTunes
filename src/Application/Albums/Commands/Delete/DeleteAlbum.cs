using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.Delete;

public record DeleteAlbumCommand(int Id) : IRequest;

public class DeleteAlbumCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteAlbumCommand>
{
    public async Task Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Albums
            .Where(a => a.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Albums.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
