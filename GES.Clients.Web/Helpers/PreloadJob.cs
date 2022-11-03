using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Services;
using Quartz;
using System.IO;
using System.Web;
using Serilog;
using GES.Clients.Web.Infrastructure;
using GES.Common.Logging;

namespace GES.Clients.Web.Helpers
{
    public class PreloadJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // TODO:
            // construct services
            IGesLogger log = new SerilogAdapter(LoggerHelper.CreateJobLogger());

            var entites = new GesEntities();
            var dbContext = new GesRefreshDbContext();
            var unitOfWork = new UnitOfWork<GesEntities>(entites, log);
            var gesUnitOfWork = new UnitOfWork<GesRefreshDbContext>(dbContext, log);
            var dashboardService = new DashboardService(unitOfWork, new G_NewsRepository(entites, log), log);
            var gesUserService = new GesUserService(gesUnitOfWork, new GesUserRepository(dbContext, log), log);
            var oldUserService = new OldUserService(unitOfWork, new G_UsersRepository(entites, log), log);

            // get list of individualIds and orgIds for clearing cache
            var recentLoggedInEntities = CommonHelper.GetRecentLoggedInIndividualIdsAndOrgIds(gesUserService, oldUserService);
            var toBeClearedIndividualIds = recentLoggedInEntities.Select(i => i.Id).ToList();
            var toBeClearedOrgIds = recentLoggedInEntities.Where(i => i.ForeignKey != null).Select(i => i.ForeignKey.Value).Distinct().ToList();

            // reset all cache
            CacheHelper.ClearAllCache(toBeClearedIndividualIds, toBeClearedOrgIds);

            // run scheduled job
            var organizationsWithBiggestSize = dashboardService.GetOrganizationsWithBiggestSize();

            foreach (var orgId in organizationsWithBiggestSize)
            {
                // latest news, milestones and doughnut charts, map: use the same core function > preload core function
                //dashboardService.GetGesLastestNews(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetMilestones(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetRecomendationChart(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetNormChart(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetGICSectorChart(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetLocationChart(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetCountriesMapData(orgId, 1, "portfolios", new List<long>());
                dashboardService.GetCaseReportbyPortfolios(orgId, new List<long>());
                dashboardService.GetMasterCompanyIdsFromListPortfolios(orgId, new List<long>());

                // not utilizing the same second-level cache as above functions >>> not pre-load
                //dashboardService.GetCalendarEvents(orgId, 1, "portfolios", new List<long>());
                //dashboardService.GetDasboardInfoBox(orgId, 1, "portfolios", new List<long>());
            }
        }
    }
}