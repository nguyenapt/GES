using System.Web.Mvc;

namespace GES.Clients.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());

            // OutputCache Globaly 
            //filters.Add(new OutputCacheAttribute
            //{
            //    Duration = 24 * 60 * 60,
            //    VaryByParam = "*"
            //});
        }
    }
}
