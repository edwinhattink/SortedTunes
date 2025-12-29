using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.StepDefinitions;

[Binding]
public class NavigationStepDefinitions : PlaywrightTest
{
    private static string BaseUrl => ConfigurationHelper.GetBaseUrl();
    public Page Page { get; private set; }

    protected NavigationStepDefinitions(BrowserHook browserHook)
    {
        Page = browserHook.Page;
        SetDefaultExpectTimeout(30000);
    }


    [Given("the user navigates to '(.*)'")]
    public async Task AUserOpensAPage(string path)
    {
        await Page.GoToAsync($"{BaseUrl}/{path}");
    }
}
