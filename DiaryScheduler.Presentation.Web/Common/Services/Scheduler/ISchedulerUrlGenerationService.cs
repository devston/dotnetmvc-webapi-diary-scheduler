using DiaryScheduler.Presentation.Models.Scheduler;

namespace DiaryScheduler.Presentation.Web.Common.Services.Scheduler;

/// <summary>
/// The interface for the scheduler url generation service.
/// This is a temporary service until we have a way to generate urls in the presentation service layer.
/// </summary>
public interface ISchedulerUrlGenerationService
{
    /// <summary>
    /// Set the urls on the <see cref="SchedulerIndexViewModel"/>.
    /// </summary>
    /// <param name="vm">The scheduler index view model.</param>
    /// <returns>The configured <see cref="SchedulerIndexViewModel"/>.</returns>
    SchedulerIndexViewModel SetIndexUrls(SchedulerIndexViewModel vm);

    /// <summary>
    /// Set the create urls on the <see cref="SchedulerModifyViewModel"/>.
    /// </summary>
    /// <param name="vm">The scheduler modify view model.</param>
    /// <returns>The configured <see cref="SchedulerModifyViewModel"/>.</returns>
    SchedulerModifyViewModel SetCreateUrls(SchedulerModifyViewModel vm);

    /// <summary>
    /// Set the edit urls on the <see cref="SchedulerModifyViewModel"/>.
    /// </summary>
    /// <param name="vm">The scheduler modify view model.</param>
    /// <returns>The configured <see cref="SchedulerModifyViewModel"/>.</returns>
    SchedulerModifyViewModel SetEditUrls(SchedulerModifyViewModel vm);
}
