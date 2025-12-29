using SortedTunes.Web.AcceptanceTests.Hooks;
using SortedTunes.Web.AcceptanceTests.Models;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class ProductLibraryPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public ILocator ProductRowLocator(int id)
    {
        return Page.GetByTestId($"products-library-row-{id}");
    }

    public async Task SearchForProduct(string search)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GetByTestId("search-products-input").FillAsync(search),
            "Products"
        );
    }

    public async Task GoToProductDetails(string productName)
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            async () => await Page.GetByRole(AriaRole.Link, new() { Name = productName }).ClickAsync(),
            $"Products"
        );
    }

    public async Task GoToProductForm(Product product)
    {
        var productRow = ProductRowLocator(product.Id);
        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            async () => await productRow.GetByTestId("edit-icon").ClickAsync(),
            $"Products/{product.Id}",
            HttpMethod.Get,
            HttpStatusCode.OK
        );

        var hasBeenCompleted = Page.GetByTestId("navigation-progress-bar__edit-button-button");

        if (await hasBeenCompleted.IsVisibleAsync())
        {
            await hasBeenCompleted.ClickAsync();
        }
    }

    public async Task DeleteTheProduct(Product product)
    {
        var productRow = ProductRowLocator(product.Id);
        async Task DeleteTask()
        {
            await productRow.GetByTestId("delete-icon").ClickAsync();
            await Page.GetByTestId("delete-button").ClickAsync();
        }

        await Page.ActionAndWaitForResponseUrlEqualsAsync(
            DeleteTask,
            $"Products/{product.Id}",
            HttpMethod.Delete,
            HttpStatusCode.NoContent
        );
    }

    public async Task ClickOnCreateNewProduct()
    {
        await Page.GetByTestId("create-product-button").ClickAsync();
    }

    public async Task IsOnLibraryPage()
    {
        await Page.GetByTestId("products-library-table").IsVisibleAsync();
    }
}
