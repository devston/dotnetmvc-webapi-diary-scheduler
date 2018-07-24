using System.Web.Mvc;

namespace DiaryScheduler.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}