using System;
using System.Web.Mvc.Html;
using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static System.Web.Mvc.MvcHtmlString MenuLink(this System.Web.Mvc.HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string cssClass)
         {
             string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
             string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
             
             TagBuilder tb = new TagBuilder("li");
             if (string.Equals(currentAction, actionName, StringComparison.OrdinalIgnoreCase) && string.Equals(currentController, controllerName, StringComparison.OrdinalIgnoreCase))
             {
                 tb.AddCssClass(cssClass);
             }

            // System.Web.Mvc.MvcHtmlString link = htmlHelper.ActionLink(linkText, actionName, controllerName);
            tb.InnerHtml += htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString();
            return new System.Web.Mvc.MvcHtmlString(tb.ToString());
         }
    }
}