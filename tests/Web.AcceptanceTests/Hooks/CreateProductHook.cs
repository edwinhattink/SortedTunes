using Buynamics.Toolkit.AcceptanceTests.Helpers;
using SortedTunes.Web.AcceptanceTests.Clients;
using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.Hooks;

[Binding]
public class CreateProductHook(
    ScenarioContext scenarioContext,
    ProductHttpClient productClient,
    RandomStringGenerator randomStringGenerator
)
{
    private Product Product
    {
        get { return scenarioContext.Get<Product>("Product"); }
        set { scenarioContext["Product"] = value; }
    }


    [BeforeScenario("PrepareProduct", Order = 100)]
    public async Task PrepareProductBefore()
    {
        var productName = $"Test Product {randomStringGenerator.RandomString(10)}";

        var productId = await productClient.CreateProduct(productName);
        Product = new Product() { Id = productId, Name = productName };
    }

    [AfterScenario(Order = 1000)]
    public async Task DeleteProductAfter()
    {
        if (!scenarioContext.ContainsKey("Product"))
        {
            return;
        }
        if (Product == null)
        {
            return;
        }
        await productClient.DeleteProduct(Product.Id);
        Product = null!;
    }
}
