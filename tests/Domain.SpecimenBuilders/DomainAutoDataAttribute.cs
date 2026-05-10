using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit4;
using SortedTunes.Domain.SpecimenBuilders.Entities;

namespace SortedTunes.Domain.SpecimenBuilders;

[AttributeUsage(AttributeTargets.Method)]
public class DomainAutoDataAttribute : AutoDataAttribute
{
    public DomainAutoDataAttribute() : base(CreateFixture) { }

    public DomainAutoDataAttribute(Func<IFixture> func) : base(func) { }

    private static IFixture? s_fixture;
    public static IFixture CreateFixture()
    {
        if (s_fixture != null) return s_fixture;
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

        fixture.Customizations.Add(new DateOnlySpecimenBuilder());

        fixture.Customizations.Add(new AlbumSpecimenBuilder());
        fixture.Customizations.Add(new ArtistSpecimenBuilder());
        fixture.Customizations.Add(new ContributionSpecimenBuilder());
        fixture.Customizations.Add(new DiscSpecimenBuilder());
        fixture.Customizations.Add(new DiscContributionSpecimenBuilder());
        fixture.Customizations.Add(new GenreSpecimenBuilder());
        fixture.Customizations.Add(new TrackSpecimenBuilder());

        s_fixture = fixture;
        return fixture;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DomainInlineAutoDataAttribute : InlineAutoDataAttribute
{
    public DomainInlineAutoDataAttribute(params object[] arguments) : base(DomainAutoDataAttribute.CreateFixture, arguments) { }

    public DomainInlineAutoDataAttribute(Func<IFixture> func, params object[] arguments) : base(func, arguments) { }
}

