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
        /// Return all user calendar entries.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="start">Search start date</param>
        /// <param name="end">Search end date</param>
        /// <returns>A collection of <see cref="CalEventDm"/>.</returns>
        List<CalEventDm> GetAllUserEntries(string id, DateTime start, DateTime end);

        /// <summary>
        /// Get a calendar entry by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="CalEventDm"/>.</returns>
        CalEventDm GetCalendarEntry(Guid id);

        #endregion

        #region Checks

        /// <summary>
        /// Check if the calendar entry exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A value indicating whether the calendar entry exists.</returns>
        bool DoesCalEntryExist(Guid id);

        #endregion

        #region Create, update and delete

        /// <summary>
        /// Create a calendar entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>Created entry id</returns>
        Guid CreateCalendarEntry(CalEventDm entry);

        /// <summary>
        /// Edit an existing calendar entry.
        /// </summary>
        /// <param name="entry">The calendar entry to edit.</param>
        void EditCalendarEntry(CalEventDm entry);

        /// <summary>
        /// Delete a calendar entry.
        /// </summary>
        /// <param name="id">The id of the entry to delete.</param>
        void DeleteCalendarEntry(Guid id);

        #endregion
    }
}
