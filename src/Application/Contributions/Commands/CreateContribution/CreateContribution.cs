using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Domain.Entities;
using SortedTunes.Domain.Enums;

namespace SortedTunes.Application.Contributions.Commands.CreateContribution;

public record CreateContributionCommand : IRequest<int>
{
    public required int ArtistId { get; init; }
    public required int TrackId { get; init; }
    public required ContributionType ContributionType { get; init; }
}

public class CreateContributionCommandHandler : IRequestHandler<CreateContributionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateContributionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateContributionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Contribution()
        {
            ArtistId = request.ArtistId,
            TrackId = request.TrackId,
            ContributionType = request.ContributionType,
        };

        _context.Contributions.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
