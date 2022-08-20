namespace DiaryScheduler.Presentation.Services.Configuration;

/// <summary>
/// This model is used to represent the event api configuration settings.
/// </summary>
public class EventApiConfig
{
    public const string SectionName = "EventApi";

    /// <summary>
    /// Gets or sets the api url.
    /// </summary>
    public string Url { get; set; }
}
