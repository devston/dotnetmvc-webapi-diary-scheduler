using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiaryScheduler.Presentation.Models.Scheduler;

/// <summary>
/// The calendar event view model.
/// </summary>
public class CalendarEventViewModel
{
    /// <summary>
    /// Gets or sets the calendar entry id.
    /// </summary>
    public Guid CalendarEventId { get; set; }

    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the from date.
    /// </summary>
    [DisplayName("From date")]
    [Required(ErrorMessage = "Select a {0}")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Gets or sets the to date.
    /// </summary>
    [DisplayName("To date")]
    [Required(ErrorMessage = "Select a {0}")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the event is an all day event.
    /// </summary>
    [DisplayName("All day?")]
    public bool AllDay { get; set; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    [Required(ErrorMessage = "Select a {0}")]
    [StringLength(100, ErrorMessage = "{0} must be {1} characters or less.")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    [StringLength(200, ErrorMessage = "{0} must be {1} characters or less.")]
    public string Description { get; set; }
}
