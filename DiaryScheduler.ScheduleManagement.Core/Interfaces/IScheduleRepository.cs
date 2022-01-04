using DiaryScheduler.ScheduleManagement.Core.Models;
using System;
using System.Collections.Generic;

namespace DiaryScheduler.ScheduleManagement.Core.Interfaces
{
    /// <summary>
    /// The interface for the schedule repository.
    /// </summary>
    public interface IScheduleRepository
    {
        #region Gets

        /// <summary>
        /// Return all user calendar events.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="start">Search start date</param>
        /// <param name="end">Search end date</param>
        /// <returns>A collection of <see cref="CalEventDm"/>.</returns>
        List<CalEventDm> GetAllUserEvents(string id, DateTime start, DateTime end);

        /// <summary>
        /// Get a calendar event by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="CalEventDm"/>.</returns>
        CalEventDm GetCalendarEvent(Guid id);

        #endregion

        #region Checks

        /// <summary>
        /// Check if the calendar event exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A value indicating whether the calendar event exists.</returns>
        bool DoesEventExist(Guid id);

        #endregion

        #region Create, update and delete

        /// <summary>
        /// Create a calendar event.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>The created event id.</returns>
        Guid CreateCalendarEvent(CalEventDm entry);

        /// <summary>
        /// Edit an existing calendar event.
        /// </summary>
        /// <param name="entry">The calendar event to edit.</param>
        void EditCalendarEvent(CalEventDm entry);

        /// <summary>
        /// Delete a calendar event.
        /// </summary>
        /// <param name="id">The id of the event to delete.</param>
        void DeleteCalendarEvent(Guid id);

        #endregion
    }
}
