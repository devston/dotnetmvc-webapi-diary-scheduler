﻿using DiaryScheduler.ScheduleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiaryScheduler.ScheduleManagement.Core.Interfaces;

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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of <see cref="CalEventDm"/>.</returns>
    Task<List<CalEventDm>> GetAllEventsBetweenDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken);

    /// <summary>
    /// Get a calendar event by id asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="CalEventDm"/>.</returns>
    Task<CalEventDm> GetCalendarEventByEventIdAsync(Guid id, CancellationToken cancellationToken);

    #endregion

    #region Checks

    /// <summary>
    /// Check if the calendar event exists asynchronously.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A value indicating whether the calendar event exists.</returns>
    Task<bool> DoesEventExistAsync(Guid id, CancellationToken cancellationToken);

    #endregion

    #region Create, update and delete

    /// <summary>
    /// Create a calendar event asynchronously.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created event id.</returns>
    Task<Guid> CreateCalendarEventAsync(CalEventDm entry, CancellationToken cancellationToken);

    /// <summary>
    /// Edit an existing calendar event asynchronously.
    /// </summary>
    /// <param name="entry">The calendar event to edit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task EditCalendarEventAsync(CalEventDm entry, CancellationToken cancellationToken);

    /// <summary>
    /// Delete a calendar event asynchronously.
    /// </summary>
    /// <param name="id">The id of the event to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteCalendarEventAsync(Guid id, CancellationToken cancellationToken);

    #endregion
}
