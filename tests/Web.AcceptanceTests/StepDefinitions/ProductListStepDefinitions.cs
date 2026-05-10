using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.StepDefinitions;

[Binding]
public class ProductListStepDefinitions(
    ScenarioContext scenarioContext,
    ProductLibraryPageObject productLibraryPage
) : PlaywrightTest
{
    private Product Product
    {
        get => scenarioContext.Get<Product>("Product");
        set => scenarioContext["Product"] = value;
    }

    private Product CompletedProduct
    {
        get => scenarioContext.Get<Product>("CompleteProduct");
        set => scenarioContext["CompleteProduct"] = value;
    }

    [Given("they search for {string} and open the product details")]
    public async Task TheySearchForAndOpenTheProductDetails(string productName)
    {
        await productLibraryPage.SearchForProduct(productName);
        await productLibraryPage.GoToProductDetails(productName);
    }

    [Given("they search for {string}")]
    public async Task TheySearchForProduct(string productName)
    {
        await productLibraryPage.SearchForProduct(productName);
    }

    [Given("they search for prepared product and open the product details")]
    public async Task TheySearchForPreparedProductAndOpenProductDetails()
    {
        await productLibraryPage.SearchForProduct(Product.Name);
        await productLibraryPage.GoToProductDetails(Product.Name);
    }

    [Given("they search for prepared completed product and open the product details")]
    public async Task TheySearchForPreparedCompletedProductAndOpenProductDetails()
    {
        await productLibraryPage.SearchForProduct(CompletedProduct.Name);
        await productLibraryPage.GoToProductDetails(CompletedProduct.Name);
    }

    [Given("they search for prepared product")]
    public async Task TheySearchForPreparedProduct()
    {
        await productLibraryPage.SearchForProduct(Product.Name);
    }
}
