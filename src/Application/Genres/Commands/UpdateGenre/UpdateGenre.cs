using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand : IRequest
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public int? ParentGenreId { get; set; }
}

public class UpdateGenreCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGenreCommand>
{
    public async Task Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Genres
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.ParentGenreId = request.ParentGenreId;

        await context.SaveChangesAsync(cancellationToken);
    }
}
