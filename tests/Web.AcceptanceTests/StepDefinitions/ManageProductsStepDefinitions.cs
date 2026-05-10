using System.Globalization;
using System.Text.RegularExpressions;
using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.StepDefinitions;

[Binding]
public class ManageProductsStepDefinitions(
    ProductLibraryPageObject productLibraryPageObject,
    CreateProductModalObject createProductModalObject,
    ProductDetailsPageObject productDetailsPageObject,
    CommoditySearchModalPageObject commoditySearchModalPageObject,
    ComponentSearchModalPageObject componentSearchModalPageObject,
    CostProfileSearchModalPageObject costProfileSearchModalPageObject,
    FreightLanesSearchModalPageObject freightLanesSearchModalPageObject,
    ScenarioContext scenarioContext
) : PlaywrightTest
{
    private Product Product
    {
        get { return scenarioContext.Get<Product>("Product"); }
        set { scenarioContext["Product"] = value; }
    }

    private Product CompleteProduct
    {
        get { return scenarioContext.Get<Product>("CompleteProduct"); }
    }

    [Given("they go to the product form")]
    public async Task TheyGoToTheProductForm()
    {
        await productLibraryPageObject.GoToProductForm(Product);
    }

    [When("they create a product with name {string}")]
    public async Task TheyCreateAProductWithName(string name)
    {
        await productLibraryPageObject.ClickOnCreateNewProduct();
        Product = new Product()
        {
            Id = await createProductModalObject.SetProductName(name),
            Name = name
        };
    }

    [When("they change the name to {string}")]
    public async Task TheyChangeTheNameTo(string name)
    {
        await Task.Delay(2000);
        await productDetailsPageObject.ChangeInputValue(Product.Id, productDetailsPageObject.ProductNameInput(), name);
        Product.Name = name;
    }

    [When("they change the description to {string}")]
    public async Task TheyChangeTheDescriptionTo(string description)
    {
        await productDetailsPageObject.ChangeInputValue(Product.Id, productDetailsPageObject.ProductDescription(), description);
    }

    [When("they change the currency to {string} and select {string}")]
    public async Task TheyChangeTheCurrencyTo(string currency, string resultValue)
    {
        var productCurrencyInput = productDetailsPageObject.ProductCurrency().First;

        await productCurrencyInput.ClearAsync();
        await productDetailsPageObject.SearchAndSelectCurrency(currency, resultValue);
    }

    [When("the user selects the industry {string} with region {string}")]
    public async Task TheUserSelectsTheIndustryWithRegion(string industry, string region)
    {
        await productDetailsPageObject.ClickSelectCostProfile();
        await costProfileSearchModalPageObject.SearchFor(industry);
        await costProfileSearchModalPageObject.SearchAndSelectRegion(region);
        await costProfileSearchModalPageObject.ClickOnAddSelection(Product.Id);
    }

    [When("the user selects the custom industry {string} with region {string}")]
    public async Task TheUserSelectsTheCustomIndustryWithRegion(string industry, string region)
    {
        await productDetailsPageObject.ClickSelectCostProfile();
        await costProfileSearchModalPageObject.ClickOnCustomOnly();
        await costProfileSearchModalPageObject.SearchForCustom(industry);
        await costProfileSearchModalPageObject.SearchAndSelectCustomRegion(region);
        await costProfileSearchModalPageObject.ClickOnAddSelection(Product.Id);
    }

    [When("the user marks the product as complete")]
    public async Task TheUserMarksTheProductAsComplete()
    {
        await productDetailsPageObject.MarkProductAsComplete(Product.Id);
    }

    [When("the user adds the commodity {string}")]
    public async Task TheUserAddsTheCommodity(string commodityName)
    {
        await TheUserAddsTheCommodityFromRegion(commodityName);
    }

    [When("the user adds the commodity {string} and changes the weight to {int}")]
    public async Task TheUserAddsTheCommodityWithWeight(string commodityName, int weight)
    {
        await TheUserAddsTheCommodityFromRegion(commodityName);
        await productDetailsPageObject.SetWeightForMaterial(Product.Id, commodityName, weight);
    }

    [When("the user adds the commodity {string} from {string}")]
    public async Task TheUserAddsTheCommodityFromRegion(string commodityName, string region = "")
    {
        await productDetailsPageObject.ClickOnAddCommodities();
        await commoditySearchModalPageObject.SearchFor(commodityName);
        await commoditySearchModalPageObject.SelectTheCommodityAndRegion(commodityName, region);
        await commoditySearchModalPageObject.ClickOnAddSelection(Product.Id);
    }

    [When("the user opens the freight lane search modal and selects the freight lane {string}")]
    public async Task TheUserSelectsTheFreightLane(string freightLaneName)
    {
        await productDetailsPageObject.ClickOnOpenFreightLaneSearchModalButton();
        await freightLanesSearchModalPageObject.SearchFor(freightLaneName);
        await freightLanesSearchModalPageObject.SelectFreightLane(freightLaneName);
        await freightLanesSearchModalPageObject.ClickOnAddSelection(Product.Id);
    }

    [When("the user adds the commodity {string} from {string} and changes the weight to {int}")]
    public async Task TheUserAddsTheCommodityFromRegionWithWeight(string commodityName, string region, int weight)
    {
        await TheUserAddsTheCommodityFromRegion(commodityName, region);
        await productDetailsPageObject.SetWeightForMaterial(Product.Id, commodityName, weight);
    }

    [When("the user adds the prepared component with multiplier {int}")]
    public async Task TheUserAddsTheComponentWithMultiplier(int multiplier)
    {
        await productDetailsPageObject.ClickOnAddComponents();
        await componentSearchModalPageObject.SearchFor(CompleteProduct.Name);
        await componentSearchModalPageObject.SelectTheComponent(CompleteProduct.Name);
        await componentSearchModalPageObject.ClickOnAddSelection(Product.Id);
        await productDetailsPageObject.SetWeightForMaterial(Product.Id, CompleteProduct.Name, multiplier);
    }

    [When("they delete the product")]
    public async Task TheyDeleteTheProduct()
    {
        await productLibraryPageObject.DeleteTheProduct(Product);
        Product = null!;
    }

    [When("the user adds the historical purchasing price for price {decimal}, quantity {int} and date {string}")]
    public async Task WhenAddingHistoricalPurchasingPrice(decimal unitPrice, int quantity, string date)
    {
        await productDetailsPageObject.ClickAddAPurchasingPrice();
        await productDetailsPageObject.SetHistoricalUnitPrice(Product.Id, unitPrice, quantity, date);
    }

    [When("they edit the historical purchasing price to price {decimal}, quantity {int} and date {string}")]
    public async Task EditHistoricalPurchasingPrice(decimal unitPrice, int quantity, string date)
    {
        var rows = productDetailsPageObject.GetPurchasePriceRows();

        if (await rows.CountAsync() > 0)
        {
            var row = rows.First;
            await productDetailsPageObject.EditPurchasePriceRow(row, Product.Id, unitPrice, quantity, date);
        }
    }

    [When("they delete the purchasing price with unit price {decimal}, quantity {int} and date {string}")]
    public async Task DeletePurchasingPrice(decimal unitPrice, int quantity, string date)
    {
        var rows = productDetailsPageObject.GetPurchasePriceRows();
        for (int i = 0; i < await rows.CountAsync(); i++)
        {
            var row = rows.Nth(i);
            var priceText = await row.GetByTestId("price-input").InputValueAsync();
            var quantityText = await row.GetByTestId("quantity-input").InputValueAsync();
            var dateText = await row.GetByTestId("date-input").InputValueAsync();
            if (priceText == unitPrice.ToString("0.00") && quantityText == quantity.ToString() && dateText == date)
            {
                await productDetailsPageObject.DeletePurchasePriceRow(row, Product.Id);
                break;
            }
        }
    }

    [When("they delete the freight lane with name {string}")]
    public async Task DeleteFreightLane(string freightLaneName)
    {
        var freightLane = productDetailsPageObject.ProductFreightLane();
        var name = await freightLane.GetByTestId("product-freight-lane-name").TextContentAsync();
        if (name == freightLaneName)
        {
            await productDetailsPageObject.DeleteFreightLane(freightLane, Product.Id);
        }
    }

    [When("they edit the number of units to {int}")]
    public async Task EditTheNumberOfUnits(int numberOfUnits)
    {
        var numberOfUnitsText = productDetailsPageObject.ProductFreightLaneNumberOfUnits();

        await productDetailsPageObject.EditFreightLaneUnits(numberOfUnitsText, Product.Id, numberOfUnits);
    }

    private static bool DecimalTextEquals(string? text, decimal expected)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
               && parsed == expected;
    }

    [Then("the purchasing price table has a row with unit price {decimal}, quantity {int} and date {string}")]
    public async Task PurchasingPriceTableHasRow(decimal unitPrice, int quantity, string date)
    {
        var rows = productDetailsPageObject.GetPurchasePriceRows();
        bool found = false;
        for (int i = 0; i < await rows.CountAsync(); i++)
        {
            var row = rows.Nth(i);
            var priceText = await row.GetByTestId("price-input").InputValueAsync();
            var quantityText = await row.GetByTestId("quantity-input").InputValueAsync();
            var dateText = await row.GetByTestId("date-input").InputValueAsync();
            if (DecimalTextEquals(priceText, unitPrice) && quantityText == quantity.ToString() && dateText == date)
            {
                found = true;
                break;
            }
        }
        Assert.That(found, Is.True, $"Expected purchasing price row {unitPrice}/{quantity}/{date} not found");
    }

    [Then("the purchasing price table does not have a row with unit price {decimal}, quantity {int} and date {string}")]
    public async Task PurchasingPriceTableDoesNotHaveRow(decimal unitPrice, int quantity, string date)
    {
        var rows = productDetailsPageObject.GetPurchasePriceRows();
        for (int i = 0; i < await rows.CountAsync(); i++)
        {
            var row = rows.Nth(i);
            var priceText = await row.GetByTestId("price-input").InputValueAsync();
            var quantityText = await row.GetByTestId("quantity-input").InputValueAsync();
            var dateText = await row.GetByTestId("date-input").InputValueAsync();
            Assert.That(!(priceText == unitPrice.ToString("0.00") && quantityText == quantity.ToString() && dateText == date), $"Found deleted purchasing price row {unitPrice}/{quantity}/{date}");
        }
    }

    [Then("the product does not have a freight lane")]
    public async Task ThenTheProductDoesNotHaveAFreightLane()
    {
        var freightLane = productDetailsPageObject.ProductFreightLane();
        Assert.That(await freightLane.IsVisibleAsync(), Is.False);
        var freightLaneConfigured = productDetailsPageObject.NoFreightLaneConfigured();
        await Expect(freightLaneConfigured).ToBeVisibleAsync();
    }

    [Then("the product has industry {string} from {string}")]
    public async Task ThenTheProductHasIndustryFrom(string industry, string region)
    {
        await Expect(productDetailsPageObject.ProductIndustry()).ToHaveTextAsync(industry);
        await Expect(productDetailsPageObject.ProductRegion()).ToHaveTextAsync(region);
    }

    [Then("the product is created")]
    public void TheProductIsCreated()
    {
        Assert.That(Product.Id, Is.GreaterThan(0));
        Assert.That(Product.Name, Is.Not.Null);
    }

    [Then("the product has name {string}")]
    public async Task TheProductHasName(string name)
    {
        Assert.That(Product.Name.Equals(name));
        await Expect(productDetailsPageObject.ProductNameInput()).ToHaveValueAsync(name);
    }

    [Then("the product has description {string}")]
    public async Task TheProductHasDescription(string description)
    {
        await Task.Delay(500);
        await Expect(productDetailsPageObject.ProductDescription()).ToHaveValueAsync(description);
    }

    [Then("the table shows a list of products")]
    public async Task ShowProductLibrary()
    {
        await productLibraryPageObject.IsOnLibraryPage();
    }

    [Then("the product is marked as complete")]
    public async Task TheProductIsMarkedAsComplete()
    {
        await Expect(productDetailsPageObject.NavigationBar()).ToContainTextAsync("Completed");
    }

    [Then("the product is deleted")]
    public void TheProductIsDeleted()
    {
        Assert.That(Product, Is.Null);
    }

    [Then("the product has freight lane with name {string} and price {int}")]
    public async Task ThenTheProductHasFreightLaneWithNameAndPrice(string freightLaneName, int price)
    {
        var freightLane = productDetailsPageObject.ProductFreightLane();

        await Expect(productDetailsPageObject.ProductFreightLaneNumberOfUnits()).ToHaveValueAsync("1");
        await Expect(freightLane.GetByTestId("product-freight-lane-name")).ToHaveTextAsync(freightLaneName);
        await Expect(freightLane.GetByTestId("product-freight-lane-rate-value")).ToHaveTextAsync(new Regex($@"^€{price:n0}$"));
    }

    [Then("the product has a freight lane with the number of units of {int}")]
    public async Task ThenTheProductHasAFreightLaneWithTheNumberOfUnits(int numberOfUnits)
    {
        await Expect(productDetailsPageObject.ProductFreightLaneNumberOfUnits()).ToHaveValueAsync(numberOfUnits.ToString());
    }

    [Then("the direct-materials table should have the following materials")]
    public async Task TheDirectMaterialsTableShouldHaveTheFollowingMaterials(DataTable table)
    {
        var directMaterials = table.CreateSet<(string commodity, string netWeight, string weightComposition)>();
        var resultsTable = productDetailsPageObject.GetDirectMaterials();

        for (int i = 0; i < directMaterials.Count(); i++)
        {
            var directMaterial = directMaterials.ElementAt(i);
            var resultRow = resultsTable.Nth(i);
            await Expect(resultRow.GetByTestId("name-cell")).ToContainTextAsync(directMaterial.commodity);
            await Expect(resultRow.GetByTestId("weight-composition-cell")).ToHaveTextAsync(directMaterial.weightComposition);
            await Expect(resultRow.GetByTestId("main-value-input")).ToHaveValueAsync(directMaterial.netWeight);
        }
    }

    [Then("the direct-materials table should have the prepared component")]
    public async Task TheDirectMaterialsTableShouldHaveThePreparedComponent(DataTable table)
    {
        var resultsTable = productDetailsPageObject.GetDirectMaterials();
        var (netWeight, weightComposition) = table.CreateInstance<(string netWeight, string weightComposition)>();

        for (int i = 0; i < await resultsTable.CountAsync(); i++)
        {
            var resultRow = resultsTable.Nth(i);
            var text = await resultRow.GetByTestId("name-cell").TextContentAsync();
            if (text == CompleteProduct.Name)
            {
                await Expect(resultRow.GetByTestId("weight-composition-cell")).ToHaveTextAsync(weightComposition);
                await Expect(resultRow.GetByTestId("net-weight-cell")).ToHaveTextAsync(netWeight);
            }
        }
    }

    [Then("the total netweight is {string}")]
    public async Task ValidateTotalsDirectMaterials(string netWeight)
    {
        var totalResults = productDetailsPageObject.GetDirectMaterials(true).First;
        await Expect(totalResults.GetByTestId("total-net-weight-cell")).ToHaveTextAsync(netWeight);
    }

    [Then("the product has currency {string}")]
    public async Task ValidateProductCurrency(string currency)
    {
        await Expect(productDetailsPageObject.ProductCurrency()).ToHaveValueAsync(currency);
    }
}
