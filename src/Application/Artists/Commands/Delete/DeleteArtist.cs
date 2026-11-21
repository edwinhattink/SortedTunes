using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;

namespace SortedTunes.Application.Artists.Commands.Delete;

public record DeleteArtistCommand(int Id) : IRequest;

public class DeleteArtistCommandHandler(
    IApplicationDbContext context,
    IElasticsearchArtistsService elasticsearchArtistsService
) : IRequestHandler<DeleteArtistCommand>
{
    public async Task Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Artists
            .Where(a => a.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Artists.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        await elasticsearchArtistsService.DeleteArtistAsync(request.Id, cancellationToken);
    }
}
