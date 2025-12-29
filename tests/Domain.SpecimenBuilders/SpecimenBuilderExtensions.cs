namespace SortedTunes.Domain.SpecimenBuilders;

public static class SpecimenBuilderExtensions
{
    public static float CreateFloat(this ISpecimenContext fixture, int min, int max)
    {
        return fixture.Create<float>() % (max - min + 1) + min;
    }

    public static int CreateInt(this ISpecimenContext fixture, int min, int max)
    {
        return fixture.Create<int>() % (max - min + 1) + min;
    }

    public static decimal CreateDecimal(this ISpecimenContext fixture, int min, int max, int decimals = 2)
    {
        var randomValue = (decimal)fixture.Create<double>(); // Generate a random double

        return Math.Round(
            min + (randomValue % (max - min + 1)),
            decimals,
            MidpointRounding.AwayFromZero
        );
    }
}
