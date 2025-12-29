using Buynamics.Toolkit.AcceptanceTests.Authentication;

namespace SortedTunes.Web.AcceptanceTests.Hooks;

[Binding]
public class LoginHook(BrowserHook browserHook)
{
    public Page Page { get; private set; } = browserHook.Page;
    private static string BaseUrl => ConfigurationHelper.GetBaseUrl();

    [BeforeScenario("Admin", Order = 150)]
    public async Task Admin()
    {
        await Page.LoginAsAsync(UserType.Admin);
        await OpenWebpage();
    }

    [BeforeScenario("User", Order = 150)]
    public async Task LoginAsUser()
    {
        await Page.LoginAsAsync(UserType.User);
        await OpenWebpage();
    }

    private async Task OpenWebpage()
    {
        await Page.RunAndWaitForSuccesfulResponseAsync(
            () => Page.GoToAsync($"{BaseUrl}/cost-models/products"),
            "oauth/token"
        );
    }
}
