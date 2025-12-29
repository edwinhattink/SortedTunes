using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class ProductDetailsPageObject(BrowserHook browerHook) : BasePageObject(browerHook.Page)
{
    public ILocator ProductDescription() => Page.GetByTestId("product-description-input");
    public ILocator ProductNameInput() => Page.GetByTestId("product-name-input");
    public ILocator ProductTagsInput() => Page.GetByTestId("product-tags-input");
    public ILocator ProductIndustry() => Page.GetByTestId("product-industry");
    public ILocator ProductRegion() => Page.GetByTestId("product-region");
    public ILocator MarkAsCompleteButton() => Page.GetByTestId("mark-as-complete-button");
    public ILocator NavigationBar() => Page.GetByTestId("navigation-progress-bar-main-container");
    public ILocator HistoricalBuyingPrices() => Page.GetByTestId("navigation-progress-bar-main-container");
    public ILocator ProductFreightLane() => Page.GetByTestId("product-freight-lane");
    public ILocator NoFreightLaneConfigured() => Page.GetByTestId("no-freight-lane-configured");
    public ILocator ProductFreightLaneNumberOfUnits() => Page.GetByTestId("number-of-units-input");
    public ILocator ProductCurrency() => Page.GetByTestId("currency-autocomplete-input");

    public ILocator CommodityRowLocator(string which, string commodityName = "")
    {
        var productRows = Page.GetByRole(AriaRole.Table).Locator($"tr[data-testid^='search-modal-{which}-index-row-']");
        return string.IsNullOrEmpty(commodityName) ?
            productRows.First :
            productRows.Filter(new() { HasText = commodityName }).First;
    }

    public ILocator GetDirectMaterials(bool getTotalRow = false)
    {
        var testId = getTotalRow ? "product-composition-form-totals-row" : "product-composition-form-row";
        return Page.GetByRole(AriaRole.Table).Locator($"tr[data-testid^='{testId}']");
    }

    public async Task SetWeightForMaterial(int productId, string materialName, int weightValue)
    {
        var materialRow = Page.GetByRole(AriaRole.Row, new() { Name = materialName });
        var weightInput = materialRow.GetByTestId("main-value-input");

        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => weightInput.FillAsync(weightValue.ToString()),
            $"Products/{productId}/Commodities"
        );
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ClickSelectCostProfile()
    {
        await Page.GetByTestId("select-cost-profile").ClickAsync();
    }

    public async Task ClickOnAddCommodities()
    {
        await Page.GetByTestId("add-commodities-button").ClickAsync();
    }

    public async Task ClickOnAddComponents()
    {
        await Page.GetByTestId("add-components-button").ClickAsync();
    }

    public async Task ClickOnOpenFreightLaneSearchModalButton()
    {
        await Page.GetByTestId("open-freight-lanes-search-modal-button").ClickAsync();
    }

    public async Task ClickAddAPurchasingPrice()
    {
        await Page.GetByTestId("create-product-purchase-price-button").ClickAsync();
    }

    public async Task SetHistoricalUnitPrice(int productId, decimal unitPrice, int quantity, string date)
    {
        var inputRow = Page.GetByTestId("purchasePriceRow--1");

        await EditPurchasePriceRow(inputRow, productId, unitPrice, quantity, date);
    }

    public ILocator GetPurchasePriceRows()
    {
        return Page.Locator("tr[data-testid^='purchasePriceRow-']").Filter(new LocatorFilterOptions { HasNotText = "--1" });
    }

    public async Task DeletePurchasePriceRow(ILocator row, int productId)
    {
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            async () => await row.GetByTestId("delete-purchase-price-button").ClickAsync(),
            $"Products/{productId}",
            HttpMethod.Delete,
            HttpStatusCode.NoContent
        );
    }

    public async Task DeleteFreightLane(ILocator row, int productId)
    {
        await Page.RunAndWaitForResponseAsync(
            async () => await row.GetByTestId("product-freight-lane-action-button").ClickAsync(),
            response => response.Url.Contains($"Products/{productId}/FreightLanes")
                && response.Status == (int)HttpStatusCode.NoContent
                && response.Request.Method == HttpMethod.Delete.ToString()
        );
    }

    public async Task EditFreightLaneUnits(ILocator input, int productId, int numberOfUnits)
    {
        await Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await input.FillAsync(numberOfUnits.ToString());
                await input.BlurAsync();
            },
            response => response.Url.Contains($"Products/{productId}/FreightLanes")
                && response.Status == (int)HttpStatusCode.NoContent
                && response.Request.Method == HttpMethod.Put.ToString()
        );
    }

    public async Task EditPurchasePriceRow(ILocator row, int productId, decimal unitPrice, int quantity, string date)
    {
        var dateParts = date.Split('/');

        await Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await row.GetByTestId("price-input").FillAsync(unitPrice.ToString());
                await row.GetByTestId("quantity-input").FillAsync(quantity.ToString());

                await row.GetByRole(AriaRole.Spinbutton, new() { Name = "Day" }).FillAsync(dateParts[0]);
                await row.GetByRole(AriaRole.Spinbutton, new() { Name = "Month" }).FillAsync(dateParts[1]);
                await row.GetByRole(AriaRole.Spinbutton, new() { Name = "Year" }).FillAsync(dateParts[2]);

                await row.BlurAsync();
            },
            response => response.Url.Contains($"Products/{productId}/PurchasingPrices") && response.Request.Method == HttpMethod.Get.ToString()
        );
    }

    public async Task ChangeInputValue(int productId, ILocator input, string value)
    {
        await input.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            async () =>
            {
                await input.FillAsync(value);
                await input.BlurAsync();
            },
            $"Products/{productId}",
            HttpMethod.Get,
            HttpStatusCode.OK
        );
    }

    public async Task MarkProductAsComplete(int productId)
    {
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            async () => await MarkAsCompleteButton().ClickAsync(),
            $"Products/{productId}/MarkAsComplete",
            HttpMethod.Post,
            HttpStatusCode.NoContent
        );
    }

    public async Task SearchAndSelectCurrency(string searchValue, string resultValue, int retryCounter = 0)
    {
        try
        {
            await Page.RunAndWaitForSuccesfulResponseAsync(
                () => ProductCurrency().FillAsync(searchValue),
                $"Search={searchValue}"
            );

            var option = Page.GetByRole(AriaRole.Option, new() { Name = resultValue });
            await option.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 2000 });
            await Page.RunAndWaitForSuccesfulResponseAsync(
                () => option.ClickAsync(),
                $"CurrencyCode={searchValue}"
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
