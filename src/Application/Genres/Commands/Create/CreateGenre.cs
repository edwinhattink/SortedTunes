using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Genres.Commands.Create;

public record CreateGenreCommand : IRequest<int>
{
    public int? ParentGenreId { get; set; }
    public required string Name { get; init; }
}

public class CreateGenreCommandHandler(
    IApplicationDbContext context,
    IElasticsearchGenresService elasticsearchGenresService
) : IRequestHandler<CreateGenreCommand, int>
{
    public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = new Genre() { Name = request.Name, ParentGenreId = request.ParentGenreId };

        context.Genres.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // Laad ParentGenre voor nested field (indien aanwezig)
        if (entity.ParentGenreId.HasValue)
        {
            await context.Genres.Entry(entity).Reference(g => g.ParentGenre).LoadAsync(cancellationToken);
        }

        await elasticsearchGenresService.IndexGenreAsync(GenreDocument.Create(entity), cancellationToken);

        return entity.Id;
    }
}
