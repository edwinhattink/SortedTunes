using FluentAssertions.Specialized;
using SortedTunes.Application.Common.Exceptions;

namespace SortedTunes.Application.FunctionalTests.Extensions;

public static class FluentAssertionExtensions
{
    public static async Task<ExceptionAssertions<TException>> WithErrorOnProperty<TException>(
        this Task<ExceptionAssertions<TException>> task,
        string property,
        string? error = null)
        where TException : ValidationException
    {
        if (error == null)
        {
            return (await task).Where(e => e.Errors.ContainsKey(property));
        }

        return (await task).Where(e =>
            e.Errors.ContainsKey(property) &&
            e.Errors[property] != null &&
            e.Errors[property].Contains(error)
        );
    }
}
