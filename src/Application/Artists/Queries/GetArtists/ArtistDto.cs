using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Artists.Queries.GetArtists;

public class ArtistDto
{
    public int Id { get; init; }
    public required string Name { get; init; }

    public static ArtistDto Create(Artist artist)
    {
        return new ArtistDto()
        {
            Id = artist.Id,
            Name = artist.Name
        };
    }
}
