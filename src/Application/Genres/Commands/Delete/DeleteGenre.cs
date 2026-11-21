using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;

namespace SortedTunes.Application.Genres.Commands.Delete;

public record DeleteGenreCommand(int Id) : IRequest;

public class DeleteGenreCommandHandler(
    IApplicationDbContext context,
    IElasticsearchGenresService elasticsearchGenresService
) : IRequestHandler<DeleteGenreCommand>
{
    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Genres
            .Where(g => g.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Genres.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        await elasticsearchGenresService.DeleteGenreAsync(request.Id, cancellationToken);
    }
}
