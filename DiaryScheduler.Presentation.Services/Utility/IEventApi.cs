using DiaryScheduler.Presentation.Models.Scheduler;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Utility;

public interface IEventApi
{
    [Get("/api/event-management/events")]
    Task<List<CalendarEventViewModel>> GetEventsBetweenDateRangeAsync(DateTime start, DateTime end);

    [Get("/api/event-management/events/{id}")]
    Task<CalendarEventViewModel> GetEventByIdAsync(Guid id);

    [Post("/api/event-management/events")]
    Task<Guid> CreateEventAsync([Body] CalendarEventViewModel calendarEvent);

    [Put("/api/event-management/events/{id}")]
    Task<string> UpdateEventAsync(Guid id, [Body] CalendarEventViewModel calendarEvent);

    [Delete("/api/event-management/events/{id}")]
    Task<string> DeleteEventAsync(Guid id);
}
