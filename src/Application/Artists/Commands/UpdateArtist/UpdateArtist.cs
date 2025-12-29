using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Artists.Commands.UpdateArtist;

public record UpdateArtistCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Artists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
