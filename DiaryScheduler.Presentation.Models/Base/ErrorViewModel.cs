namespace DiaryScheduler.Presentation.Models.Base;

/// <summary>
/// The error view model.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request id.
    /// </summary>
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the request id.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
