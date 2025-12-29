using System.Net.Http.Json;
using Buynamics.Toolkit.AcceptanceTests.Helpers;
using SortedTunes.Web.AcceptanceTests.Clients;
using SortedTunes.Web.AcceptanceTests.Dtos;
using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.Hooks;

[Binding]
public class CreateCompleteProductHook(
    ScenarioContext scenarioContext,
    ProductHttpClient productClient,
    HttpClientCreator httpClientCreator,
    RandomStringGenerator randomStringGenerator
)
{
    private Product CompleteProduct
    {
        get { return scenarioContext.Get<Product>("CompleteProduct"); }
        set { scenarioContext["CompleteProduct"] = value; }
    }

    [BeforeScenario("PrepareCompleteProduct", Order = 100)]
    public async Task PrepareComponentBefore()
    {
        var client = await httpClientCreator.GetClient(ProductHttpClient.Audience);
        var hopsCommodities = await client.GetFromJsonAsync<SearchResultsDto<CommodityDto>>("Search/Commodities?search=hops")
            ?? throw new Exception("Cannot get hops commodities");
        var grainsCommodities = await client.GetFromJsonAsync<SearchResultsDto<CommodityDto>>("Search/Commodities?search=brewers grains")
            ?? throw new Exception("Cannot get brewers grains commodities");
        var industries = await client.GetFromJsonAsync<SearchResultsDto<IndustryDto>>("Search/Industries?search=breweries")
            ?? throw new Exception("Cannot get custom industries");
        var regions = await client.GetFromJsonAsync<SearchResultsDto<RegionDto>>("Search/Regions?search=Netherlands")
            ?? throw new Exception("Cannot get regions");
        var productComponentName = $"Test Component {randomStringGenerator.RandomString(10)}";
        var productId = await productClient.CreateProduct(productComponentName, regions.Items[0].Id, industries.Items[0].Id);
        await productClient.CreateProductCommodity(
            productId,
            hopsCommodities.Items[0].Regions[0].CommodityRegionId,
            2
        );
        await productClient.CreateProductCommodity(
            productId,
            grainsCommodities.Items[0].Regions[0].CommodityRegionId,
            1
        );
        await productClient.MarkAsComplete(productId);

        await productClient.CreateHistoricalPrices(productId);

        CompleteProduct = new Product() { Id = productId, Name = productComponentName };
    }

    [AfterScenario(Order = 1100)]
    public async Task DeleteComponentAfter()
    {
        if (!scenarioContext.ContainsKey("CompleteProduct"))
        {
            return;
        }
        if (CompleteProduct == null)
        {
            return;
        }
        await productClient.DeleteProduct(CompleteProduct.Id);
        CompleteProduct = null!;
    }
}
