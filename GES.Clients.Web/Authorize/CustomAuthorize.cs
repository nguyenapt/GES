using System.Web;
using System.Web.Mvc;
using GES.Common.Enumeration;

namespace GES.Clients.Web.Authorize
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public string Model { get; set; }
        public ActionEnum Action { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
                return false;


            //this.Roles


            //var rolesProvider = System.Web.Security.Roles.Providers["TimeShareRoleProvider"];

            //string[] roles = rolesProvider.GetRolesForUser(httpContext.User.Identity.Name);

            //if (roles.Contains(Website.Roles.RegisteredClient, StringComparer.OrdinalIgnoreCase))
            //{
            //    return true;
            //}

            base.AuthorizeCore(httpContext);

            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);


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
}