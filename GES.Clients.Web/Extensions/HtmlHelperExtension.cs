using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GES.Clients.Web.Extensions
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// Determines whether the specified controller is selected.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static string IsSelected(this HtmlHelper html, string queryString = "", string action = null, string controller = null, string currentQueryString = "", string currentAction = null, string currentController = null)
        {
            const string cssClass = "active";
            if (currentAction == null)
                currentAction = (string)html.ViewContext.RouteData.Values["action"];
            if (currentController == null)
                currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            bool actionCondition = false;
            if (String.IsNullOrEmpty(action))
            {
                actionCondition = true;
            }
            else
            {
                if (action.Contains("]["))
                {
                    currentAction = $"[{currentAction}]";
                    actionCondition = action.Contains(currentAction);
                }
                else
                {
                    actionCondition = action == currentAction;
                }
            }

            return controller == currentController && actionCondition && queryString == currentQueryString ?
                cssClass : String.Empty;
        }
        
        

        public static string GetPlainTextFromHtml(this HtmlHelper helper, string htmlString)
        {
            const string acceptable = "strong|em";
            const string htmlTagPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = Regex.Replace(htmlString, Environment.NewLine, "<br/>", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }
    }
}