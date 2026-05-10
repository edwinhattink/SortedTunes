using SortedTunes.Web.Attributes;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace SortedTunes.Web.Filters;

public class SwaggerPublicEndpointFilter : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if the endpoint's declaring type (the class) has the PublicEndpointAttribute.
        var hasAttribute = context.MethodInfo?.DeclaringType?
            .GetCustomAttributes(typeof(PublicEndpointAttribute), inherit: false)
            .Length > 0;
        return hasAttribute;
    }
}
