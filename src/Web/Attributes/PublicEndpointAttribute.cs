namespace SortedTunes.Web.Attributes;

/// <summary>
/// Specifies the controller to be in the public API documentation
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PublicEndpointAttribute : Attribute
{
}
