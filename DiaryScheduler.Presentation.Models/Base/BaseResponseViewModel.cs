namespace DiaryScheduler.Presentation.Models.Base;

/// <summary>
/// The base response view model.
/// </summary>
public class BaseResponseViewModel
{
    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    public int StatusCode { get; set; } = 200;

    /// <summary>
    /// Gets or sets the response message.
    /// </summary>
    public string Message { get; set; } = "Success";
}
