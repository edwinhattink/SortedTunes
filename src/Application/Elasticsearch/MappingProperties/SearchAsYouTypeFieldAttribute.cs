using Elastic.Clients.Elasticsearch.Mapping;

namespace SortedTunes.Application.Elasticsearch.MappingProperties;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class SearchAsYouTypeFieldAttribute : BaseAttribute
{
    public override string Name => "search_as_you_type";

    public override IProperty GetElasticProperty()
    {
        return new SearchAsYouTypeProperty();
    }
}
