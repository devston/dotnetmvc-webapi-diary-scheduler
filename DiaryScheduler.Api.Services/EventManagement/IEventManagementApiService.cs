using DiaryScheduler.Presentation.Models.Scheduler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Services.EventManagement
{
    /// <summary>
    /// The interface for the event management api service.
    /// </summary>
    public interface IEventManagementApiService
    {
        /// <summary>
        /// Get a collection of calendar events between a date range asynchronously.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>A collection of <see cref="CalendarEventViewModel"/>.</returns>
        Task<List<CalendarEventViewModel>> GetCalendarEventsBetweenDateRangeAsync(DateTime start, DateTime end);

        /// <summary>
        /// Get a calendar event by its id asynchronously.
        /// </summary>
        /// <param name="id">The calendar event id.</param>
        /// <returns>The <see cref="CalendarEventViewModel"/>.</returns>
        Task<CalendarEventViewModel> GetCalendarEventByIdAsync(Guid id);

        /// <summary>
        /// Create a calendar event asynchronously.
        /// </summary>
        /// <param name="eventVm">The event to create.</param>
        /// <returns>The created event id.</returns>
        Task<Guid> CreateCalendarEventAsync(CalendarEventViewModel eventVm);

        /// <summary>
        /// Update a calendar event asynchronously.
        /// </summary>
        /// <param name="eventVm">The event to update.</param>
        /// <returns>The return message.</returns>
        Task<string> UpdateCalendarEventAsync(CalendarEventViewModel eventVm);

        /// <summary>
        /// Delete a calendar event asynchronously.
        /// </summary>
        /// <param name="id">The id of the event to delete.</param>
        /// <returns>The return message.</returns>
        Task<string> DeleteCalendarEventAsync(Guid id);
    }
}
