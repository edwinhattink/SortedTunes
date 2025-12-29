using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

public abstract class BaseAttribute : Attribute
{
    public abstract string Name { get; }
    public abstract IProperty GetElasticProperty();
}
