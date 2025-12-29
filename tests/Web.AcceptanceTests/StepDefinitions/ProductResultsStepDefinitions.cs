using System.Text.RegularExpressions;
using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.StepDefinitions;

[Binding]
public class ProductResultsStepDefinitions(
    ProductLibraryPageObject productLibraryPageObject,
    ProductResultsPageObject productResultsPageObject,
    ScenarioContext scenarioContext
) : PlaywrightTest
{
    private Product CompleteProduct
    {
        get { return scenarioContext.Get<Product>("CompleteProduct"); }
    }

    [When("they go to the product details")]
    public async Task TheyGoToTheProductDetails()
    {
        await productLibraryPageObject.GoToProductForm(CompleteProduct);
    }

    [When("they change the results-tab to {string}")]
    public async Task TheyChangeTheResultsTabTo(string tab)
    {
        await productResultsPageObject.GetResultsTab(tab).ClickAsync();
    }

    [When("they change the whats-the-price to {string}")]
    public async Task TheyChangeWhatsThePriceTabTo(string tab)
    {
        await productResultsPageObject.GetWhatsThePriceTabs(tab).ClickAsync();
    }

    [When("they change the price calculation method to {word}")]
    public async Task TheyChangeThePriceCalculationMethodTo(string method)
    {
        await productResultsPageObject.ChangePriceCalculationMethod(method);
    }

    [When("they expend all the components in the landed costs table")]
    public async Task TheyExpendAllTheComponentsInTheLandedCostsTable()
    {
        var productDetailsOutput = productResultsPageObject.GetProductDetailsOutput();
        var expandableRows = productDetailsOutput.Locator(".product-whats-the-price-table__row").Filter(new LocatorFilterOptions
        {
            Has = productDetailsOutput.Locator(".chevron-down")
        });

        var count = await expandableRows.CountAsync();
        for (int i = 0; i < count; i++)
        {
            var chevronButton = expandableRows.Nth(i).Locator(".chevron-down");
            await chevronButton.ClickAsync();
        }
    }

    [Then("the whats the price table shows the following results")]
    public async Task TheWhatsThePriceTableShowsTheFollowingResults(Table table)
    {
        var results = table.CreateSet<WhatsThePriceTableResult>();
        var resultsTables = productResultsPageObject.GetWhatsThePriceTable();
        foreach (var result in results)
        {
            await Expect(
                resultsTables.GetByTestId($"whats-the-price-table-percentage-{result.CostComponent}")
            ).ToHaveTextAsync(new Regex($@"^{result.Percentage}\.\d{{2}}%$"));
            await Expect(
                resultsTables.GetByTestId($"whats-the-price-table-cost-{result.CostComponent}")
            ).ToHaveTextAsync(new Regex($@"€{result.Costs}\.\d{{3}}$"));

            if (!string.IsNullOrEmpty(result.PricePaid))
            {
                await Expect(
                    resultsTables.GetByTestId($"whats-the-price-table-price-paid-{result.CostComponent}")
                ).ToHaveTextAsync(new Regex($@"€{result.PricePaid}\.\d{{3}}$"));
                await Expect(
                    resultsTables.GetByTestId($"whats-the-price-table-variance-{result.CostComponent}")
                ).ToHaveTextAsync(new Regex($@"€{result.Difference}\.\d{{3}}$"));
            }
        }
    }

    [When("they change the calculation method to {string}")]
    public async Task WhenTheySwitchCalculationMethodTo(string value)
    {
        await productResultsPageObject.SwitchCalculationMethod(value);
    }

    [Then("the price discipline chart shows {string} and {string} series")]
    public async Task ValidatePriceDisciplineChartSeries(string purchasing, string priceDiscipline)
    {
        var chart = productResultsPageObject.GetPriceDisciplineChart();
        await Expect(chart).ToBeVisibleAsync();
        var lineChart = productResultsPageObject.GetLineChartContainer();
        await Expect(lineChart).ToBeVisibleAsync();

        var purchasingline = productResultsPageObject.GetLineChartSeries(purchasing);
        var discipline = productResultsPageObject.GetLineChartSeries(priceDiscipline);

        await purchasingline.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 3000 });
        await discipline.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 3000 });
    }

    [When("they change the date-control currency to {string} and select {string}")]
    public async Task ChangeCurrencyInDateControls(string searchTerm, string resultValue)
    {
        var productCurrencyInput = productResultsPageObject.GetCurrencyInput().First;

        await productCurrencyInput.ClearAsync();
        await productResultsPageObject.SearchAndSelectCurrency(searchTerm, resultValue);
    }

    [Then("the product direct material costs shows {string}")]
    public async Task ThePriceCurrencyShows(string currency)
    {
        await Expect(productResultsPageObject.GetCurrencyInput()).ToHaveValueAsync(currency);
        var regexSymbol = currency switch
        {
            "EUR" => "€",
            "USD" => "$",
            _ => throw new ArgumentException($"Unsupported currency: {currency}")
        };

        var currencyText = await productResultsPageObject.GetDirectMaterialTotal().TextContentAsync();
        Assert.That(currencyText, Does.Contain(regexSymbol));
    }

    [Then("the landed costs results shows the following values")]
    public async Task TheLandedCostsResultsShowsTheFollowingValues(Table table)
    {
        var results = table.CreateSet<LandedCostsResults>();
        var resultsContainer = productResultsPageObject.GetLandedCostsResults();
        foreach (var result in results)
        {
            await Expect(
                resultsContainer.GetByTestId($"product-landed-cost-result-{result.DataTestId}-title")
            ).ToHaveTextAsync(result.Title);
            await Expect(
                resultsContainer.GetByTestId($"product-landed-cost-result-{result.DataTestId}-value")
            ).ToHaveTextAsync(new Regex($@"€{result.Value}"));
        }
    }

    internal class WhatsThePriceTableResult
    {
        public required string CostComponent { get; set; }
        public required string Percentage { get; set; }
        public required string Costs { get; set; }
        public string? PricePaid { get; set; }
        public string? Difference { get; set; }
    }

    internal class LandedCostsResults
    {
        public required string Title { get; set; }
        public required string DataTestId { get; set; }
        public required string Value { get; set; }
    }
}
