using Autofac;
using GES.Clients.Web.Infrastructure.Rendering;
using GES.Common.Enumeration;
using GES.Common.Factories;
using GES.Common.Services.Interface;
using GES.Clients.Web.Infrastructure.PdfExport;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Repository.Interfaces;
using Serilog;
using AutofacSerilogIntegration;
using GES.Common.Logging;
using System.Diagnostics;
using System.Web;
using System.IO;
using GES.Clients.Web.PhantomJs;

namespace GES.Clients.Web.Modules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRender(builder);

            builder.RegisterGeneric(typeof(DefaultAutofacServiceFactory<>))
                .As(typeof(IServiceFactory<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<RotativaPdfExportProcessor>()
                .As<IExportProcessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StoredProcedureRunner>()
                .As<IStoredProcedureRunner>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PhantomJsRunner>()
                .As<IPhantomJsRunner>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PdfFilesMerger>()
                .As<IPdfFilesMerger>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PdfFileDownload>()
                .As<IPdfFileDownload>()
                .InstancePerLifetimeScope();

            RegisterLogger(builder);
        }

        private void RegisterRender(ContainerBuilder builder)
        {
            builder.RegisterType<BcDisengageBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BcDisengage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BcEngageBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BcEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BcEvaluateBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BcEvaluate.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BcResolvedBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BcResolved.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BcArchivedBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BcArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrEmeBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrEmeEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrEmeBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrEmeArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrGovBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrGovEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrGovBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrGovArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrGovBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrGovResolved.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrCarbonRiskBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrCarbonRiskEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrCarbonRiskBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrCarbonRiskResolved.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<SrCarbonRiskBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.SrCarbonRiskArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<StConfirmedBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.StConfirmed.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<StIndicationOfViolationBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.StIndicationOfViolation.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<StAlertBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.StAlert.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<StResolvedBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.StResolved.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<StArchivedBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.StArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BpBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BpEngage.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<BpBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.BpArchived.ToString()))
                .InstancePerLifetimeScope();

            builder.RegisterType<GenerationBuilder>()
                .As<CaseProfileBuilder>()
                .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.GenerationType.ToString()))
                .InstancePerLifetimeScope();

            //builder.RegisterType<FullAttributeBuilder>()
            //    .As<CaseProfileBuilder>()
            //    .WithMetadata<Metadata>(m => m.For(am => am.Key, GesCaseReportType.GenerationType.ToString()))
            //    .InstancePerLifetimeScope();

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