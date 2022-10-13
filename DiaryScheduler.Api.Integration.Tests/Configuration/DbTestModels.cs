using DiaryScheduler.Data.Models;
using System;

namespace DiaryScheduler.Api.Integration.Tests.Configuration;

public static class DbTestModels
{
    public static CalendarEvent Event1 => new CalendarEvent
    {
        CalendarEventId = new Guid("db87f663-4a99-4ec1-b940-49b27cc4ee46"),
        DateFrom = DateTime.UtcNow,
        DateTo = DateTime.UtcNow.AddDays(1),
        Title = "Test Event",
        Description = "This is a test event"
    };
}
