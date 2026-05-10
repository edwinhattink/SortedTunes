using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class FreightLanesSearchModalPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task SearchFor(string searchTerm)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("search-input").FillAsync(searchTerm),
            $"Lanes?Search"
        );
        await GetSearchResults().First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task SelectFreightLane(string resultName)
    {
        var results = GetSearchResults();
        foreach (var freightLaneRow in await results.AllAsync())
        {
            await freightLaneRow.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            var row = freightLaneRow.GetByText(resultName);
            if (row != null)
            {
                await row.ClickAsync();
                break;
            }
        }
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public ILocator GetSearchResults()
    {
        return Page.Locator("[data-testid^='freight-lane-search-row']");
    }

    public async Task ClickOnAddSelection(int productId)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("add-button").ClickAsync(),
            $"Products/{productId}/FreightLanes"
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
