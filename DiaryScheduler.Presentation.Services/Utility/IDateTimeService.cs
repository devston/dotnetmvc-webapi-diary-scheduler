using System;

namespace DiaryScheduler.Presentation.Services.Utility;

/// <summary>
/// The interface for the date time utility service.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// A wrapper around DateTime.Now to assist with unit testing.
    /// </summary>
    /// <returns></returns>
    DateTime GetDateTimeNow();

    /// <summary>
    /// A wrapper around DateTime.UtcNow to assist with unit testing.
    /// </summary>
    /// <returns></returns>
    DateTime GetDateTimeUtcNow();
}
