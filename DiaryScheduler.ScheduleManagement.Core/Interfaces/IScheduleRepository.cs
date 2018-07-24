using DiaryScheduler.ScheduleManagement.Core.Models;
using System;
using System.Collections.Generic;

namespace DiaryScheduler.ScheduleManagement.Core.Interfaces
{
    public interface IScheduleRepository
    {
        #region Gets

        /// <summary>
        /// Return all user calendar entries.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="start">Search start date</param>
        /// <param name="end">Search end date</param>
        /// <returns></returns>
        List<CalEntry> GetAllUserEntries(string id, DateTime start, DateTime end);

        #endregion

        #region Create, update and delete

        /// <summary>
        /// Create a calendar entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>Created entry id</returns>
        Guid CreateCalendarEntry(CalEntry entry);

        #endregion
    }
}
