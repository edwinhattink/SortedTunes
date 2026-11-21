using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Tracks.Commands.Update;

public record UpdateTrackCommand : IRequest
{
    public int Id { get; init; }
    public int? Number { get; init; }
    public required string Title { get; init; }
    public string? FileName { get; set; }
    public required int DiscId { get; set; }
    public required int GenreId { get; set; }
}

public class UpdateTrackCommandHandler(
    IApplicationDbContext context,
    IElasticsearchTracksService elasticsearchTracksService
) : IRequestHandler<UpdateTrackCommand>
{
    public async Task Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Tracks
            .Include(t => t.Disc)
            .Include(t => t.Genre)
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Number = request.Number;
        entity.Title = request.Title;
        entity.FileName = request.FileName;
        entity.DiscId = request.DiscId;
        entity.GenreId = request.GenreId;

        await context.SaveChangesAsync(cancellationToken);

        // Herlaad Disc en Genre na update (DiscId/GenreId kunnen gewijzigd zijn)
        await context.Tracks.Entry(entity).Reference(t => t.Disc).LoadAsync(cancellationToken);
        await context.Tracks.Entry(entity).Reference(t => t.Genre).LoadAsync(cancellationToken);

        await elasticsearchTracksService.IndexTrackAsync(TrackDocument.Create(entity), cancellationToken);
    }
}
