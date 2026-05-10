using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class SearchFieldAttribute : BaseAttribute
{
    public override string Name => "search";

    public override IProperty GetElasticProperty()
    {
        return new TextProperty() { Analyzer = "standard" };
    }
}
