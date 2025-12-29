using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Commands.UpdateAlbum;

public record UpdateAlbumCommand : IRequest
{
    public int Id { get; init; }
    public required string Title { get; init; }
}

public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAlbumCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Albums
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
