using iBudget.AutomatedTest.Person.Shared;

namespace iBudget.AutomatedTest.Person;

public class PersonCreationDateTest
{
    private static readonly TimeZoneInfo DatabaseTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    [Fact]
    public void ConvertsUtcInstantToDatabaseDate()
    {
        var instant = new DateTimeOffset(2026, 10, 15, 2, 6, 22, TimeSpan.Zero);

        var dates = PersonCreationDate.GetExpectedDates(instant, instant, DatabaseTimeZone);

        Assert.Equal([new DateTime(2026, 10, 14)], dates);
    }

    [Fact]
    public void IncludesBothDatesWhenCreationSpansDatabaseMidnight()
    {
        var before = new DateTimeOffset(2026, 10, 15, 2, 59, 59, TimeSpan.Zero);

        var dates = PersonCreationDate.GetExpectedDates(before, before.AddSeconds(2), DatabaseTimeZone);

        Assert.Equal([new DateTime(2026, 10, 14), new DateTime(2026, 10, 15)], dates);
    }

    [Fact]
    public void UsesProvidedDatabaseTimeZone()
    {
        var instant = new DateTimeOffset(2026, 10, 15, 2, 6, 22, TimeSpan.Zero);

        var utcDates = PersonCreationDate.GetExpectedDates(instant, instant, TimeZoneInfo.Utc);
        var saoPauloDates = PersonCreationDate.GetExpectedDates(instant, instant, DatabaseTimeZone);

        Assert.Equal([new DateTime(2026, 10, 15)], utcDates);
        Assert.Equal([new DateTime(2026, 10, 14)], saoPauloDates);
    }
}
