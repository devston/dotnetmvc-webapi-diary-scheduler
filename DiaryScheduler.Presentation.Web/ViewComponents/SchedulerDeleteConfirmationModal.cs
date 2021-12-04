using Microsoft.AspNetCore.Mvc;

namespace DiaryScheduler.Presentation.Web.ViewComponents
{
    [ViewComponent]
    public class SchedulerDeleteConfirmationModal : ViewComponent
    {
        public IViewComponentResult Invoke(int maxPriority, bool isDone)
        {
            return View();
        }
    }
}
