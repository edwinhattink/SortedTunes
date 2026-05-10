namespace SortedTunes.Web.AcceptanceTests.Dtos;

public record CommodityDto
{
    public required CommodityRegionDto[] Regions { get; set; }
}
