using DiaryScheduler.Presentation.Models.Scheduler;
using System;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Scheduler
{
    /// <summary>
    /// The interface for the scheduler presentation service.
    /// </summary>
    public interface ISchedulerPresentationService
    {
        /// <summary>
        /// Create the <see cref="SchedulerIndexViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SchedulerIndexViewModel"/>.</returns>
        SchedulerIndexViewModel CreateSchedulerIndexViewModel();

        /// <summary>
        /// Create the create variant of the <see cref="SchedulerModifyViewModel"/>.
        /// </summary>
        /// <returns>The <see cref="SchedulerModifyViewModel"/>.</returns>
        SchedulerModifyViewModel CreateSchedulerCreateViewModel();

        /// <summary>
        /// Create the create variant of the <see cref="SchedulerModifyViewModel"/>.
        /// </summary>
        /// <param name="title">The event title.</param>
        /// <param name="start">The event start date.</param>
        /// <param name="end">The event end date.</param>
        /// <returns>The <see cref="SchedulerModifyViewModel"/>.</returns>
        SchedulerModifyViewModel CreateSchedulerCreateViewModel(string title, DateTime start, DateTime end);

        /// <summary>
        /// Create the edit variant of the <see cref="SchedulerModifyViewModel"/> asynchronously.
        /// </summary>
        /// <param name="id">The calendar entry id.</param>
        /// <returns>The <see cref="SchedulerModifyViewModel"/>.</returns>
        Task<SchedulerModifyViewModel> CreateSchedulerEditViewModelAsync(Guid id);

        /// <summary>
        /// Get the calendar events between a date range asynchronously.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>The calendar events as an object.</returns>
        Task<object> GetCalendarEventsBetweenDateRangeAsync(DateTime start, DateTime end);

        /// <summary>
        /// Generate an ical file for a calendar event asynchronously.
        /// </summary>
        /// <param name="id">The event id.</param>
        /// <returns>The file data stored in <see cref="CalendarIcalViewModel"/>.</returns>
        Task<CalendarIcalViewModel> GenerateIcalForCalendarEventAsync(Guid id);

        /// <summary>
        /// Generate an ical file between a date range asynchronously.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>The file data stored in <see cref="CalendarIcalViewModel"/>.</returns>
        Task<CalendarIcalViewModel> GenerateIcalBetweenDateRangeAsync(DateTime start, DateTime end);

        /// <summary>
        /// Create a calendar event asynchronously.
        /// </summary>
        /// <param name="eventVm">The calendar entry to event.</param>
        /// <returns>The event id.</returns>
        Task<Guid> CreateCalendarEventAsync(CalendarEventViewModel eventVm);

        /// <summary>
        /// Update a calendar event asynchronously.
        /// </summary>
        /// <param name="eventVm">The event to update.</param>
        Task UpdateCalendarEventAsync(CalendarEventViewModel eventVm);

        /// <summary>
        /// Delete a calendar event asynchronously.
        /// </summary>
        /// <param name="id">The calendar event id.</param>
        Task DeleteCalendarEventAsync(Guid id);
    }
}
