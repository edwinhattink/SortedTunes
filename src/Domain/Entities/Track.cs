﻿namespace SortedTunes.Domain.Entities;

public class Track : BaseAuditableEntity
{
    public int? Number { get; set; }
    public required string Title { get; set; }

    public string? FileName { get; set; }

    public int DiscId { get; set; }
    public Disc? Disc { get; set; }

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    public List<Contribution> Contributions { get; set; } = new List<Contribution>();
}
