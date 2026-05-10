using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class CreateProductModalObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public ILocator ProductNameFromCreateModal()
    {
        return Page.GetByTestId("product-name-input");
    }

    public async Task<int> SetProductName(string productName)
    {
        await ProductNameFromCreateModal().FillAsync(productName);

        var response = await Page.ActionAndWaitForResponseUrlEqualsAsync(
           async () => await Page.GetByTestId("create-button").ClickAsync(),
           "Products",
           HttpMethod.Post,
           HttpStatusCode.Created
       );

        return int.Parse(response);
    }
}
