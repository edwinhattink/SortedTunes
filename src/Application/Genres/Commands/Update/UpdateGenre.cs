using Microsoft.EntityFrameworkCore;

using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Genres.Commands.Update;

public record UpdateGenreCommand : IRequest
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public int? ParentGenreId { get; set; }
}

public class UpdateGenreCommandHandler(
    IApplicationDbContext context,
    IElasticsearchGenresService elasticsearchGenresService
) : IRequestHandler<UpdateGenreCommand>
{
    public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Genres
            .Include(g => g.ParentGenre)
            .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.ParentGenreId = request.ParentGenreId;

        await context.SaveChangesAsync(cancellationToken);

        // Herlaad ParentGenre na update (ParentGenreId kan gewijzigd zijn)
        if (entity.ParentGenreId.HasValue)
        {
            await context.Genres.Entry(entity).Reference(g => g.ParentGenre).LoadAsync(cancellationToken);
        }
        else
        {
            entity.ParentGenre = null;
        }

        await elasticsearchGenresService.IndexGenreAsync(GenreDocument.Create(entity), cancellationToken);
    }
}
