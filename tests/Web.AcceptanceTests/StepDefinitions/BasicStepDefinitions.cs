namespace SortedTunes.Web.AcceptanceTests.StepDefinitions;

[Binding]
public class BasicStepDefinitions(BasicPageObject basicPage) : PlaywrightTest
{
    [When("the user changes the date range from {word} {int} to {word} {int}")]
    public async Task ChangesTheDateRangeFromTo(string fromMonth, int fromYear, string toMonth, int toYear)
    {
        await basicPage.FillInStartMonth(fromMonth, fromYear);
        await basicPage.FillInEndMonth(toMonth, toYear);
    }
}

