using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Application.Artists.Queries.GetArtists;

//[Authorize]
public record GetArtistsQuery : IRequest<PaginatedList<ArtistDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetArtistsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetArtistsQuery, PaginatedList<ArtistDto>>
{
    public async Task<PaginatedList<ArtistDto>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
    {
        var artists = await context.Artists.ToListAsync(cancellationToken);

        return new PaginatedList<ArtistDto>(
            artists.Select(ArtistDto.Create).ToList(), artists.Count, 1, artists.Count);
    }
}
