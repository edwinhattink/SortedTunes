using System.Data.Common;

namespace SortedTunes.Application.FunctionalTests;

public interface ITestDatabase
{
    Task InitialiseAsync();
    DbConnection GetConnection();
    string GetConnectionString();
    Task ResetAsync();
    Task DisposeAsync();
}
