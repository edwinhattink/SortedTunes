using Elastic.Clients.Elasticsearch.Mapping;
using SortedTunes.Application.Elasticsearch.Exceptions;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class IgnoreFieldAttribute : BaseAttribute
{
    public override string Name => "ignore";

    public override IProperty GetElasticProperty()
    {
        throw new ElasticsearchMappingException("ignore", "ignore");
    }
}
