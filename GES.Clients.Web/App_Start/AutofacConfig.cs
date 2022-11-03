using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using GES.Clients.Web.Modules;

namespace GES.Clients.Web
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new InfrastructureModule());
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}