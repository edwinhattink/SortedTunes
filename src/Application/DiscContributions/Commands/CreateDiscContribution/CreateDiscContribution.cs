using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.DiscContributions.Commands.CreateDiscContribution;

public record CreateDiscContributionCommand : IRequest<int>
{
    public required int DiscId { get; init; }
    public required int ArtistId { get; init; }
}

public class CreateDiscContributionCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateDiscContributionCommand, int>
{
    public async Task<int> Handle(CreateDiscContributionCommand request, CancellationToken cancellationToken)
    {
        var entity = new DiscContribution() { DiscId = request.DiscId, ArtistId = request.ArtistId };

        context.DiscContributions.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
