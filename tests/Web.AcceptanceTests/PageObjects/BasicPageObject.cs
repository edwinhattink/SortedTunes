using SortedTunes.Web.AcceptanceTests.Hooks;

namespace SortedTunes.Web.AcceptanceTests.PageObjects;

public class BasicPageObject(BrowserHook browserHook) : BasePageObject(browserHook.Page)
{
    public async Task FillInStartMonth(string month, int year)
    {
        var datePicker = Page.GetByTestId("start-date-picker-container");
        await FillInDate(datePicker, month, year);
    }

    public async Task FillInEndMonth(string month, int year)
    {
        var datePicker = Page.GetByTestId("end-date-picker-container");
        await FillInDate(datePicker, month, year);
    }

    private static async Task FillInDate(ILocator datePicker, string month, int year)
    {
        await datePicker.GetByRole(AriaRole.Spinbutton, new() { Name = "Month" }).FillAsync(month);
        await datePicker.GetByRole(AriaRole.Spinbutton, new() { Name = "Year" }).FillAsync($"{year}");
        await datePicker.BlurAsync();
    }
}
