using SortedTunes.Application.Common.Models;
using FluentValidation.Results;

namespace SortedTunes.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, ValidationErrorDetail[]> Errors { get; }

    public ValidationException()
        : base("exceptions.validation.error-occurred")
    {
        Errors = new Dictionary<string, ValidationErrorDetail[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.Select(fg => new ValidationErrorDetail() { Error = fg }).ToArray()
            );
    }

    public ValidationException(IDictionary<string, ValidationErrorDetail[]> failures)
        : this()
    {
        Errors = failures;
    }

    // constructor die in de tests wordt gebruikt
    public ValidationException(string message, IDictionary<string, ValidationErrorDetail[]> errors)
        : base(message)
    {
        Errors = errors;
    }
}
