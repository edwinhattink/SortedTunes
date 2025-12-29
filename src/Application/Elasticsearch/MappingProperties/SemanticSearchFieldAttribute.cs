using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class SemanticSearchFieldAttribute : BaseAttribute
{
    public override string Name => "semantic_search";

    public override IProperty GetElasticProperty()
    {
        return new SemanticTextProperty();
    }
}
