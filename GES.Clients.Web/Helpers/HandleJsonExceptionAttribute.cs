using System.Collections.Generic;
using System.Web.Mvc;

namespace GES.Clients.Web.Helpers
{
    public class HandleJsonExceptionAttribute : ActionFilterAttribute
    {
        // next class example are from the http://www.dotnetcurry.com/ShowArticle.aspx?ID=496
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.Exception == null) return;
            filterContext.HttpContext.Response.StatusCode =
                (int)System.Net.HttpStatusCode.InternalServerError;

            var exInfo = new List<ExceptionInformation>();
            for (var ex = filterContext.Exception; ex != null; ex = ex.InnerException)
            {
                exInfo.Add(new ExceptionInformation
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    StackTrace = ex.StackTrace
                });
            }
            filterContext.Result = new JsonResult { Data = exInfo };
            filterContext.ExceptionHandled = true;
        }
    }

    // to send exceptions as json we define [HandleJsonException] attribute
    public class ExceptionInformation
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}
