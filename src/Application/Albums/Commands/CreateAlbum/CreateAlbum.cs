using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Albums.Commands.CreateAlbum;

public record CreateAlbumCommand : IRequest<int>
{
    public required string Title { get; init; }
}

public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAlbumCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        var entity = new Album() { Title = request.Title };

        _context.Albums.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
