namespace DiaryScheduler.Presentation.Models.Scheduler;

/// <summary>
/// The scheduler modify screen view model.
/// </summary>
public class SchedulerModifyViewModel : CalendarEventViewModel
{
    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    public string PageTitle { get; set; }

    /// <summary>
    /// Gets or sets the save url.
    /// </summary>
    public string SaveUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the delete button.
    /// </summary>
    public bool ShowDeleteBtn { get; set; }

    /// <summary>
    /// Gets or sets the delete url.
    /// </summary>
    public string DeleteUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the export button.
    /// </summary>
    public bool ShowExportBtn { get; set; }

    /// <summary>
    /// Gets or sets the export url.
    /// </summary>
    public string ExportUrl { get; set; }
}
