namespace SortedTunes.Application.Elasticsearch.Exceptions;

public class ElasticsearchMappingException(string propertyName, string propertyType)
    : Exception($"Mapping exception, cannot map property: {propertyName}, type: {propertyType}")
{
}
