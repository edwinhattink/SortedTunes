using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Albums.Queries;
//[Authorize]
public record GetAlbumQuery : IRequest<AlbumDto>
{
    public int Id { get; init; }
}

public class GetAlbumQueryHandler : IRequestHandler<GetAlbumQuery, AlbumDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAlbumQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AlbumDto> Handle(GetAlbumQuery request, CancellationToken cancellationToken)
    {
        return await _context.Albums
           .ProjectTo<AlbumDto>(_mapper.ConfigurationProvider)
           .FirstAsync(a => a.Id == request.Id, cancellationToken);
    }
}
