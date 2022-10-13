namespace DiaryScheduler.Presentation.Models.Base;

/// <summary>
/// The validation result view model.
/// </summary>
public class ValidationResultViewModel
{
    /// <summary>
    /// Gets or sets the property name.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string Message { get; set; }
}
