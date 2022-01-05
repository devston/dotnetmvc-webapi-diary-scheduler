using DiaryScheduler.Presentation.Models.Scheduler;
using Microsoft.AspNetCore.Routing;

namespace DiaryScheduler.Presentation.Web.Common.Services.Scheduler
{
    /// <summary>
    /// The implementation of the <see cref="ISchedulerUrlGenerationService"/>.
    /// </summary>
    public class SchedulerUrlGenerationService : ISchedulerUrlGenerationService
    {
        private readonly LinkGenerator _linkGenerator;

        public SchedulerUrlGenerationService(
            LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public SchedulerIndexViewModel SetIndexUrls(SchedulerIndexViewModel vm)
        {
            vm.CreateEventUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.Create), "Scheduler", null);
            vm.CreateEventMoreOptionsUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.CreateMoreOptions), "Scheduler", new { title = "title_placeholder", start = "start_placeholder", end = "end_placeholder" });
            vm.EditEventUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.Edit), "Scheduler", new { id = "id_placeholder" });
            vm.CalendarSourceUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.CalendarEvents), "Scheduler", null);
            vm.PostCreateEventUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.CreateEvent), "Scheduler", null);
            vm.ExportIcalUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.ExportEventsToIcal), "Scheduler", new { start = "start_placeholder", end = "end_placeholder" });
            return vm;
        }

        public SchedulerModifyViewModel SetCreateUrls(SchedulerModifyViewModel vm)
        {
            vm.SaveUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.CreateEvent), "Scheduler", null);
            return vm;
        }

        public SchedulerModifyViewModel SetEditUrls(SchedulerModifyViewModel vm)
        {
            vm.SaveUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.EditEvent), "Scheduler", null);
            vm.DeleteUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.DeleteEvent), "Scheduler", null);
            vm.ExportUrl = _linkGenerator.GetPathByAction(nameof(Controllers.SchedulerController.ExportEventToIcal), "Scheduler", null);
            return vm;
        }
    }
}
