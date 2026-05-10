using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Genres.Commands.Create;

public record CreateGenreCommand : IRequest<int>
{
    public int? ParentGenreId { get; set; }
    public required string Name { get; init; }
}

public class CreateGenreCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGenreCommand, int>
{
    public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var entity = new Genre() { Name = request.Name, ParentGenreId = request.ParentGenreId };

        context.Genres.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
