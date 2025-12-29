namespace SortedTunes.Domain.SpecimenBuilders;
public class DateOnlySpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(DateOnly))
        {
            var dateTime = context.Create<DateTime>();
            return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        return new NoSpecimen();
    }
}
