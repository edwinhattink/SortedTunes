using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.Update;

public record UpdateAlbumCommand : IRequest
{
    public int Id { get; init; }
    public required string Title { get; init; }
}

public class UpdateAlbumCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateAlbumCommand>
{
    public async Task Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Albums
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);
    }
}
