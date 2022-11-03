using GES.Common.Enumeration;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Infrastructure;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Repository.Interfaces;
using Microsoft.AspNet.Identity;

public class CustomAuthorize : AuthorizeAttribute
{
    public string Model { get; set; }
    public ActionEnum Action { get; set; }
    public string FormKey { get; set; }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (!httpContext.Request.IsAuthenticated)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(FormKey))
        {
            var userId = httpContext.User.Identity.GetUserId();
            GesRefreshDbContext gesRefresh = new GesRefreshDbContext();
            IGesLogger log = new SerilogAdapter(LoggerHelper.CreateJobLogger());

            IGesRolesRepository rolesRepository = new GesRolesRepository(gesRefresh, log);

            if (!rolesRepository.CheckPermission(userId, FormKey, (int) Action))
            {
                return false;
            }
        }
        
        base.AuthorizeCore(httpContext);
        return true;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {

        if (filterContext.HttpContext.Request.IsAjaxRequest())
        {
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.Result = new HttpStatusCodeResult(403, "Sorry, you do not have the required permission to perform this action.");
        }
        else
        {
            var viewResult = new ViewResult();

            viewResult.ViewName = "~/Views/Error/Unauthorized.cshtml";
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.Result = viewResult;
        }

        //   base.HandleUnauthorizedRequest(filterContext);
    }
}