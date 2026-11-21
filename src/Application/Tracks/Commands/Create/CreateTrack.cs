using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Tracks.Commands.Create;

public record CreateTrackCommand : IRequest<int>
{
    public int? Number { get; init; }
    public required string Title { get; init; }
    public string? FileName { get; set; }
    public required int DiscId { get; set; }
    public required int GenreId { get; set; }
}

public class CreateTrackCommandHandler(
    IApplicationDbContext context,
    IElasticsearchTracksService elasticsearchTracksService
) : IRequestHandler<CreateTrackCommand, int>
{
    public async Task<int> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {
        var entity = new Track()
        {
            Number = request.Number,
            Title = request.Title,
            FileName = request.FileName,
            DiscId = request.DiscId,
            GenreId = request.GenreId
        };

        context.Tracks.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // Laad Disc en Genre voor de nested fields in het document
        await context.Tracks.Entry(entity).Reference(t => t.Disc).LoadAsync(cancellationToken);
        await context.Tracks.Entry(entity).Reference(t => t.Genre).LoadAsync(cancellationToken);

        await elasticsearchTracksService.IndexTrackAsync(TrackDocument.Create(entity), cancellationToken);

        return entity.Id;
    }
}
