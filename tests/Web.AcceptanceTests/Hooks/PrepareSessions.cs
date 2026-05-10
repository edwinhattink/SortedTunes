using Buynamics.Toolkit.AcceptanceTests.Authentication;
using Buynamics.Toolkit.AcceptanceTests.Helpers;

namespace SortedTunes.Web.AcceptanceTests.Hooks;

[Binding]
public static class PrepareSessions
{
    public static string? LogPath { get; private set; }

    [BeforeTestRun]
    public static async Task CreateSessions()
    {
        if (!await ApiHealthCheck.CheckApiHealth())
        {
            throw new Exception("API is not healthy");
        }

        var (succesful, logs) = await UserSessions.CreateSessions("");

        if (!succesful)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), $"PrepareUserSessions_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            await File.WriteAllLinesAsync(fileName, logs);
            LogPath = fileName;
        }
    }
}
