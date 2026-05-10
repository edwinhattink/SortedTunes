using SortedTunes.Application.Common.Models;

namespace SortedTunes.Infrastructure.Models;

public class ErrorResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public IDictionary<string, ValidationErrorDetail[]>? Errors { get; set; }
}
