using System.Text.Json.Serialization;

namespace SortedTunes.Application.Common.Models;

public record ValidationErrorDetail
{
    public required string Error { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Data { get; set; }
}
