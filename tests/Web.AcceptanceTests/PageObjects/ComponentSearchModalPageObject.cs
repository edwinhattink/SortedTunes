using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class ComponentSearchModalPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task SearchFor(string searchTerm)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("default-input").FillAsync(searchTerm),
            $"Search={WebUtility.UrlEncode(searchTerm)}"
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await GetSearchResults().First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task SelectTheComponent(string resultName)
    {
        var results = GetSearchResults();
        foreach (var commodityRow in await results.AllAsync())
        {
            var text = await commodityRow.GetByTestId("search-result-component-name").TextContentAsync();
            if (text == resultName)
            {
                await commodityRow.WaitForAsync(new() { State = WaitForSelectorState.Visible });
                await commodityRow.ClickAsync();
            }
        }
    }

    public ILocator GetSearchResults()
    {
        return Page.Locator("[data-testid^='search-modal-component-index-row-']");
    }

    public async Task ClickOnAddSelection(int productId)
    {
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            () => Page.GetByTestId("add-button").ClickAsync(),
            $"Products/{productId}/Components",
            HttpMethod.Get,
            HttpStatusCode.OK
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
