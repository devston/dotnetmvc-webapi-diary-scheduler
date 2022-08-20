using System;

namespace DiaryScheduler.Presentation.Services.Utility;

/// <summary>
/// The implementation of the <see cref="IDateTimeService"/>.
/// </summary>
public class DateTimeService : IDateTimeService
{
    public DateTime GetDateTimeNow()
    {
        return DateTime.Now;
    }

    public DateTime GetDateTimeUtcNow()
    {
        return DateTime.UtcNow;
    }
}
