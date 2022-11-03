using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    /// <summary>
    /// Define the class that help to build the view
    /// </summary>
    public abstract class ViewBuilder
    {
        protected readonly StringBuilder viewBuilder = new StringBuilder();

        protected virtual string GetViewPath(string viewName) => string.Format("~/Views/Shared/{0}.cshtml", viewName);

        /// <summary>
        /// Method to build component of view with explicit view path & model
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="viewName"></param>
        /// <param name="componentModel"></param>
        /// <returns></returns>
        public virtual ViewBuilder BuildComponent(HtmlHelper helper, string viewName, object componentModel)
        {
            if (componentModel != null && helper != null)
                viewBuilder.AppendLine(helper.Partial(GetViewPath(viewName), componentModel).ToHtmlString());

            return this;
        }

        /// <summary>
        /// Method to append tag to the view result
        /// </summary>
        /// <param name="tag">tag with its attributes in string format</param>
        /// <returns></returns>
        public virtual ViewBuilder BuildTag(string tag)
        {
            viewBuilder.AppendLine(tag);
            return this;
        }
        
        /// <summary>
        /// Get the view result after build successfully
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString GetView()
        {
            return MvcHtmlString.Create(viewBuilder.ToString());
        }
    }
}