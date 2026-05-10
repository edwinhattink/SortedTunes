using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class CommoditySearchModalPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task SearchFor(string searchTerm)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("default-input").FillAsync(searchTerm),
            $"Search={WebUtility.UrlEncode(searchTerm)}"
        );
        await GetSearchResults().First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task SelectTheCommodityAndRegion(string resultName, string regionName = "")
    {
        var results = GetSearchResults();
        foreach (var commodityRow in await results.AllAsync())
        {
            var text = await commodityRow.GetByTestId("search-result-commodity-name").TextContentAsync();
            if (text == resultName)
            {
                await commodityRow.WaitForAsync(new() { State = WaitForSelectorState.Visible });
                await commodityRow.ClickAsync();
                ILocator regionSelector = string.IsNullOrEmpty(regionName)
                    ? Page.GetByRole(AriaRole.Option).First
                    : Page.GetByRole(AriaRole.Option, new() { Name = regionName });
                if (await regionSelector.IsVisibleAsync())
                {
                    await regionSelector.ClickAsync();
                    await commodityRow.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync(new() { Force = true });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync(new() { Force = true });
                }
                break;
            }
        }
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public ILocator GetSearchResults()
    {
        return Page.Locator("[data-testid^='search-modal-commodity-index-row-']");
    }

    public async Task ClickOnAddSelection(int productId)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("add-button").ClickAsync(),
            $"Products/{productId}/Commodities"
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
