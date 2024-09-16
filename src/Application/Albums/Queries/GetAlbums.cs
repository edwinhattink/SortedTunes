using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Common.Mappings;
using SortedTunes.Application.Common.Models;

namespace SortedTunes.Application.Albums.Queries.GetAlbums;

//[Authorize]
public record GetAlbumsQuery : IRequest<PaginatedList<AlbumDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, PaginatedList<AlbumDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAlbumsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Albums
           .ProjectTo<AlbumDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
