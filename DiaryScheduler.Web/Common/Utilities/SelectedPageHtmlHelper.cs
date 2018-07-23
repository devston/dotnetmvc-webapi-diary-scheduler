using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DiaryScheduler.Web.Common.Utilities
{
    public static class SelectedPageHtmlHelper
    {
        /// <summary>
        /// Check if the link is the current page.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers"></param>
        /// <param name="actions"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string IsCurrentPage(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
            {
                viewContext = html.ViewContext.ParentActionViewContext;
            }

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
            {
                actions = currentAction;
            }

            if (String.IsNullOrEmpty(controllers))
            {
                controllers = currentController;
            }

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }
    }
}