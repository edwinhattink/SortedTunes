using SortedTunes.Application.Common.Exceptions;
using SortedTunes.Application.Common.Models;
using FluentValidation.Results;
using NUnit.Framework;

namespace SortedTunes.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        Assert.That(actual.Keys, Is.Empty);
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new("Age", "must be over 18"),
            };

        var actual = new ValidationException(failures).Errors;

        Assert.That(actual.Keys, Is.EquivalentTo(["Age"]));

        var expected = new List<ValidationErrorDetail>
        {
            new() { Error = "must be over 18" }
        };
        Assert.That(actual["Age"], Is.EqualTo(expected).AsCollection);
    }

    [Test]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new("Age", "must be 18 or older"),
                new("Age", "must be 25 or younger"),
                new("Password", "must contain at least 8 characters"),
                new("Password", "must contain a digit"),
                new("Password", "must contain upper case letter"),
                new("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        Assert.That(actual.Keys, Is.EquivalentTo(["Password", "Age"]));

        var expectedAge = new[]
        {
            new ValidationErrorDetail() { Error = "must be 18 or older" },
            new ValidationErrorDetail() { Error = "must be 25 or younger" },
        };
        Assert.That(actual["Age"], Is.EqualTo(expectedAge).AsCollection);

        var expectedPassword = new[]
        {
            new ValidationErrorDetail() { Error = "must contain at least 8 characters" },
            new ValidationErrorDetail() { Error = "must contain a digit" },
            new ValidationErrorDetail() { Error = "must contain upper case letter" },
            new ValidationErrorDetail() { Error = "must contain lower case letter" },
        };
        Assert.That(actual["Password"], Is.EqualTo(expectedPassword).AsCollection);
    }

}
