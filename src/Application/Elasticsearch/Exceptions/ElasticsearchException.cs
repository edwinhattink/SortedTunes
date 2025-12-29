using Elastic.Transport.Products.Elasticsearch;

namespace SortedTunes.Application.Elasticsearch.Exceptions;

public class ElasticsearchException(ElasticsearchResponse response)
    : Exception(response.ElasticsearchServerError?.Error.Reason ?? "Unknown error")
{
}

