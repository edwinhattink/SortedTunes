using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Queries;

//[Authorize]
public record GetAlbumQuery : IRequest<AlbumDto>
{
    public int Id { get; init; }
}

public class GetAlbumQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAlbumQuery, AlbumDto>
{
    public async Task<AlbumDto> Handle(GetAlbumQuery request, CancellationToken cancellationToken)
    {
        var album = await context.Albums
           .FirstAsync(a => a.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, album);

        return AlbumDto.Create(album);
    }
}
