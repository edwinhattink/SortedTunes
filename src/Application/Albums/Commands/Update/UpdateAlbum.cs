using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Albums.Commands.Update;

public record UpdateAlbumCommand : IRequest
{
    public int Id { get; init; }
    public required string Title { get; init; }
}

public class UpdateAlbumCommandHandler(
    IApplicationDbContext context,
    IElasticsearchAlbumsService elasticsearchAlbumsService
) : IRequestHandler<UpdateAlbumCommand>
{
    public async Task Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Albums
            .Include(a => a.Discs)
            .SingleOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);

        await elasticsearchAlbumsService.IndexAlbumAsync(AlbumDocument.Create(entity), cancellationToken);
    }
}
