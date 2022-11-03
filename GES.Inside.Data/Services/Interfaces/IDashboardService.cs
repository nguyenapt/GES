using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IDashboardService : IEntityService<G_News>
    {

        List<EventListViewModel> GetCalendarEvents(long orgId, long individualId, string landingPageType, List<long> selectedIndices);

        List<GesLatestNewsModel> GetGesLastestNews(long orgId, long individualId, string landingPageType, List<long> selectedIndices);

        List<MilestoneModel> GetMilestones(long orgId, long individualId, string landingPageType, List<long> selectedIndices);

        DashboardInfoBoxModel GetDasboardInfoBox(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices);

        List<ChartModel> GetNormChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices);

        List<ChartModel> GetGICSectorChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices);

        List<ChartModel> GetLocationChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices);

        List<ChartModel> GetCountriesMapData(long orgId, long individualId, string landingPageType,
            List<long> selectedPortfoliosOrIndices);

        List<ChartModel> GetRecomendationChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices);

        List<GesBlogModel> GetBlogModels(string url);

        PaginatedResults<CaseReportListViewModel> GetUserCasesForGrid(JqGridViewModel jqGridParams, long userId);
    }
}
