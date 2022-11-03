using GES.Clients.Web.Infrastructure;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Services;
using Quartz;

namespace GES.Clients.Web.Helpers
{
    public class WarmUpAppPoolJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // TODO
            // construct services
            IGesLogger log = new SerilogAdapter(LoggerHelper.CreateJobLogger());

            var gesEntities = new GesEntities();
            var gesRefreshDbContext = new GesRefreshDbContext();
            var unitOfWork = new UnitOfWork<GesEntities>(gesEntities, log);
            var gesUnitOfWork = new UnitOfWork<GesRefreshDbContext>(gesRefreshDbContext, log);
            var gesUserService = new GesUserService(gesUnitOfWork, new GesUserRepository(gesRefreshDbContext, log), log);
            var oldUserService = new OldUserService(unitOfWork, new G_UsersRepository(gesEntities, log), log);

            // get list of individualIds and orgIds for clearing cache
            var recentLoggedInEntities = CommonHelper.GetRecentLoggedInIndividualIdsAndOrgIds(gesUserService, oldUserService);

        }
    }
}