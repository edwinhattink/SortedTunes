using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Discs.Commands.CreateDisc;

public record CreateDiscCommand : IRequest<int>
{
    public required int Number { get; init; }
    public string? Title { get; init; }
    public required int AlbumId { get; init; }
}

public class CreateDiscCommandHandler : IRequestHandler<CreateDiscCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDiscCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDiscCommand request, CancellationToken cancellationToken)
    {
        var entity = new Disc()
        {
            Number = request.Number,
            Title = request.Title,
            AlbumId = request.AlbumId
        };

        _context.Discs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
