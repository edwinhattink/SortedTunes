using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class CostProfileSearchModalPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task SearchFor(string searchTerm)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("default-input").FillAsync(searchTerm),
            $"Search={WebUtility.UrlEncode(searchTerm)}"
        );
        await GetSearchResults().First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task SearchForCustom(string searchTerm)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("default-input").FillAsync(searchTerm),
            $"Search"
        );
        await GetSearchResults().First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    private ILocator GetSearchResults()
    {
        return Page.Locator("[data-testid^='products-industry-region-search-modal-index-row-']");
    }

    public async Task SearchAndSelectRegion(string regionName, int retryCount = 0)
    {
        try
        {
            var searchResults = GetSearchResults();
            var regionInput = searchResults.First.GetByTestId("select-regions-input");
            await regionInput.ClickAsync();
            await Page.RunAndWaitForSuccesfulResponseAsync(
                () => regionInput.FillAsync(regionName),
                $"Search={WebUtility.UrlEncode(regionName)}"
            );
            await GetRegionResult(regionName).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await regionInput.BlurAsync();
            await GetRegionResult(regionName).ClickAsync();
        }
        catch (TimeoutException)
        {
            if (retryCount < 3)
            {
                await SearchAndSelectRegion(regionName, retryCount + 1);
            }
            else
            {
                throw;
            }
        }
    }

    public async Task SearchAndSelectCustomRegion(string regionName, int retryCount = 0)
    {
        try
        {
            var searchResults = GetSearchResults();
            var regionInput = searchResults.First.GetByTestId("autocomplete-input");
            await regionInput.ClickAsync();
            await GetRegionResult(regionName).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await GetRegionResult(regionName).ClickAsync();
        }
        catch (TimeoutException)
        {
            if (retryCount < 3)
            {
                await SearchAndSelectCustomRegion(regionName, retryCount + 1);
            }
            else
            {
                throw;
            }
        }
    }

    public ILocator GetRegionResult(string regionName)
    {
        return Page.GetByRole(AriaRole.Option, new() { Name = regionName });
    }

    public async Task ClickOnAddSelection(int productId)
    {
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            async () => await Page.GetByTestId("add-button").ClickAsync(),
            $"Products/{productId}",
            HttpMethod.Put,
            HttpStatusCode.NoContent
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ClickOnCustomOnly()
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
             async () => await Page.GetByTestId("custom-only-input").ClickAsync(),
             "Search/Industries"
        );
    }
}
