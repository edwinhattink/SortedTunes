using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand : IRequest<int>
{
    public int? ParentGenreId { get; set; }
    public required string Name { get; init; }
}

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateGenreCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = new Genre() { Name = request.Name, ParentGenreId = request.ParentGenreId };

        _context.Genres.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
