namespace SortedTunes.Application.SpecimenBuilders;

[AttributeUsage(AttributeTargets.Method)]
public class ApplicationAutoDataAttribute : DomainAutoDataAttribute
{
    public ApplicationAutoDataAttribute() : base(CreateFixture) { }

    public new static IFixture CreateFixture()
    {
        var fixture = DomainAutoDataAttribute.CreateFixture();

        //fixture.Customizations.Add(new AuthUserSpecimenBuilder());

        return fixture;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ApplicationInlineAutoDataAttribute : DomainInlineAutoDataAttribute
{
}
