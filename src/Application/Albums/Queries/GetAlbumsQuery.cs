using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Application.Albums.Queries;

//[Authorize]
public record GetAlbumsQuery : IRequest<PaginatedList<AlbumDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAlbumsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAlbumsQuery, PaginatedList<AlbumDto>>
{
    public async Task<PaginatedList<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
    {
        //TODO: elasticsearch
        var albums = await context.Albums.ToListAsync(cancellationToken);

        return new PaginatedList<AlbumDto>(
            albums.Select(AlbumDto.Create).ToList(), albums.Count, 1, albums.Count);
    }
}
