using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class KeywordFieldAttribute : BaseAttribute
{
    public override string Name => "keyword";

    public override IProperty GetElasticProperty()
    {
        return new KeywordProperty();
    }
}
