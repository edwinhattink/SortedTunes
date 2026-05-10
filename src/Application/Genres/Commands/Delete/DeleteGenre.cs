using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Genres.Commands.Delete;

public record DeleteGenreCommand(int Id) : IRequest;

public class DeleteGenreCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGenreCommand>
{
    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Genres
            .Where(g => g.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Genres.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
