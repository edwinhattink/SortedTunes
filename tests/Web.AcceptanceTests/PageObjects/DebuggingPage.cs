using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class DebuggingPage(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task<byte[]> ScreenshotAsync()
    {
        return await Page.ScreenshotAsync();
    }

    public async Task<string> ContentAsync()
    {
        return await Page.ContentAsync();
    }

    public IEnumerable<string> BrowserConsoleLog => Page.BrowserConsoleLog;
    public IEnumerable<string> RequestLog => Page.RequestLog;
    public IEnumerable<string> ResponseLog => Page.ResponseLog;

    public Page GetPage() { return Page; }
}

