namespace DiaryScheduler.Presentation.Models.Scheduler;

/// <summary>
/// A model to store any data for a calendar ical.
/// </summary>
public class CalendarIcalViewModel
{
    /// <summary>
    /// Gets or sets the file data.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// Gets or sets the file content type.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the file name.
    /// </summary>
    public string FileName { get; set; }
}
