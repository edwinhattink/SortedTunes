using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Albums.Commands.Create;

public record CreateAlbumCommand : IRequest<int>
{
    public required string Title { get; init; }
}

public class CreateAlbumCommandHandler(
    IApplicationDbContext context,
    IElasticsearchAlbumsService elasticsearchAlbumsService
) : IRequestHandler<CreateAlbumCommand, int>
{
    public async Task<int> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = new Album() { Title = request.Title };

        context.Albums.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        await elasticsearchAlbumsService.IndexAlbumAsync(AlbumDocument.Create(entity), cancellationToken);

        return entity.Id;
    }
}
