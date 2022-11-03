using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GES.Clients.Web.Configs;
using GES.Clients.Web.Helpers;
using GES.Inside.Data.Configs;
using Serilog;

namespace GES.Clients.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.Configure();
            AutoMapperDataConfiguration.Configure();
            NGS.Templater.Configuration.Configure(SiteSettings.TemplaterCustomer, SiteSettings.TemplaterLicense);
            JobScheduler.Start();
        }

        public void Application_End()
        {
            Log.CloseAndFlush();
        }
    }
}
