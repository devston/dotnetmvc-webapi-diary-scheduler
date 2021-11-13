using System.Web;

namespace DiaryScheduler.Web.Common.Utilities.UrlHelper
{
    /// <summary>
    /// The url helper service - used to help generate urls outside of a mvc controller.
    /// </summary>
    public class UrlHelperService : IUrlHelperService
    {
        public System.Web.Mvc.UrlHelper GetUrlHelper()
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper;
        }
    }
}