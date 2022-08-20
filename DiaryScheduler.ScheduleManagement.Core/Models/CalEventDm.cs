using System;

namespace DiaryScheduler.ScheduleManagement.Core.Models;

/// <summary>
/// The calendar event domain model.
/// </summary>
public class CalEventDm
{
    /// <summary>
    /// Gets or sets the calendar entry id.
    /// </summary>
    public Guid CalendarEntryId { get; set; }

    /// <summary>
    /// Gets or sets the from date.
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Gets or sets the to date.
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entry is an all day event.
    /// </summary>
    public bool AllDay { get; set; }

    /// <summary>
    /// Gets or sets the calendar event title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the calendar event description.
    /// </summary>
    public string Description { get; set; }
}
