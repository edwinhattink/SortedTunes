using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class ProductResultsPageObject(BrowserHook browerHook) : BasePageObject(browerHook.Page)
{
    public ILocator GetResultsTab(string tabName)
    {
        return Page.GetByRole(AriaRole.Tab, new() { Name = tabName });
    }

    public ILocator GetWhatsThePriceTabs(string tabName)
    {
        var input = Page.GetByRole(AriaRole.Tablist);
        return input.GetByRole(AriaRole.Tab, new() { Name = tabName });
    }

    public ILocator GetProductDetailsOutput()
    {
        return Page.GetByTestId("product-details-output");
    }

    public ILocator GetWhatsThePriceTable()
    {
        return Page.Locator("[data-testid^='whats-the-price-table-row-']");
    }

    public ILocator GetLandedCostsResults()
    {
        return Page.GetByTestId("landed-costs-results");
    }

    public async Task ChangePriceCalculationMethod(string method)
    {
        await Page.GetByTestId("select-calculation-method-container").Locator("div").Nth(1).ClickAsync();
        await Page.GetByRole(AriaRole.Option, new() { Name = method }).ClickAsync();
    }

    public ILocator GetPriceDisciplineChart()
    {
        return Page.GetByTestId("price-discipline-chart");
    }

    public ILocator GetLineChartContainer()
    {
        return GetPriceDisciplineChart().GetByTestId("line-chart");
    }

    public ILocator GetLineChartSeries(string seriesName)
    {
        return GetLineChartContainer().Locator($"g[data-series='{seriesName}']");
    }

    public ILocator GetDirectMaterialTotal()
    {
        return Page.GetByTestId("product-direct-material-costs-total-start");
    }

    public ILocator GetCurrencyInput()
    {
        return Page.GetByTestId("currency-autocomplete-input");
    }

    public ILocator GetCalculationMethodInput()
    {
        return Page.GetByTestId("select-calculation-method-input");
    }

    public async Task SwitchCalculationMethod(string resultValue)
    {
        var calculationMethodInput = GetCalculationMethodInput();
        await calculationMethodInput.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible });

        await calculationMethodInput.ClickAsync();

        var option = Page.GetByRole(AriaRole.Option, new() { Name = resultValue });
        await option.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible });
        await Page.RunAndWaitForSuccesfulResponseAsync(
                () => option.ClickAsync(),
                "Results/LandedCosts?FreightLaneCalculationMethod"
        );
    }

    public async Task SearchAndSelectCurrency(string searchValue, string resultValue, int retryCounter = 0)
    {
        try
        {
            await Page.RunAndWaitForSuccesfulResponseAsync(
                () => GetCurrencyInput().FillAsync(searchValue),
                $"Search={searchValue}"
            );

            var option = Page.GetByRole(AriaRole.Option, new() { Name = resultValue });
            await option.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 2000 });
            await Page.RunAndWaitForSuccesfulResponseAsync(
                () => option.ClickAsync(),
                "Product"
            );
        }
        catch (TimeoutException)
        {
            if (retryCounter >= 3)
            {
                throw new Exception($"Failed to select value '{resultValue}' when searching for '{searchValue}'");
            }
            await SearchAndSelectCurrency(searchValue, resultValue, retryCounter + 1);
        }
    }
}
