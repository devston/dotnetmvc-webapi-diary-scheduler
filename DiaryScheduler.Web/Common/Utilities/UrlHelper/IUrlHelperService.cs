namespace DiaryScheduler.Web.Common.Utilities.UrlHelper
{
    /// <summary>
    /// The interface for the url helper service.
    /// </summary>
    public interface IUrlHelperService
    {
        /// <summary>
        /// Get the <see cref="System.Web.Mvc.UrlHelper"/>.
        /// </summary>
        /// <returns>The <see cref="System.Web.Mvc.UrlHelper"/>.</returns>
        System.Web.Mvc.UrlHelper GetUrlHelper();
    }
}
