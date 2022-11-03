using GES.Common.Exceptions;
using GES.Common.Logging;
using GES.Inside.Data.Models;
using System;
using System.Web.Mvc;

namespace GES.Clients.Web.Controllers
{
    public abstract class GesControllerBase : Controller
    {
        protected readonly IGesLogger Logger;

        protected GesControllerBase(IGesLogger logger)
        {
            this.Logger = logger;
        }
        
        protected virtual string GetBaseUrl()
        {
            if (Request.Url != null)
                return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            return "";
        }

        protected virtual void SafeExecute(Action action, string errorLogTemplate, params object[] properties)
        {
            try
            {
                action();
            }
            catch(GesServiceException ex)
            {
                Logger.Error(ex, errorLogTemplate, properties);

                throw;
            }
        }

        protected virtual T SafeExecute<T>(Func<T> func, string errorLogTemplate, params object[] properties)
        {
            try
            {
                return func();
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, errorLogTemplate, properties);

                throw;
            }
        }

        protected virtual JsonResult Json<T>(PaginatedResults<T> result, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return Json(new
            {
                total = result.TotalPages,
                page = result.PageIndex,
                records = result.TotalResults,
                rows = result.Results,
                message = result.Message
            }, behavior);
        }
    }

    [Authorize]
    public abstract class AuthorizedGesControllerBase : GesControllerBase
    {
        protected AuthorizedGesControllerBase(IGesLogger logger) : base(logger)
        {
        }
    }
}