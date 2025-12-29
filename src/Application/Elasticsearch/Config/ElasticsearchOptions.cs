using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using SortedTunes.Application.Elasticsearch.Exceptions;

namespace SortedTunes.Application.Elasticsearch.Config;

public sealed class ElasticsearchOptions
{
    // Cloud Inlog gegevens
    public string? CloudId { get; set; }
    public string? ApiKey { get; set; }

    // Lokale Inlog gegevens
    public string? Url { get; set; }
    public string? Fingerprint { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public ElasticsearchClient CreateClient()
    {
        if (CloudId != null && ApiKey != null)
        {
            return new ElasticsearchClient(CloudId, new ApiKey(ApiKey));
        }

        if (Url != null && ApiKey != null)
        {
            var settings = new ElasticsearchClientSettings(new Uri(Url))
                .Authentication(new ApiKey(ApiKey));
            return new ElasticsearchClient(settings);
        }

        if (Url != null &&
            Fingerprint != null &&
            Username != null &&
            Password != null)
        {
            var settings = new ElasticsearchClientSettings(new Uri(Url))
                .CertificateFingerprint(Fingerprint)
                .Authentication(new BasicAuthentication(Username, Password));
            return new ElasticsearchClient(settings);
        }

        throw new ElasticsearchConfigException();
    }
}
