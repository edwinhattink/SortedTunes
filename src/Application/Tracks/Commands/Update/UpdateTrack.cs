using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.Tracks.Commands.Update;

public record UpdateTrackCommand : IRequest
{
    public int Id { get; init; }
    public int? Number { get; init; }
    public required string Title { get; init; }
    public string? FileName { get; set; }
    public required int DiscId { get; set; }
    public required int GenreId { get; set; }
}

public class UpdateTrackCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTrackCommand>
{
    public async Task Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Tracks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Number = request.Number;
        entity.Title = request.Title;
        entity.FileName = request.FileName;
        entity.DiscId = request.DiscId;
        entity.GenreId = request.GenreId;

        await context.SaveChangesAsync(cancellationToken);
    }
}
