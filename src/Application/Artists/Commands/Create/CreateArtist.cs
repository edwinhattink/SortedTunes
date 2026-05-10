using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Artists.Commands.Create;

public record CreateArtistCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateArtistCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateArtistCommand, int>
{
    public async Task<int> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = new Artist() { Name = request.Name };

        context.Artists.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
