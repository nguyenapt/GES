using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using Autofac;
using AutofacSerilogIntegration;
using GES.Common.Logging;
using GES.Common.Services;
using GES.Common.Services.Interface;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services;
using GES.Inside.Data.Services.Interfaces;
using Serilog;

namespace GES.Inside.Web.Modules
{
    public class ServiceModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("GES.Inside.Data"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType<GesDocumentService>()
                .As<IGesDocumentService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<ApplicationSettingsService>()
                .As<IApplicationSettingsService>();

            builder.RegisterType<StoredProcedureRunner>()
                .As<IStoredProcedureRunner>()
                .InstancePerLifetimeScope();

            RegisterLogger(builder);
        }

        private void RegisterLogger(ContainerBuilder builder)
        {
            var log = Path.Combine(HttpContext.Current.Server.MapPath("logs"), @"log-{Date}.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(log, shared: true)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            builder.RegisterLogger();

            builder.RegisterType<SerilogAdapter>()
                .As<IGesLogger>()
                .SingleInstance();
        }
    }
}