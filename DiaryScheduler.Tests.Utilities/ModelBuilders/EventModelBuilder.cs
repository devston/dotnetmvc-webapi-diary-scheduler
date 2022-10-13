using DiaryScheduler.Presentation.Models.Scheduler;
using System;

namespace DiaryScheduler.Tests.Utilities.ModelBuilders;

public static class EventModelBuilder
{
    public static CalendarEventViewModel CreateValidCalendarEventViewModel()
    {
        var eventVm = new CalendarEventViewModel();
        eventVm.CalendarEventId = Guid.NewGuid();
        eventVm.Title = "My test event";
        eventVm.DateFrom = DateTime.UtcNow;
        eventVm.DateTo = DateTime.UtcNow.AddDays(1);
        eventVm.Description = "This is an event used for testing.";
        return eventVm;
    }
}
