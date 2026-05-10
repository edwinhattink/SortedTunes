using System.Reflection;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using SortedTunes.Application.Elasticsearch.Exceptions;
using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Application.Elasticsearch.Helpers;

public static class MappingHelpers
{
    public static Dictionary<PropertyName, IProperty> GetMappingProperties(Type type)
        => GetMappingProperties(type, []);

    private static Dictionary<PropertyName, IProperty> GetMappingProperties(Type type, HashSet<Type> visited)
    {
        if (!visited.Add(type))
            return [];

        var properties = type.GetProperties();
        var mappingProperties = new Dictionary<PropertyName, IProperty>();

        foreach (var property in properties)
        {
            if (property.IsDefined(typeof(IgnoreFieldAttribute), false))
                continue;

            var propertyName = char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
            var propertyType = property.PropertyType;

            var multiLingualAttributes = property.GetCustomAttributes<BaseMultiLingualAttribute>(false);
            var multiLingualField = MapMultilLingualField(multiLingualAttributes);
            if (multiLingualField != null)
            {
                mappingProperties[propertyName] = multiLingualField;
                continue;
            }

            if (property.IsDefined(typeof(NestedFieldAttribute), false))
            {
                // --- CHANGE: also unwrap IEnumerable<T>, not just arrays ---
                if (TryGetEnumerableElementType(propertyType, out var elem))
                {
                    propertyType = elem!;
                }
                else if (propertyType.IsArray)
                {
                    propertyType = propertyType.GetElementType()
                        ?? throw new ElasticsearchMappingException(propertyName, propertyType.Name);
                }

                mappingProperties[propertyName] = new ObjectProperty
                {
                    Properties = new Properties(GetMappingProperties(propertyType, visited))
                };
                continue;
            }

            var elasticTextProperties = CreateTextFields(property.GetCustomAttributes(false).OfType<BaseAttribute>());
            if (elasticTextProperties.Count > 0)
            {
                mappingProperties[propertyName] = new TextProperty
                {
                    Fields = new Properties(elasticTextProperties)
                };
            }
            else
            {
                var elasticsearchType = GetElasticsearchType(propertyName, propertyType);
                mappingProperties[propertyName] = elasticsearchType;
            }
        }

        // allow same type to appear again in sibling branches
        visited.Remove(type);
        return mappingProperties;
    }

    public static IDictionary<PropertyName, IProperty> CreateTextFields(IEnumerable<BaseAttribute> attributes)
    {
        var elasticTextProperties = new Dictionary<PropertyName, IProperty>();
        foreach (var attribute in attributes)
        {
            elasticTextProperties.Add(attribute.Name, attribute.GetElasticProperty());
        }
        return elasticTextProperties;
    }

    public static IProperty? MapMultilLingualField(IEnumerable<BaseMultiLingualAttribute> attributes)
    {
        if (attributes.Any())
        {
            var multiLingualElasticTextProperties = CreateTextFields(attributes);
            var propertiesPerLanguage = new Dictionary<PropertyName, IProperty>();
            //foreach (BuynamicsCultureCode bcc in Enum.GetValues<BuynamicsCultureCode>())
            //{
            //    propertiesPerLanguage.Add(bcc.ToString(), new TextProperty() { Fields = new Properties(multiLingualElasticTextProperties) });
            //}

            return new ObjectProperty()
            {
                Properties = new Properties(propertiesPerLanguage)
            };
        }

        return null;
    }

    public static IProperty GetElasticsearchType(string propertyName, Type propertyType)
    {
        if (IsNullable(propertyType, out var underlying))
            propertyType = underlying!;

        if (TryGetEnumerableElementType(propertyType, out var elementType))
            propertyType = elementType!;

        switch (Type.GetTypeCode(propertyType))
        {
            case TypeCode.Int32:
                return new IntegerNumberProperty();
            case TypeCode.Int64:
                return new LongNumberProperty();
            case TypeCode.Double:
                return new DoubleNumberProperty();
            case TypeCode.Single:
                return new FloatNumberProperty();
            case TypeCode.Decimal:
                return new ScaledFloatNumberProperty
                {
                    ScalingFactor = 100 // example: 2 decimal places
                };
            case TypeCode.Boolean:
                return new BooleanProperty();
            case TypeCode.DateTime:
                return new DateProperty();
            case TypeCode.String:
                return new KeywordProperty();
        }

        if (propertyType == typeof(DateTimeOffset))
            return new DateProperty();

        if (propertyType.IsEnum)
            return new KeywordProperty();

        throw new ElasticsearchMappingException(propertyName, propertyType.Name.ToLowerInvariant());
    }

    private static bool IsNullable(Type type, out Type? underlying)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            underlying = Nullable.GetUnderlyingType(type)!;
            return true;
        }
        underlying = null;
        return false;
    }

    private static bool TryGetEnumerableElementType(Type type, out Type? elementType)
    {
        elementType = null;
        if (type == typeof(string))
            return false;

        if (type.IsArray)
        {
            elementType = type.GetElementType();
            return elementType != null && elementType != typeof(char);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            elementType = type.GetGenericArguments()[0];
            return true;
        }

        var ienum = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        if (ienum != null)
        {
            elementType = ienum.GetGenericArguments()[0];
            return true;
        }

        return false;
    }
}
