using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Discs.Commands.Create;

public record CreateDiscCommand : IRequest<int>
{
    public required int Number { get; init; }
    public string? Title { get; init; }
    public required int AlbumId { get; init; }
}

public class CreateDiscCommandHandler(
    IApplicationDbContext context,
    IElasticsearchDiscsService elasticsearchDiscsService,
    IElasticsearchAlbumsService elasticsearchAlbumsService
) : IRequestHandler<CreateDiscCommand, int>
{
    public async Task<int> Handle(CreateDiscCommand request, CancellationToken cancellationToken)
    {
        var entity = new Disc()
        {
            Number = request.Number,
            Title = request.Title,
            AlbumId = request.AlbumId
        };

        context.Discs.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // Laad Album voor nested field + update DiscCount op het album
        await context.Discs.Entry(entity).Reference(d => d.Album).LoadAsync(cancellationToken);

        await elasticsearchDiscsService.IndexDiscAsync(DiscDocument.Create(entity), cancellationToken);

        // Herindexeer het album zodat DiscCount bijgewerkt is
        var album = await context.Albums
            .Include(a => a.Discs)
            .SingleOrDefaultAsync(a => a.Id == request.AlbumId, cancellationToken);

        if (album != null)
        {
            await elasticsearchAlbumsService.IndexAlbumAsync(AlbumDocument.Create(album), cancellationToken);
        }

        return entity.Id;
    }
}
