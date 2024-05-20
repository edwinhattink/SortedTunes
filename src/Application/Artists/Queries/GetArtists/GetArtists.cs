using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Common.Mappings;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Application.Artists.Queries.GetArtists;

//[Authorize]
public record GetArtistsQuery : IRequest<PaginatedList<ArtistDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, PaginatedList<ArtistDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetArtistsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ArtistDto>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Artists
           .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
