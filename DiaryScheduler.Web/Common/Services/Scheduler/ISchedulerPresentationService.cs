using DiaryScheduler.Web.Models.Scheduler;
using System;

namespace DiaryScheduler.Web.Common.Services.Scheduler
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
        /// Create the edit variant of the <see cref="SchedulerModifyViewModel"/>.
        /// </summary>
        /// <param name="id">The calendar entry id.</param>
        /// <returns>The <see cref="SchedulerModifyViewModel"/>.</returns>
        SchedulerModifyViewModel CreateSchedulerEditViewModel(Guid id);

        /// <summary>
        /// Create a calendar event.
        /// </summary>
        /// <param name="eventVm">The calendar entry to event.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The event id.</returns>
        Guid CreateCalendarEvent(CalendarEventViewModel eventVm, string userId);

        /// <summary>
        /// Update a calendar event.
        /// </summary>
        /// <param name="eventVm">The event to update.</param>
        void UpdateCalendarEvent(CalendarEventViewModel eventVm);

        /// <summary>
        /// Delete a calendar event.
        /// </summary>
        /// <param name="id">The calendar event id.</param>
        void DeleteCalendarEvent(Guid id);
    }
}
