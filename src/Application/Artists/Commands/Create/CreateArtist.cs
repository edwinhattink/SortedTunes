using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Artists.Commands.Create;

public record CreateArtistCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateArtistCommandHandler(
    IApplicationDbContext context,
    IElasticsearchArtistsService elasticsearchArtistsService
) : IRequestHandler<CreateArtistCommand, int>
{
    public async Task<int> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = new Artist() { Name = request.Name };

        context.Artists.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        await elasticsearchArtistsService.IndexArtistAsync(ArtistDocument.Create(entity), cancellationToken);

        return entity.Id;
    }
}
