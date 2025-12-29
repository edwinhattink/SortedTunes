namespace SortedTunes.Web.AcceptanceTests.Hooks;

[Binding]
public class BrowserHook
{
    private IBrowser _browser = null!;

    public Page Page { get; private set; } = null!;

    [BeforeScenario(Order = 25)]
    public async Task CreateBrowser()
    {
        _browser = await BrowserFactory.CreateBrowser();

        Page = new Page(_browser);
        await Page.CreateNewPageAsync();
    }

    [AfterScenario]
    public async Task DisposeBrowser(ScenarioContext scenarioContext, DebuggingPage debugPage)
    {
        if (!string.IsNullOrEmpty(PrepareSessions.LogPath))
        {
            TestContext.AddTestAttachment(PrepareSessions.LogPath, "PrepareUserSessions");
        }

        if (scenarioContext.TestError != null)
        {
            var consolelogFileName = Path.Combine(Directory.GetCurrentDirectory(),
                $"{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            await File.WriteAllLinesAsync(consolelogFileName, debugPage.BrowserConsoleLog);
            TestContext.AddTestAttachment(consolelogFileName, "Browserlog on failure");

            var screenshot = await debugPage.ScreenshotAsync();
            var screenshotFileName = Path.Combine(Directory.GetCurrentDirectory(),
                $"{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            await File.WriteAllBytesAsync(screenshotFileName, screenshot);
            TestContext.AddTestAttachment(screenshotFileName, "Screenshot on failure");

            var requestlogFileName = Path.Combine(Directory.GetCurrentDirectory(),
               $"RequestLog_{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            await File.WriteAllLinesAsync(requestlogFileName, debugPage.RequestLog);
            TestContext.AddTestAttachment(requestlogFileName, "Requestlog on failure");

            var responselogFileName = Path.Combine(Directory.GetCurrentDirectory(),
                $"ResponseLog_{scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            await File.WriteAllLinesAsync(responselogFileName, debugPage.ResponseLog);
            TestContext.AddTestAttachment(responselogFileName, "Responselog on failure");
        }

        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
    }
}
