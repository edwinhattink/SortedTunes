using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Albums.Queries;

public class AlbumDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }
    public string? Image { get; set; }

    public static AlbumDto Create(Album album)
    {
        return new AlbumDto()
        {
            Id = album.Id,
            Title = album.Title,
            ReleaseYear = album.ReleaseYear,
            Image = album.Image
        };
    }
}
