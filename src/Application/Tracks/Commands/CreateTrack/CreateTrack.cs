using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Tracks.Commands.CreateTrack;

public record CreateTrackCommand : IRequest<int>
{
    public int? Number { get; init; }
    public required string Title { get; init; }
    public string? FileName { get; set; }
    public required int DiscId { get; set; }
    public required int GenreId { get; set; }
}

public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTrackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {
        var entity = new Track()
        {
            Number = request.Number,
            Title = request.Title,
            FileName = request.FileName,
            DiscId = request.DiscId,
            GenreId = request.GenreId
        };

        _context.Tracks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
