namespace SortedTunes.Application.Elasticsearch.Exceptions;

public class ElasticsearchConfigException()
    : Exception("Connection options not filled in app settings")
{
}
