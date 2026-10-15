using System.Globalization;
using Shared;

namespace iBudget.AutomatedTest.Person.Shared;

internal static class PersonCreationDate
{
    // Match the PostgreSQL session timezone that generates CURRENT_TIMESTAMP.
    // Override for environments whose database uses a different timezone.
    internal static TimeZoneInfo DatabaseTimeZone => TimeZoneInfo.FindSystemTimeZoneById(
        Environment.GetEnvironmentVariable("IBUDGET_TEST_DATABASE_TIMEZONE") ?? "America/Sao_Paulo");

    internal static void AssertCreatedBetween(string displayedDate, DateTimeOffset before,
        DateTimeOffset after, TimeZoneInfo timeZone)
    {
        var actual = DateTime.ParseExact(displayedDate, Constants.ptBRDateFormat,
            CultureInfo.InvariantCulture);
        Assert.Contains(actual, GetExpectedDates(before, after, timeZone));
    }

    internal static IReadOnlyList<DateTime> GetExpectedDates(DateTimeOffset before,
        DateTimeOffset after, TimeZoneInfo timeZone)
    {
        var firstDate = TimeZoneInfo.ConvertTime(before, timeZone).Date;
        var lastDate = TimeZoneInfo.ConvertTime(after, timeZone).Date;
        if (lastDate < firstDate)
            throw new ArgumentOutOfRangeException(nameof(after), "Creation window cannot end before it starts.");

        return Enumerable.Range(0, (lastDate - firstDate).Days + 1)
            .Select(day => firstDate.AddDays(day))
            .ToArray();
    }
}
