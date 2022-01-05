using DiaryScheduler.ScheduleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaryScheduler.ScheduleManagement.Core.Interfaces
{
    /// <summary>
    /// The interface for the schedule repository.
    /// </summary>
    public interface IScheduleRepository
    {
        #region Gets

        /// <summary>
        /// Return all calendar events between a date range asynchronously.
        /// </summary>
        /// <param name="start">Search start date</param>
        /// <param name="end">Search end date</param>
        /// <returns>A collection of <see cref="CalEventDm"/>.</returns>
        Task<List<CalEventDm>> GetAllEventsBetweenDateRangeAsync(DateTime start, DateTime end);

        /// <summary>
        /// Get a calendar event by id asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="CalEventDm"/>.</returns>
        Task<CalEventDm> GetCalendarEventByEventIdAsync(Guid id);

        #endregion

        #region Checks

        /// <summary>
        /// Check if the calendar event exists asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A value indicating whether the calendar event exists.</returns>
        Task<bool> DoesEventExistAsync(Guid id);

        #endregion

        #region Create, update and delete

        /// <summary>
        /// Create a calendar event asynchronously.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>The created event id.</returns>
        Task<Guid> CreateCalendarEventAsync(CalEventDm entry);

        /// <summary>
        /// Edit an existing calendar event asynchronously.
        /// </summary>
        /// <param name="entry">The calendar event to edit.</param>
        Task EditCalendarEventAsync(CalEventDm entry);

        /// <summary>
        /// Delete a calendar event asynchronously.
        /// </summary>
        /// <param name="id">The id of the event to delete.</param>
        Task DeleteCalendarEventAsync(Guid id);

        #endregion
    }
}
