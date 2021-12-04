using Microsoft.AspNetCore.Mvc;

namespace DiaryScheduler.Presentation.Web.ViewComponents
{
    [ViewComponent]
    public class SchedulerQuickCreateModal : ViewComponent
    {
        public IViewComponentResult Invoke(int maxPriority, bool isDone)
        {
            return View();
        }
    }
}
