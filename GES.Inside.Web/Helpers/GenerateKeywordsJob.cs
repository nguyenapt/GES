using GES.Clients.Web.Infrastructure;
using GES.Common.Logging;
using GES.Common.Services;
using GES.Common.Services.Interface;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Services;
using GES.Inside.Web.Configs;
using Quartz;

namespace GES.Inside.Web.Helpers
{
    public class GenerateKeywordsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var entites = new GesEntities();
            IGesLogger log = new SerilogAdapter(LoggerHelper.CreateJobLogger());
            IApplicationSettingsService applicationSettingsService = new ApplicationSettingsService();
            var unitOfWork = new UnitOfWork<GesEntities>(entites, log);
            var companiesRepository = new I_CompaniesRepository(entites, log, new StoredProcedureRunner(entites, applicationSettingsService, log));

            var gesCaseReportsService = new I_GesCaseProfilesService(
                unitOfWork, 
                new I_GesCaseReportsRepository(entites, log, new I_ConventionsRepository(entites, log), companiesRepository, new GesCaseReportSdgRepository(entites, log), new SdgRepository(entites, log)),
                companiesRepository,
                new I_GesCaseReportsExtraRepository(entites, log), 
                new G_ManagedDocumentsRepository(entites, log),
                new GesCaseReportSignUpRepository(entites, log),
                new I_CompaniesService(unitOfWork, 
                    new I_CompaniesRepository(entites, log, new StoredProcedureRunner(entites, applicationSettingsService, log)), 
                    new I_GesCaseReportsRepository(entites, log, new I_ConventionsRepository(entites, log), companiesRepository, new GesCaseReportSdgRepository(entites, log), new SdgRepository(entites, log)), null, log),
                new CalendarService(unitOfWork, new I_CalendarEventsRepository(entites, log), log), 
                log);
            gesCaseReportsService.ExtractKeywordsForAllCaseProfiles(SiteSettings.JobGenerateKeywordsMaxItemsEachRun);
        }
    }
}