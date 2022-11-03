using System.Reflection;
using Autofac;
using GES.Clients.Web.Configs;
using GES.Common.Services;
using GES.Common.Services.Interface;

namespace GES.Clients.Web.Modules
{
    public class ServiceModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("GES.Inside.Data"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType<MailService>()
                   .As<IMailService>()
                   .WithParameter(new NamedParameter("apiKey", SiteSettings.MandrillApiKey))
                   .WithParameter(new NamedParameter("mailPrefix", SiteSettings.MailPrefix))
                   .WithParameter(new NamedParameter("mailNoReply", SiteSettings.MailNoReply))
                   .WithParameter(new NamedParameter("mailNoReplyName", SiteSettings.MailNoReplyName))
                   .WithParameter(new NamedParameter("contactEmail", SiteSettings.MailContact))
                   .WithParameter(new NamedParameter("companyName", SiteSettings.MailCompanyName));

            builder.RegisterType<ApplicationSettingsService>()
                .As<IApplicationSettingsService>()
                .SingleInstance();
        }
    }
}