using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Artists.Commands.UpdateArtist;

public record UpdateArtistCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public class UpdateArtistCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateArtistCommand>
{
    public async Task Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Artists
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);
    }
}
