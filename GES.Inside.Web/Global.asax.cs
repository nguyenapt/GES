using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GES.Inside.Web.Configs;
using GES.Inside.Web.Helpers;
using NGS.Templater;

namespace GES.Inside.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IDocumentFactory TemplaterFactory = NGS.Templater.Configuration.Configure(SiteSettings.TemplaterCustomer, SiteSettings.TemplaterLicense);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.Configure();
            AutoMapperConfiguration.Configure();
            JobScheduler.Start();
        }
    }
}
