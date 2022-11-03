using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using HtmlAgilityPack;

namespace GES.Inside.Data.Services
{
    public class DashboardService : EntityService<GesEntities, G_News>, IDashboardService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_NewsRepository _newsReporsitory;
        private readonly List<long> _gesCaseReportStatusForCharts = new List<long> { 6, 7 };
        private readonly List<long> _gesCaseReportStatusForRecommendationChart = new List<long> { 6, 7, 8 };
        private readonly long bussinessConductId = 30;
        private const string FocusListLandingPage = "focuslist";

        public DashboardService(IUnitOfWork<GesEntities> unitOfWork, IG_NewsRepository newsReporsitory, IGesLogger logger) : base(unitOfWork, logger, newsReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _newsReporsitory = newsReporsitory;// comment
        }


        #region Public methods
        public List<EventListViewModel> GetCalendarEvents(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            return this.SafeExecute<List<EventListViewModel>>(() =>
            {
                switch (landingPageType)
                {
                    case FocusListLandingPage:
                        return GetCalendarEventsByFocusList(orgId, individualId);
                    default:
                        return GetCalendarEventsByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices);
                }
            });
        }

        public List<GesLatestNewsModel> GetGesLastestNews(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            return this.SafeExecute<List<GesLatestNewsModel>>(() =>
            {
                switch (landingPageType)
                {
                    case FocusListLandingPage:
                        return GetGesLastestNewsByFocusList(orgId, individualId);
                    default:
                        return GetGesLastestNewsByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices);
                }
            });
        }

        public List<MilestoneModel> GetMilestones(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            return this.SafeExecute<List<MilestoneModel>>(() =>
            {
                switch (landingPageType)
                {
                    case FocusListLandingPage:
                        return GetMilestonesByFocusList(orgId, individualId);
                    default:
                        return GetMilestonesByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices);
                }
            });
        }

        public DashboardInfoBoxModel GetDasboardInfoBox(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            return this.SafeExecute<DashboardInfoBoxModel>(() =>
            {
                switch (landingPageType)
                {
                    case FocusListLandingPage:
                        return GetDashboardInfoBoxByFocusList(orgId, individualId);
                    default:
                        return GetDashboardInfoBoxByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices);
                }
            });
        }

        public List<PortfolioCompaniesReportModel> GetCaseReportbyPortfolios(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            return this.SafeExecute<List<PortfolioCompaniesReportModel>>(() =>
            {
                var isAllIndices = !selectedPortfoliosOrIndices.Any();

                var query = (from po in _dbContext.I_PortfoliosG_Organizations
                             join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id
                                 equals pos.I_PortfoliosG_Organizations_Id
                             join g in _dbContext.G_Services on pos.G_Services_Id equals g.G_Services_Id
                             join et in _dbContext.I_EngagementTypes on g.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                             join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                             join c1 in _dbContext.I_Companies on pc.I_Companies_Id equals c1.I_Companies_Id
                             join gcs in _dbContext.I_GesCompanies on
                                 c1.MasterI_Companies_Id != null ? c1.MasterI_Companies_Id.Value : c1.I_Companies_Id equals
                                 gcs.I_Companies_Id
                             join r in _dbContext.I_GesCaseReports on gcs.I_GesCompanies_Id equals r.I_GesCompanies_Id
                             where
                                 po.G_Organizations_Id == orgId && r.ShowInClient
                             group r by new
                             {
                                 r.I_GesCaseReports_Id,
                                 et.I_EngagementTypes_Id,
                                 po.I_PortfoliosG_Organizations_Id
                             }
                                            into g
                             select new
                             {
                                 CaseReportId = g.Key.I_GesCaseReports_Id,
                                 EngagementTypeId = g.Key.I_EngagementTypes_Id,
                                 PortfolioOrganizationId = g.Key.I_PortfoliosG_Organizations_Id
                             }).FromCache().ToList();

                var result = from q in query
                             where isAllIndices || selectedPortfoliosOrIndices.Contains(q.PortfolioOrganizationId)
                             group q by new
                             {
                                 q.CaseReportId,
                                 q.EngagementTypeId
                             } into g
                             select new PortfolioCompaniesReportModel
                             {
                                 CaseReportId = g.Key.CaseReportId,
                                 EngagementTypeId = g.Key.EngagementTypeId
                             };

                return result.ToList();
            });
        }

        public List<long> GetMasterCompanyIdsFromListPortfolios(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            var isAllIndices = !selectedPortfoliosOrIndices.Any();

            return this.SafeExecute<List<long>>(() =>
            {
                var query = (from po in _dbContext.I_PortfoliosG_Organizations
                             join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id
                                 equals pos.I_PortfoliosG_Organizations_Id
                             join gs in _dbContext.G_OrganizationsG_Services on pos.G_Services_Id equals gs.G_Services_Id
                             join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                             join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                             where po.G_Organizations_Id == orgId && gs.G_Organizations_Id == orgId
                             group c by new
                             {
                                 CompanyId = c.MasterI_Companies_Id ?? c.I_Companies_Id,
                                 PortfolioOrganizationId = po.I_PortfoliosG_Organizations_Id
                             }
                into g
                             select new
                             {
                                 g.Key.CompanyId,
                                 g.Key.PortfolioOrganizationId
                             }).FromCache().ToList();

                var result = from q in query
                             where isAllIndices || selectedPortfoliosOrIndices.Contains(q.PortfolioOrganizationId)
                             group q by new
                             {
                                 q.CompanyId
                             } into g
                             select g.Key.CompanyId;

                return result.ToList();
            });
        }

        public PaginatedResults<CaseReportListViewModel> GetUserCasesForGrid(JqGridViewModel jqGridParams, long userId)
        {
            var result = (from r in _dbContext.I_GesCaseReports
                        join gc in _dbContext.I_GesCompanies on r.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id

                        join l in _dbContext.Countries on r.CountryId equals l.Id
                        join rec in _dbContext.I_GesCaseReportStatuses on r.NewI_GesCaseReportStatuses_Id equals rec
                            .I_GesCaseReportStatuses_Id into recommendation
                        from rec in recommendation.DefaultIfEmpty()

                        join n in _dbContext.I_NormAreas on r.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                        from n in ng.DefaultIfEmpty()

                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on r.I_GesCaseReports_Id equals cret
                        .I_GesCaseReports_Id

                        join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                        et.I_EngagementTypes_Id into etg
                        from et in etg.DefaultIfEmpty()
                        join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                        from etc in etcg.DefaultIfEmpty()

                        where r.AnalystG_Users_Id == userId
                        select new CaseReportListViewModel
                {
                    Id = r.I_GesCaseReports_Id,
                    IssueName = r.ReportIncident,
                    Location = l.Name,
                    Recommendation = rec.Name,
                    EngagementType = et.Name,
                    EngagementTypeId = cret.I_EngagementTypes_Id,
                    EngagementTypeCategory = etc.Name,
                    CompanyId = c.I_Companies_Id,
                    CompanyName = c.Name,
                    Norm = n.Name,
                    SortOrderRecommendation = rec.SortOrder??0
                }).ToList();

            var query = from r in result
                select new CaseReportListViewModel
                {
                    Id = r.Id,
                    IssueName = r.IssueName,
                    Location = r.Location,
                    Recommendation = r.Recommendation,
                    EngagementType = r.EngagementType,
                    EngagementTypeId = r.EngagementTypeId,
                    EngagementTypeCategory = r.EngagementTypeCategory,
                    CompanyId = r.CompanyId,
                    CompanyName = r.CompanyName,
                    Norm = r.Norm,
                    EngagementThemeNorm = DataHelper.GetEngagementThemeNorm(r.EngagementTypeCategory, r.Norm, r.EngagementType, r.EngagementTypeId ?? 0),
                    SortOrderRecommendation = r.SortOrderRecommendation
                };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    //
                    case "companyname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.CompanyName)
                            : query.OrderByDescending(x => x.CompanyName);
                        break;
                    case "issuename":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.IssueName)
                            : query.OrderByDescending(x => x.IssueName);
                        break;
                    case "location":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Location)
                            : query.OrderByDescending(x => x.Location);
                        break;
                    case "engagementthemenorm":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.EngagementThemeNorm)
                            : query.OrderByDescending(x => x.EngagementThemeNorm);
                        break;
                    case "recommendation":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Recommendation)
                            : query.OrderByDescending(x => x.Recommendation);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.SortOrderRecommendation);
                        break;
                }
            }
            

            if (!jqGridParams._search) return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            var finalRules = JqGridHelper.GetFilterRules<CaseReportListViewModel>(jqGridParams);
            query = string.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }


        #region Data for charts

        public List<ChartModel> GetNormChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            switch (landingPageType)
            {
                case FocusListLandingPage:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataForChartByFocusList(orgId, individualId, "No"));
                default:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices, "No"));
            }
        }

        public List<ChartModel> GetGICSectorChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            switch (landingPageType)
            {
                case FocusListLandingPage:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataForChartByFocusList(orgId, individualId, "Se"));
                default:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices, "Se"));
            }
        }

        public List<ChartModel> GetLocationChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            switch (landingPageType)
            {
                case FocusListLandingPage:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataForChartByFocusList(orgId, individualId, "Con"));
                default:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices, "Con"));
            }
        }

        public List<ChartModel> GetCountriesMapData(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            switch (landingPageType)
            {
                case FocusListLandingPage:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataForChartByFocusList(orgId, individualId, "Co"));
                default:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices, "Co"));
            }
        }

        public List<ChartModel> GetRecomendationChart(long orgId, long individualId, string landingPageType, List<long> selectedPortfoliosOrIndices)
        {
            switch (landingPageType)
            {
                case FocusListLandingPage:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataForChartByFocusList(orgId, individualId, "Re"));
                default:
                    return this.SafeExecute<List<ChartModel>>(() => GetDataByPortfoliosOrIndices(orgId, selectedPortfoliosOrIndices, "Re"));
            }
        }

        #endregion

        #endregion

        #region Private methods

        private List<EventListViewModel> GetCalendarEventsByPortfoliosOrIndices(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            var result = new List<EventListViewModel>();
            var dtLimit = DateTime.Today.AddDays(-30);

            var listCompaniesFromPortfolios = GetMasterCompanyIdsFromListPortfolios(orgId, selectedPortfoliosOrIndices);

            var query = (from ca in _dbContext.I_CalenderEvents
                        join c in _dbContext.I_Companies on ca.I_Companies_Id equals c.I_Companies_Id
                        where ca.EventDate >= dtLimit
                        group ca by new
                        {
                            Id = ca.I_CalenderEvents_Id,
                            CompanyId = ca.I_Companies_Id,
                            Heading = c.Name,
                            EventEndDate = ca.EventEndDate,
                            EventDate = ca.EventDate,
                            Description = ca.Description,
                            
                            IsGesEvent = ca.GesEvent
                        } into g
                        select new EventListViewModel
                        {
                            Id = g.Key.Id,
                            CompanyId = g.Key.CompanyId,
                            Heading = g.Key.Heading,
                            EventEndDate = g.Key.EventEndDate,
                            EventDate = g.Key.EventDate,
                            Description = g.Key.Description,
                            IsGesEvent = g.Key.IsGesEvent
                        }).ToList();

            result = (from q in query
                      join c in listCompaniesFromPortfolios on q.CompanyId equals c
                      select q
                    ).OrderBy(d => d.EventDate).ToList();

            var engagementTypeEvents = from tl in _dbContext.I_TimelineItems
                                       join et in _dbContext.I_EngagementTypes on tl.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                       where tl.Date >= dtLimit
                                       select new EventListViewModel
                                       {
                                           Id = tl.I_TimelineItems_Id,
                                           Heading = et.Name,
                                           EventDate = tl.Date ?? DateTime.Now, 
                                           Description = tl.Description,
                                           IsGesEvent = false,
                                           EngagementTypeId = et.I_EngagementTypes_Id
                                       };

            result.AddRange(engagementTypeEvents.OrderBy(d => d.EventDate).ToList());
            return result;
        }

        private List<EventListViewModel> GetCalendarEventsByFocusList(long orgId, long individualId)
        {
            var suffixIndividual = "-i" + individualId;

            var result = new List<EventListViewModel>();
            var dtLimit = DateTime.Today.AddDays(-30);

            var query = from ca in _dbContext.I_CalenderEvents
                        join c in _dbContext.I_Companies on ca.I_Companies_Id equals c.I_Companies_Id into gc
                        from c in gc.DefaultIfEmpty()
                        join cp in (
                                from w in _dbContext.I_GesCompanyWatcher
                                join gc in _dbContext.I_GesCompanies on w.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                where w.G_Individuals_Id == individualId
                                select gc.I_Companies_Id
                            ) on c.I_Companies_Id equals cp
                        where ca.EventDate >= dtLimit
                        select new EventListViewModel
                        {
                            Id = ca.I_CalenderEvents_Id,
                            CompanyId = ca.I_Companies_Id,
                            Heading = c.Name,
                            EventDate = ca.EventDate,
                            EventEndDate = ca.EventEndDate,
                            Description = ca.Description,
                            IsGesEvent = ca.GesEvent
                        };

            result = this.SafeExecute<List<EventListViewModel>>(() => query.OrderBy(d => d.EventDate).ToList());


            var engagementTypeEvents = from tl in _dbContext.I_TimelineItems
                                       join et in _dbContext.I_EngagementTypes on tl.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                       where tl.Date >= dtLimit
                                       select new EventListViewModel
                                       {
                                           Id = tl.I_TimelineItems_Id,
                                           Heading = et.Name,
                                           EventDate = tl.Date ?? DateTime.Now,
                                           Description = tl.Description,
                                           IsGesEvent = false,
                                           EngagementTypeId = et.I_EngagementTypes_Id
                                       };

            var orderedEngages = this.SafeExecute<List<EventListViewModel>>(() => engagementTypeEvents.OrderBy(d => d.EventDate).ToList());

            result.AddRange(orderedEngages);
            return result;
        }

        private List<GesLatestNewsModel> GetGesLastestNewsByPortfoliosOrIndices(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            selectedPortfoliosOrIndices = selectedPortfoliosOrIndices ?? new List<long>();
            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, selectedPortfoliosOrIndices);

            var query = (from ln in _dbContext.I_GesLatestNews
                        join rp in _dbContext.I_GesCaseReports on ln.I_GesCaseReports_Id equals rp.I_GesCaseReports_Id
                        join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                        cret.I_GesCaseReports_Id

                        join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                        et.I_EngagementTypes_Id into etg
                        from et in etg.DefaultIfEmpty()

                        join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                        from etc in etcg.DefaultIfEmpty()

                        join fm in _dbContext.G_ForumMessages on rp.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg
                        from fm in fmg.DefaultIfEmpty()
                        join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg
                        from fmt in fmtg.DefaultIfEmpty()
                         join s in _dbContext.G_Services on cret.I_EngagementTypes_Id equals s.I_EngagementTypes_Id
                        join gs in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals gs.G_Services_Id
                        where gs.G_Organizations_Id == orgId &&
                              rp.ReportIncident != "Temporary Dialogue Case" && rp.ShowInClient
                        select new GesLatestNewsModel
                        {
                            NewsId = ln.I_GesLatestNews_Id,
                            NewsDescription = ln.Description,
                            NewsLatestNewsModified = ln.LatestNewsModified,
                            NewsCreated = ln.Created,
                            GesCompanyId = gc.I_GesCompanies_Id,
                            CaseReportId = rp.I_GesCaseReports_Id,
                            CaseReportHeading = rp.ReportIncident,
                            EngagementTypeId = cret.I_EngagementTypes_Id,
                            CompanyId = c.I_Companies_Id,
                            CompanyName = c.Name,
                            EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                            ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                            ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null
                        }).ToList();

            var result = (from q in query
                join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId} equals new {c.CaseReportId, c.EngagementTypeId}
                select q).ToList();

            return result.OrderByDescending(d => d.NewsLatestNewsModified).ThenByDescending(d => d.NewsCreated).Skip(0).Take(10).ToList();
        }

        private List<GesLatestNewsModel> GetGesLastestNewsByFocusList(long orgId, long individualId)
        {
            var suffixIndividual = "-i" + individualId;

            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, new List<long>());

            var query = (from ln in _dbContext.I_GesLatestNews
                        join r in _dbContext.I_GesCaseReports on ln.I_GesCaseReports_Id equals r.I_GesCaseReports_Id
                        join gc in _dbContext.I_GesCompanies on r.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id

                        join ci in (from ci in _dbContext.I_GesCaseReportsG_Individuals
                                    where ci.G_Individuals_Id == individualId
                                    select ci.I_GesCaseReports_Id).Union(
                                        from w in _dbContext.I_GesCompanyWatcher
                                        join gc in _dbContext.I_GesCaseReports on w.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                        where w.G_Individuals_Id == individualId
                                        select gc.I_GesCaseReports_Id
                                    ) on r.I_GesCaseReports_Id equals ci

                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on r.I_GesCaseReports_Id equals
                        cret.I_GesCaseReports_Id

                         join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                        et.I_EngagementTypes_Id into etg
                        from et in etg.DefaultIfEmpty()

                                 join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                        from etc in etcg.DefaultIfEmpty()

                        join fm in _dbContext.G_ForumMessages on r.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg
                        from fm in fmg.DefaultIfEmpty()
                        join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg
                        from fmt in fmtg.DefaultIfEmpty()

                         join s in _dbContext.G_Services on cret.I_EngagementTypes_Id equals s.I_EngagementTypes_Id ?? -1
                        join gs in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals gs.G_Services_Id
                        where gs.G_Organizations_Id == orgId
                        select new GesLatestNewsModel
                        {
                            NewsId = ln.I_GesLatestNews_Id,
                            NewsDescription = ln.Description,
                            NewsLatestNewsModified = ln.LatestNewsModified,
                            NewsCreated = ln.Created,
                            GesCompanyId = gc.I_GesCompanies_Id,
                            CaseReportId = r.I_GesCaseReports_Id,
                            CaseReportHeading = r.ReportIncident,
                            EngagementTypeId = cret.I_EngagementTypes_Id,
                            CompanyId = c.I_Companies_Id,
                            CompanyName = c.Name,
                            EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                            ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                            ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null
                        }).ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          select q).ToList();

            return result.OrderByDescending(d => d.NewsLatestNewsModified).ThenByDescending(d => d.NewsCreated).Skip(0).Take(10).ToList();
        }

        private List<MilestoneModel> GetMilestonesByPortfoliosOrIndices(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            selectedPortfoliosOrIndices = selectedPortfoliosOrIndices ?? new List<long>();
            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, selectedPortfoliosOrIndices);

            var query = (from m in _dbContext.I_Milestones
                        join rp in _dbContext.I_GesCaseReports on m.I_GesCaseReports_Id equals rp.I_GesCaseReports_Id
                        join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                        cret.I_GesCaseReports_Id

                        join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                        et.I_EngagementTypes_Id into etg
                        from et in etg.DefaultIfEmpty()

                        join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                        from etc in etcg.DefaultIfEmpty()

                        join fm in _dbContext.G_ForumMessages on rp.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg
                        from fm in fmg.DefaultIfEmpty()
                        join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg
                        from fmt in fmtg.DefaultIfEmpty()

                         where rp.ReportIncident != "Temporary Dialogue Case" && rp.ShowInClient
                        group m by new
                        {
                            MilestoneId = m.I_Milestones_Id,
                            MilestoneDescription = m.Description,
                            MilestoneModified = m.MilestoneModified,
                            MileStoneCreated = m.Created,

                            CaseReportId = rp.I_GesCaseReports_Id,
                            CaseReportHeading = rp.ReportIncident,
                            EngagementTypeId = cret.I_EngagementTypes_Id,

                            GesCompanyId = rp.I_GesCompanies_Id,
                            CompanyId = c.I_Companies_Id,
                            CompanyName = c.Name,
                            EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                            ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                            ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null
                        } into g
                        select new MilestoneModel
                        {
                            MilestoneId = g.Key.MilestoneId,
                            MilestoneDescription = g.Key.MilestoneDescription,
                            MilestoneModified = g.Key.MilestoneModified,
                            MileStoneCreated = g.Key.MileStoneCreated,

                            CaseReportId = g.Key.CaseReportId,
                            CaseReportHeading = g.Key.CaseReportHeading,
                            EngagementTypeId = g.Key.EngagementTypeId,

                            GesCompanyId = g.Key.GesCompanyId,
                            CompanyId = g.Key.CompanyId,
                            CompanyName = g.Key.CompanyName,
                            EngagementTypeCategoriesId = g.Key.EngagementTypeCategoriesId,
                            ParentForumMessagesId = g.Key.ParentForumMessagesId,
                            ForumMessagesTreeId = g.Key.ForumMessagesTreeId
                        }).FromCache().ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          select q).ToList();

            return result.OrderByDescending(d => d.MilestoneModified).Skip(0).Take(10).ToList();
        }

        private List<MilestoneModel> GetMilestonesByFocusList(long orgId, long individualId)
        {
            var suffixIndividual = "-i" + individualId;

            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, new List<long>());

            var query = (from m in _dbContext.I_Milestones
                        join rp in _dbContext.I_GesCaseReports on m.I_GesCaseReports_Id equals rp.I_GesCaseReports_Id
                        join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                        cret.I_GesCaseReports_Id

                        join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                        et.I_EngagementTypes_Id into etg
                        from et in etg.DefaultIfEmpty()

                        join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                        from etc in etcg.DefaultIfEmpty()

                        join fm in _dbContext.G_ForumMessages on rp.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg
                        from fm in fmg.DefaultIfEmpty()
                        join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg
                        from fmt in fmtg.DefaultIfEmpty()

                         join ci in (from ci in _dbContext.I_GesCaseReportsG_Individuals
                                    where ci.G_Individuals_Id == individualId
                                    select ci.I_GesCaseReports_Id).Union(
                                        from w in _dbContext.I_GesCompanyWatcher
                                        join gc in _dbContext.I_GesCaseReports on w.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                        where w.G_Individuals_Id == individualId
                                        select gc.I_GesCaseReports_Id

                                    ) on rp.I_GesCaseReports_Id equals ci
                        join s in _dbContext.G_Services on cret.I_EngagementTypes_Id equals s.I_EngagementTypes_Id ?? -1
                        join gs in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals gs.G_Services_Id
                        where gs.G_Organizations_Id == orgId && rp.ReportIncident != "Temporary Dialogue Case" && rp.ShowInClient
                        group m by new
                        {
                            MilestoneId = m.I_Milestones_Id,
                            MilestoneDescription = m.Description,
                            MilestoneModified = m.MilestoneModified,
                            MileStoneCreated = m.Created,

                            CaseReportId = rp.I_GesCaseReports_Id,
                            CaseReportHeading = rp.ReportIncident,
                            EngagementTypeId = cret.I_EngagementTypes_Id,

                            GesCompanyId = rp.I_GesCompanies_Id,
                            CompanyId = c.I_Companies_Id,
                            CompanyName = c.Name,
                            EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                            ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                            ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null
                        } into g
                        select new MilestoneModel
                        {
                            MilestoneId = g.Key.MilestoneId,
                            MilestoneDescription = g.Key.MilestoneDescription,
                            MilestoneModified = g.Key.MilestoneModified,
                            MileStoneCreated = g.Key.MileStoneCreated,

                            CaseReportId = g.Key.CaseReportId,
                            CaseReportHeading = g.Key.CaseReportHeading,
                            EngagementTypeId = g.Key.EngagementTypeId,

                            GesCompanyId = g.Key.GesCompanyId,
                            CompanyId = g.Key.CompanyId,
                            CompanyName = g.Key.CompanyName,
                            EngagementTypeCategoriesId = g.Key.EngagementTypeCategoriesId,
                            ParentForumMessagesId = g.Key.ParentForumMessagesId,
                            ForumMessagesTreeId = g.Key.ForumMessagesTreeId
                        }).FromCache("FocusList" + suffixIndividual).ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          select q).ToList();

            return result.OrderByDescending(d => d.MilestoneModified).Skip(0).Take(10).ToList();
        }

        private List<ChartModel> GetDataByPortfoliosOrIndices(long orgId, List<long> selectedPortfoliosOrIndices, string typeName)
        {
            selectedPortfoliosOrIndices = selectedPortfoliosOrIndices ?? new List<long>();
            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, selectedPortfoliosOrIndices);

            var query = (from c in _dbContext.I_Companies
                        join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id

                        join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                        join rd in _dbContext.I_GesCaseReportStatuses on rp.NewI_GesCaseReportStatuses_Id equals
                            rd.I_GesCaseReportStatuses_Id
                        join ct in _dbContext.Countries on rp.CountryId equals ct.Id into gct
                        from ct in gct.DefaultIfEmpty()
                        join con in _dbContext.Regions on ct.RegionId equals con.Id into gcon
                        from con in gcon.DefaultIfEmpty()
                        join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into ghc
                        from hc in ghc.DefaultIfEmpty()

                            //join i in _dbContext.I_Msci on c.I_Msci_Id equals i.I_Msci_Id
                            //join ig in _dbContext.I_Msci on i.ParentI_Msci_Id equals ig.I_Msci_Id
                            //join s in _dbContext.I_Msci on ig.ParentI_Msci_Id equals s.I_Msci_Id
                            //join st in _dbContext.I_Msci on s.ParentI_Msci_Id equals st.I_Msci_Id
                        join i in _dbContext.SubPeerGroups on c.SubPeerGroupId equals i.Id
                        join st in _dbContext.PeerGroups on i.PeerGroupId equals st.Id

                         join nom in _dbContext.I_NormAreas on rp.I_NormAreas_Id equals nom.I_NormAreas_Id into gnom
                        from nom in gnom.DefaultIfEmpty()

                        join et in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                            et.I_GesCaseReports_Id
                        join sv in _dbContext.G_Services on et.I_EngagementTypes_Id equals sv.I_EngagementTypes_Id ?? -1
                        join gs in _dbContext.G_OrganizationsG_Services on sv.G_Services_Id equals gs.G_Services_Id
                        where
                            gs.G_Organizations_Id == orgId && rp.ReportIncident != "Temporary Dialogue Case" && rp.ShowInClient
                            && _gesCaseReportStatusForRecommendationChart.Contains(rd.I_GesCaseReportStatuses_Id)
                        group rp by new
                        {
                            CaseReportId = rp.I_GesCaseReports_Id,
                            RecommendationName = rd.Name,
                            CountryShortName = hc.Alpha3Code,
                            Continent = con.Name,
                            SectorName = st.Name,
                            Norm = nom.Name,
                            rd.I_GesCaseReportStatuses_Id,
                            G_Countries_Id = hc.Id,
                            G_Continents_Id = con.Id,
                            I_Msci_Id = st.Id,
                            nom.I_NormAreas_Id,
                            CompanyId = c.I_Companies_Id,
                            EngagementTypeId = et.I_EngagementTypes_Id,
                            ServicesId = gs.G_Services_Id
                        }
                        into g
                        select new DashboardChartModel
                        {
                            CaseReportId = g.Key.CaseReportId,
                            RecommendationName = g.Key.RecommendationName,
                            CountryShortName = g.Key.CountryShortName,
                            Continent = g.Key.Continent,
                            SectorName = g.Key.SectorName,
                            Norm = g.Key.Norm,
                            RecommendationId = g.Key.I_GesCaseReportStatuses_Id,
                            CountryId = g.Key.G_Countries_Id,
                            ContinentId = g.Key.G_Continents_Id,
                            MsciId = g.Key.I_Msci_Id,
                            NormId = g.Key.I_NormAreas_Id,
                            CompanyId = g.Key.CompanyId,
                            EngagementTypeId = g.Key.EngagementTypeId,
                            ServiceId = g.Key.ServicesId
                        }).FromCache().ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          select q).ToList();

            return GetDataForChart(result, typeName);
        }

        private List<ChartModel> GetDataForChartByFocusList(long orgId, long individualId, string typeName)
        {
            var suffixIndividual = "-i" + individualId;

            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, new List<long>());

            var query = (from r in _dbContext.I_GesCaseReports
                        join ct in _dbContext.Countries on r.CountryId equals ct.Id
                        join con in _dbContext.Regions on ct.RegionId equals con.Id
                        join n in _dbContext.I_NormAreas on r.I_NormAreas_Id equals n.I_NormAreas_Id into gn
                        from n in gn.DefaultIfEmpty()

                        join gc in _dbContext.I_GesCompanies on r.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                        join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into ghc
                        from hc in ghc.DefaultIfEmpty()

                            //join i in _dbContext.I_Msci on c.I_Msci_Id equals i.I_Msci_Id
                            //join ig in _dbContext.I_Msci on i.ParentI_Msci_Id equals ig.I_Msci_Id
                            //join se in _dbContext.I_Msci on ig.ParentI_Msci_Id equals se.I_Msci_Id
                            //join st in _dbContext.I_Msci on se.ParentI_Msci_Id equals st.I_Msci_Id

                        join i in _dbContext.SubPeerGroups on c.SubPeerGroupId equals i.Id
                        join st in _dbContext.PeerGroups on i.PeerGroupId equals st.Id


                         join rd in _dbContext.I_GesCaseReportStatuses on r.NewI_GesCaseReportStatuses_Id equals rd.I_GesCaseReportStatuses_Id

                        join ci in (from ci in _dbContext.I_GesCaseReportsG_Individuals
                                    where ci.G_Individuals_Id == individualId
                                    select ci.I_GesCaseReports_Id).Union(
                                       from w in _dbContext.I_GesCompanyWatcher
                                       join gc in _dbContext.I_GesCaseReports on w.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                       where w.G_Individuals_Id == individualId
                                       select gc.I_GesCaseReports_Id

                                   ) on r.I_GesCaseReports_Id equals ci
                        join et in _dbContext.I_GesCaseReportsI_EngagementTypes on r.I_GesCaseReports_Id equals
                            et.I_GesCaseReports_Id
                        join s in _dbContext.G_Services on et.I_EngagementTypes_Id equals s.I_EngagementTypes_Id ?? -1
                        join gs in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals gs.G_Services_Id
                        where gs.G_Organizations_Id == orgId && r.ReportIncident != "Temporary Dialogue Case" && r.ShowInClient
                        && _gesCaseReportStatusForCharts.Contains(rd.I_GesCaseReportStatuses_Id)
                        group r by new
                        {
                            CaseReportId = r.I_GesCaseReports_Id,
                            RecommendationName = rd.Name,
                            CountryShortName = hc.Alpha3Code,
                            Continent = con.Name,
                            SectorName = st.Name,
                            Norm = n.Name,
                            rd.I_GesCaseReportStatuses_Id,
                            G_Countries_Id = hc.Id,
                            G_Continents_Id = con.Id,
                            I_Msci_Id = st.Id,
                            n.I_NormAreas_Id,
                            CompanyId = c.I_Companies_Id,
                            EngagementTypeId = et.I_EngagementTypes_Id,
                            ServicesId = s.G_Services_Id
                        }
                        into g
                        select new DashboardChartModel
                        {
                            CaseReportId = g.Key.CaseReportId,
                            RecommendationName = g.Key.RecommendationName,
                            CountryShortName = g.Key.CountryShortName,
                            Continent = g.Key.Continent,
                            SectorName = g.Key.SectorName,
                            Norm = g.Key.Norm,
                            RecommendationId = g.Key.I_GesCaseReportStatuses_Id,
                            CountryId = g.Key.G_Countries_Id,
                            ContinentId = g.Key.G_Continents_Id,
                            MsciId = g.Key.I_Msci_Id,
                            NormId = g.Key.I_NormAreas_Id,
                            CompanyId = g.Key.CompanyId,
                            EngagementTypeId = g.Key.EngagementTypeId,
                            ServiceId = g.Key.ServicesId
                        }).FromCache("FocusList" + suffixIndividual).ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          select q).ToList();

            return GetDataForChart(result, typeName);
        }

        private List<ChartModel> GetDataForChart(List<DashboardChartModel> data, string typeName)
        {
            var result = new List<ChartModel>();
            switch (typeName)
            {
                case "Re":
                    result = (from r in data
                              where r.RecommendationName != null && r.RecommendationId != null
                              group r by new { r.RecommendationName, r.RecommendationId }
                        into g
                              select new ChartModel
                              {
                                  Heading = g.Key.RecommendationName,
                                  Id = g.Key.RecommendationId,
                                  NumberCount = g.Count()
                              }).OrderByDescending(d => d.NumberCount).ToList();
                    break;
                    //TODO:MENDO
                //case "Co":
                //    var temp = (from r in data
                //        where r.CountryShortName != null && r.CountryId != null &&
                //            _gesCaseReportStatusForCharts.Contains(r.RecommendationId)
                //        group r by new {r.CompanyId, r.CountryId, r.CountryShortName}
                //        into g
                //        select new
                //        {
                //            g.Key.CompanyId,
                //            g.Key.CountryId,
                //            g.Key.CountryShortName
                //        }).ToList();

                //    result = (from r in temp
                //              group r by new { r.CountryId, r.CountryShortName }
                //        into g
                //              select new ChartModel
                //              {
                //                  Heading = g.Key.CountryShortName,
                //                  Id = g.Key.CountryId.Value,
                //                  NumberCount = g.Count()
                //              }).OrderByDescending(d => d.NumberCount).ToList();
                //    break;
                //case "Con":
                //    result = (from r in data
                //              where r.Continent != null && r.ContinentId != null && _gesCaseReportStatusForCharts.Contains(r.RecommendationId)
                //              group r by new { r.ContinentId, r.Continent }
                //    into g
                //              select new ChartModel
                //              {
                //                  Heading = g.Key.Continent,
                //                  Id = g.Key.ContinentId.Value,
                //                  NumberCount = g.Count()
                //              }).OrderByDescending(d => d.NumberCount).ToList();
                //    break;
                case "Se":
                    result = (from r in data
                              where r.SectorName != null && r.MsciId != null && _gesCaseReportStatusForCharts.Contains(r.RecommendationId)
                              group r by new { r.MsciId, r.SectorName }
                        into g
                              select new ChartModel
                              {
                                  Heading = g.Key.SectorName,
                                  Id = g.Key.MsciId.Value,
                                  NumberCount = g.Count()
                              }).OrderByDescending(d => d.NumberCount).ToList();
                    break;
                case "No":
                    result = (from r in data
                              where r.ServiceId == bussinessConductId && r.Norm != null && r.NormId != null && _gesCaseReportStatusForCharts.Contains(r.RecommendationId)
                              group r by new { r.NormId, r.Norm }
                        into g
                              select new ChartModel
                              {
                                  Heading = g.Key.Norm,
                                  Id = g.Key.NormId.Value,
                                  NumberCount = g.Count()
                              }).OrderByDescending(d => d.NumberCount).ToList();
                    break;
                default:
                    result = (from r in data
                              where r.RecommendationName != null && r.RecommendationId != null
                              group r by new { r.RecommendationName, r.RecommendationId }
                        into g
                              select new ChartModel
                              {
                                  Heading = g.Key.RecommendationName,
                                  Id = g.Key.RecommendationId,
                                  NumberCount = g.Count()
                              }).OrderByDescending(d => d.NumberCount).ToList();
                    break;
            }

            return result;

        }

        private DashboardInfoBoxModel GetDashboardInfoBoxByPortfoliosOrIndices(long orgId, List<long> selectedPortfoliosOrIndices)
        {
            selectedPortfoliosOrIndices = selectedPortfoliosOrIndices ?? new List<long>();

            var numCompanies = (from c in _dbContext.I_Companies
                                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                                join c1 in ( //get master company if company is child
                                    from po in _dbContext.I_PortfoliosG_Organizations
                                    join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                                    join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                                    join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on
                                        po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
                                    where po.G_Organizations_Id == orgId
                                          && selectedPortfoliosOrIndices.Contains(po.I_PortfoliosG_Organizations_Id)
                                    group c by new
                                    {
                                        CompanyId = c.MasterI_Companies_Id ?? c.I_Companies_Id
                                    }
                                    into g
                                    select new
                                    {
                                        g.Key.CompanyId
                                    }
                                    ) on c.I_Companies_Id equals c1.CompanyId
                                where
                                    c.ShowInClient && c.Name != null
                                    && (c.MasterI_Companies_Id == null || c.MasterI_Companies_Id == c.I_Companies_Id)
                                group c by c.I_Companies_Id into g
                                select new
                                {
                                    CompanyId = g.Key
                                }).Count();

            //var numCaces = (from gcr in _dbContext.I_GesCaseReports
            //                join get in _dbContext.I_GesCaseReportsI_EngagementTypes on gcr.I_GesCaseReports_Id equals get.I_GesCaseReports_Id
            //                join b in _dbContext.G_Services on get.I_EngagementTypes_Id equals b.I_EngagementTypes_Id
            //                join os in _dbContext.G_OrganizationsG_Services on b.G_Services_Id equals os.G_Services_Id
            //                join et in _dbContext.I_EngagementTypes on b.I_EngagementTypes_Id equals
            //                         et.I_EngagementTypes_Id into etg
            //                from et in etg.DefaultIfEmpty()
            //                join ft in (from po in _dbContext.I_PortfoliosG_Organizations
            //                            join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
            //                            join g in _dbContext.G_Services on pos.G_Services_Id equals g.G_Services_Id
            //                            join et in _dbContext.I_EngagementTypes on g.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
            //                            join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
            //                            join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
            //                            join gc in _dbContext.I_GesCompanies on c.MasterI_Companies_Id != null ? c.MasterI_Companies_Id.Value : c.I_Companies_Id equals gc.I_Companies_Id
            //                            join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
            //                            where po.G_Organizations_Id == orgId && rp.ShowInClient && (selectedPortfoliosOrIndices.Contains(po.I_PortfoliosG_Organizations_Id))
            //                            select new
            //                            {
            //                                rp.I_GesCaseReports_Id,
            //                                et.I_EngagementTypes_Id
            //                            }) on new { gcr.I_GesCaseReports_Id, et.I_EngagementTypes_Id } equals new { ft.I_GesCaseReports_Id, ft.I_EngagementTypes_Id }
            //                where os.G_Organizations_Id == orgId &&
            //                 gcr.ShowInClient

            //                group gcr by gcr.I_GesCaseReports_Id into g

            //                select new
            //                {
            //                    g.Key
            //                }
            //                ).Count();

            var listPortfolios = (from c in _dbContext.I_Companies
                                  join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                                  join q in (from p in _dbContext.I_Portfolios
                                             join po in _dbContext.I_PortfoliosG_Organizations on p.I_Portfolios_Id equals po.I_Portfolios_Id
                                             join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                                             join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                                             join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
                                             let companyId = c.MasterI_Companies_Id ?? c.I_Companies_Id
                                             where po.G_Organizations_Id == orgId 
                                             && c.ShowInClient && c.Name != null
                                             group p by new
                                             {
                                                 po.I_PortfoliosG_Organizations_Id,
                                                 p.Name,
                                                 p.LastUpdated,
                                                 CompanyId = companyId
                                             } into g
                                             select new
                                             {
                                                 g.Key.I_PortfoliosG_Organizations_Id,
                                                 g.Key.Name,
                                                 g.Key.LastUpdated,
                                                 g.Key.CompanyId
                                             }) on c.I_Companies_Id equals q.CompanyId
                                  where  (c.MasterI_Companies_Id == null || c.MasterI_Companies_Id == c.I_Companies_Id) && c.ShowInClient && c.Name != null
                                  group q by new
                                  {
                                      q.I_PortfoliosG_Organizations_Id,
                                      q.Name,
                                      q.LastUpdated
                                  } into g
                                  select new DashboardInfoDetail
                                  {
                                      PortfolioId = g.Key.I_PortfoliosG_Organizations_Id,
                                      PortfolioName = g.Key.Name,
                                      LastUpdated = g.Key.LastUpdated,
                                      NumHoldings = g.Count()
                                  }).OrderBy(d => d.PortfolioName).FromCache().ToList();

            listPortfolios.ForEach(d => d.PortfolioName = d.PortfolioName.Replace("[index]", "").Trim());

            var selectPortfolio = listPortfolios.Where(d => selectedPortfoliosOrIndices.Contains(d.PortfolioId)).ToList();

            var infoBox = new DashboardInfoBoxModel()
            {
                NumberOfCompanies = numCompanies,
                //NumberOfCases = numCaces,
                DashboardInfoDetails = selectPortfolio
            };

            return infoBox;
        }

        private DashboardInfoBoxModel GetDashboardInfoBoxByFocusList(long orgId, long individualId)
        {
            var suffixIndividual = "-i" + individualId;

            var portfolioBySelected = GetCaseReportbyPortfolios(orgId, new List<long>());

            var query = (from r in _dbContext.I_GesCaseReports
                        join gc in _dbContext.I_GesCompanies on r.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                        join ci in (from ci in _dbContext.I_GesCaseReportsG_Individuals
                                    where ci.G_Individuals_Id == individualId
                                    select ci.I_GesCaseReports_Id).Union(
                                       from w in _dbContext.I_GesCompanyWatcher
                                       join gc in _dbContext.I_GesCaseReports on w.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                       where w.G_Individuals_Id == individualId
                                       select gc.I_GesCaseReports_Id

                                   ) on r.I_GesCaseReports_Id equals ci
                        join et in _dbContext.I_GesCaseReportsI_EngagementTypes on r.I_GesCaseReports_Id equals
                            et.I_GesCaseReports_Id
                        join s in _dbContext.G_Services on et.I_EngagementTypes_Id equals s.I_EngagementTypes_Id ?? -1
                        join gs in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals gs.G_Services_Id
                        where gs.G_Organizations_Id == orgId && r.ReportIncident != "Temporary Dialogue Case" && r.ShowInClient
                        group r by new
                        {
                            r.I_GesCaseReports_Id,
                            c.I_Companies_Id,
                            et.I_EngagementTypes_Id
                        }
                        into g
                        select new 
                        {
                            CaseReportId = g.Key.I_GesCaseReports_Id,
                            CompanyId = g.Key.I_Companies_Id,
                            EngagementTypeId = g.Key.I_EngagementTypes_Id
                        }).FromCache("FocusList" + suffixIndividual).ToList();

            var result = (from q in query
                          join c in portfolioBySelected on new { q.CaseReportId, q.EngagementTypeId } equals new { c.CaseReportId, c.EngagementTypeId }
                          group q by new
                          {
                              q.CompanyId,
                              q.CaseReportId
                          } into g
                          select new
                          {
                              g.Key.CompanyId,
                              g.Key.CaseReportId
                          }).ToList();

            var numCompanies = query.Select(d => d.CompanyId).Distinct().Count();
            var numCases = query.Select(d => d.CaseReportId).Distinct().Count();

            return new DashboardInfoBoxModel()
            {
                NumberOfCompanies = numCompanies,
                NumberOfCases = numCases
            };
        }

        public List<long> GetOrganizationsWithBiggestSize()
        {
            var result = new List<G_Organizations>();

            var query = (from gm in _dbContext.G_Organizations
                join tmp in (
                    from c in _dbContext.I_Companies
                    join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                    join p in _dbContext.I_Portfolios on pc.I_Portfolios_Id equals p.I_Portfolios_Id
                    join po in _dbContext.I_PortfoliosG_Organizations on p.I_Portfolios_Id equals po.I_Portfolios_Id
                    join g in _dbContext.G_Organizations on po.G_Organizations_Id equals g.G_Organizations_Id
                    group po by new {po.G_Organizations_Id}
                    into g
                    select new
                    {
                        g.Key.G_Organizations_Id,
                        Size = g.Count()
                    })
                    on gm.G_Organizations_Id equals tmp.G_Organizations_Id
                orderby tmp.Size descending
                select tmp.G_Organizations_Id).Skip(0).Take(10);

            return query.ToList();
        }

        public List<GesBlogModel> GetBlogModels(string url)
        {
            var lstRssItems = new List<GesBlogModel>();

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    XmlReader reader = XmlReader.Create(url);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    if (feed != null && feed.Items != null && feed.Items.Any())
                    {
                        foreach (SyndicationItem item in feed.Items)
                        {
                            var model = new GesBlogModel();
                            model.Title = item.Title.Text;
                            model.LinkTitle = item.Links.FirstOrDefault()?.Uri.ToString();
                            model.PublishedDate = item.PublishDate.UtcDateTime;
                            if (model.LinkTitle.ToLower().Contains("https://www.sustainalytics.com/esg-blog/"))
                            {
                                foreach (SyndicationElementExtension extension in item.ElementExtensions)
                                {
                                    if (extension.OuterName == "encoded")
                                    {
                                        XElement ele = extension.GetObject<XElement>();
                                        if (ele != null && ele.Value != null)
                                        {
                                            var doc = new HtmlDocument();
                                            doc.LoadHtml(ele.Value);
                                            var avatar = doc.DocumentNode.SelectNodes("//img[contains(@class, 'alignnone')]");

                                            var matches = doc.DocumentNode.SelectNodes("//p");

                                            if (matches.Count > 2)
                                            {
                                                var shortenContent = "";
                                                for (var i = 1; i < 3; i++)
                                                {
                                                    if (i == 1)
                                                    {
                                                        if (avatar != null && avatar.Count > 0)
                                                        {
                                                            if (avatar[0].Attributes["width"] != null)
                                                            {
                                                                avatar[0].Attributes["width"].Value = "85";
                                                            }
                                                            if (avatar[0].Attributes["height"] != null)
                                                            {
                                                                avatar[0].Attributes["height"].Value = "85";
                                                            }
                                                            shortenContent += "<p>" + avatar[0].OuterHtml + "</p>";
                                                        }
                                                        shortenContent += matches[i].InnerHtml
                                                            .Replace("&nbsp;", "")
                                                            .Replace("by ", "(by ");
                                                        shortenContent += ")";
                                                    }
                                                    else
                                                    {
                                                        shortenContent += matches[i].InnerHtml;
                                                    }

                                                    if (i == 2)
                                                    {
                                                        shortenContent += ".. <a target='_blank' href='" + model.LinkTitle + "'>Read more</a>";
                                                    }

                                                    shortenContent += "</p>";
                                                }
                                                model.Content = shortenContent;
                                            }
                                        }
                                    }
                                }
                                lstRssItems.Add(model);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return lstRssItems;
        }
        #endregion
    }

    public class DashboardChartModel
    {
        public long CaseReportId { get; set; }
        public string RecommendationName { get; set; }
        public string CountryShortName { get; set; }
        public string Continent { get; set; }
        public string SectorName { get; set; }
        public string Norm { get; set; }
        public long RecommendationId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? ContinentId { get; set; }
        public long? MsciId { get; set; }
        public long? NormId { get; set; }
        public long ServiceId { get; set; }
        public long CompanyId { get; set; }
        public long EngagementTypeId { get; set; }
    }

    public class PortfolioCompaniesReportModel
    {
        public long CaseReportId { get; set; }
        public long EngagementTypeId { get; set; }

    }



}