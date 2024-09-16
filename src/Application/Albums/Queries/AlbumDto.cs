using SortedTunes.Application.Common.Mappings;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Albums.Queries;

public class AlbumDto : IMapFrom<Album>
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }
    public string? Image { get; set; }

}
