using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Artists.Queries.GetArtists;

public class ArtistDto : IMapFrom<Artist>
{
    public int Id { get; init; }
    public required string Name { get; init; }
}
