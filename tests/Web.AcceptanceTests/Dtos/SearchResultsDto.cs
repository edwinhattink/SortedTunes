namespace SortedTunes.Web.AcceptanceTests.Dtos;

public record SearchResultsDto<T>
{
    public required T[] Items { get; set; }
}

