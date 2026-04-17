using Elastic.Transport.Products.Elasticsearch;

namespace SortedTunes.Application.Elasticsearch.Exceptions;


public class ElasticsearchException : Exception
{
    public string? RequestUrl { get; }
    public int? StatusCode { get; }
    public string? ErrorType { get; }
    public string? DebugInfo { get; }

    public ElasticsearchException(ElasticsearchResponse response)
        : base(BuildErrorMessage(response))
    {
        RequestUrl = response.ApiCallDetails?.Uri?.ToString();
        StatusCode = response.ApiCallDetails?.HttpStatusCode;
        ErrorType = response.ElasticsearchServerError?.Error?.Type;
        DebugInfo = response.DebugInformation; // Use this instead of serializing
    }

    private static string BuildErrorMessage(ElasticsearchResponse response)
    {
        var serverError = response.ElasticsearchServerError?.Error;
        if (serverError != null)
        {
            var message = $"Elasticsearch error: {serverError.Reason ?? "Unknown reason"}";

            if (!string.IsNullOrEmpty(serverError.Type))
            {
                message += $" [Type: {serverError.Type}]";
            }

            // Include caused by information for nested errors
            if (serverError.CausedBy != null)
            {
                message += $"\nCaused by: {serverError.CausedBy.Reason}";
                if (!string.IsNullOrEmpty(serverError.CausedBy.Type))
                {
                    message += $" [Type: {serverError.CausedBy.Type}]";
                }
            }

            return message;
        }

        return response.DebugInformation ?? "Unknown Elasticsearch error";
    }
}
