using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MultiLingualSearchFieldAttribute : BaseMultiLingualAttribute
{
    public override string Name => "multilingual_search";

    public override IProperty GetElasticProperty()
    {
        return new TextProperty() { Analyzer = "standard" };
    }
}
