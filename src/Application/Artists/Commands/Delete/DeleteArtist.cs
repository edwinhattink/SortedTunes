using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Artists.Commands.Delete;

public record DeleteArtistCommand(int Id) : IRequest;

public class DeleteArtistCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteArtistCommand>
{
    public async Task Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Artists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Artists.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
