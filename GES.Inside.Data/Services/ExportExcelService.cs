using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.sun.org.apache.xpath.@internal.operations;
using DocumentFormat.OpenXml.Drawing.Charts;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using ikvm.extensions;
using jdk.nashorn.@internal.ir;
using sun.awt.geom;
using sun.security.krb5.@internal.crypto;
using sun.util.resources.cs;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class ExportExcelService : EntityService<GesEntities, I_Companies>, IExportExcelService
    {
        private readonly GesEntities _dbContext;
        private readonly II_CompaniesRepository _companiesRepository;
        private readonly IStoredProcedureRunner _storedProcedureRunner;

        public ExportExcelService(IUnitOfWork<GesEntities> unitOfWork, IGesLogger logger, IStoredProcedureRunner storedProcedureRunner, II_CompaniesRepository companiesRepository) : base(unitOfWork, logger, companiesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _companiesRepository = companiesRepository;
            _storedProcedureRunner = storedProcedureRunner;
        }

        #region Nathalia's request

        public List<ExcelCaseProfile> GetDataForStandardCases(long orgId, List<long> portfolioIds, bool getNotShowedInClient = false)
        {
            var isPortfolioSearch = portfolioIds.Count > 0;
            const string temporaryReportIncident = "Temporary Dialogue Case";

            var query = (from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                join rp in
                (from os in _dbContext.G_OrganizationsG_Services
                    join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                    join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on s.I_EngagementTypes_Id equals
                    cret.I_EngagementTypes_Id
                    join grc in _dbContext.I_GesCaseReports on cret.I_GesCaseReports_Id equals
                    grc.I_GesCaseReports_Id
                    join grce in _dbContext.I_GesCaseReportsExtra on grc.I_GesCaseReports_Id equals
                    grce.I_GesCaseReports_Id into grceg
                    from grce in grceg.DefaultIfEmpty()

                    join n in _dbContext.I_NormAreas on grc.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                    from n in ng.DefaultIfEmpty()
                    join l in _dbContext.Countries on grc.CountryId equals l.Id into lg
                    from l in lg.DefaultIfEmpty()
                    join rx in _dbContext.I_GesCaseReportStatuses on grc.I_GesCaseReportStatuses_Id equals
                    rx.I_GesCaseReportStatuses_Id into rdx
                    from rx in rdx.DefaultIfEmpty()

                    where os.G_Organizations_Id == orgId && grc.ShowInClient &&
                          grc.ReportIncident != temporaryReportIncident
                          && cret.I_EngagementTypes_Id == 2 && (getNotShowedInClient || rx.ShowInClient)
                    select new
                    {
                        grc.I_GesCompanies_Id,
                        grc.ReportIncident,
                        grce.Keywords,
                        Involvement = n.Name,
                        Location = l.Alpha3Code,
                        grc.EntryDate,
                        Conclusion = rx.Name
                    }
                )
                on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id into rpg
                from rp in rpg.DefaultIfEmpty()
                join c1 in (
                    from c in _dbContext.I_Companies
                    join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                    join po in _dbContext.I_PortfoliosG_Organizations on pc.I_Portfolios_Id equals
                    po.I_Portfolios_Id
                    join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on
                    po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id

                    where po.G_Organizations_Id == orgId
                          && (isPortfolioSearch == false || portfolioIds.Contains(po.I_PortfoliosG_Organizations_Id))
                    group c by new
                    {
                        CompanyId = c.I_Companies_Id
                    }
                    into g
                    select new
                    {
                        g.Key.CompanyId
                    }

                ) on c.I_Companies_Id equals c1.CompanyId

                where c.ShowInClient && c.Name != null && rp.ReportIncident != null
                group c by new
                {
                    c.I_Companies_Id,
                    gc.I_GesCompanies_Id,
                    c.Name,
                    c.Isin,
                    c.Sedol,
                    Issue = rp.ReportIncident,
                    rp.Location,
                    rp.EntryDate,
                    rp.Involvement,
                    rp.Conclusion,
                    c.BbgID
                }
                into g
                select new
                {
                    Isin = g.Key.Isin,
                    CompanyName = g.Key.Name.Trim(),
                    Issue = g.Key.Issue,
                    Location = g.Key.Location,
                    EntryDate = g.Key.EntryDate,
                    ServiceEngagementThemeNorm = g.Key.Involvement,
                    StandardConclusion = g.Key.Conclusion,
                    g.Key.BbgID
                }).ToList();

            var result = from q in query
                select new ExcelCaseProfile
                {
                    Isin = q.Isin,
                    CompanyName = q.CompanyName,
                    Issue = q.Issue,
                    Location = q.Location,
                    EntryDate = q.EntryDate?.ToString(Configurations.DateFormat) ?? string.Empty,
                    ServiceEngagementThemeNorm = q.ServiceEngagementThemeNorm,
                    StandardConclusion = q.StandardConclusion,
                    BbgID = q.BbgID
                };

            return this.SafeExecute(() => result.ToList());
        }

        public IEnumerable<ExcelCaseProfile> ExportPrescreeningSearchToExcel(long orgId, long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notshowclosecase, bool isNew, string baseUrl, bool onlycompanieswithactivecase, List<long> companyIds,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId,
            bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId)
        {
            var linkHelper = new LinkHelper(isNew, baseUrl);
            var salienceScore = new RangeHelper();
            salienceScore.Add(0, 3.99, "Low");
            salienceScore.Add(4, 7.99, "Medium");
            salienceScore.Add(8, 10, "High");

            var companyPreparednessScore = new RangeHelper();
            companyPreparednessScore.Add(0, 3.99, "Low");
            companyPreparednessScore.Add(4, 7.99, "Medium");
            companyPreparednessScore.Add(8, 10, "High");

            var accessableCompnaies = _companiesRepository.GetFilteredCompaniesFromCache(orgId, individualId, onlyShowFocusList, name, isin, portfolioIds, notshowclosecase, recommendationId, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, homeCountryIds, sustainalyticsId);

            if (companyIds.Count > 0)
            {
                accessableCompnaies = accessableCompnaies.Where(d => (onlycompanieswithactivecase == false || d.NumCasesActive > 0) && companyIds.Contains(d.Id)).OrderBy(d => d.CompanyName).ToList();
            }
            else
            {
                accessableCompnaies = accessableCompnaies.Where(d => onlycompanieswithactivecase == false || d.NumCasesActive > 0).OrderBy(d => d.CompanyName).ToList();
            }

            var caseProfiles = GetReportListViewModelForExportExcel(notshowclosecase, orgId, individualId, recommendationId, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId);

            var caseProfileViewModel = (from company in accessableCompnaies
                join caseProfile in caseProfiles on company.Id equals caseProfile.GesCompanyId
                let focus = company.IsInFocusList == 1 || caseProfile.IsInFocusList
                where !onlyShowFocusList || focus
                select new
                {
                    company.CompanyId,
                    company.SustainalyticsID,
                    company.Isin,
                    company.CompanyName,
                    company.HomeCountry,
                    caseProfile.IssueName,
                    caseProfile.Location,
                    caseProfile.EntryDate,
                    caseProfile.EngagementThemeNorm,
                    caseProfile.Recommendation,
                    caseProfile.Confirmed,
                    caseProfile.Description,
                    caseProfile.ServiceEngagementThemeNorm,
                    caseProfile.Service,
                    caseProfile.EngagementTypeId,
                    caseProfile.Conclusion,
                    caseProfile.Id,
                    caseProfile.TemplateId,
                    caseProfile.Response,
                    caseProfile.Progress,
                    caseProfile.EngagementSince,
                    caseProfile.Development,
                    caseProfile.SignUpValue,
                    caseProfile.GesCommentary,
                    caseProfile.ChangeObjective,
                    caseProfile.EngagementActivity,
                    caseProfile.TotalScoreForCompanyPreparedness,
                    caseProfile.SalientHumanRightsPotentialViolationTotalScore
                });

            return caseProfileViewModel.Distinct()
                .Select(i => new ExcelCaseProfile
                {
                    Isin = i.Isin,
                    CompanyName = i.CompanyName,
                    SustainalyticsID = i.SustainalyticsID,
                    CompanyLink = linkHelper.GenCompanyLink(i.CompanyId),
                    HomeCountry = i.HomeCountry,
                    Issue = i.IssueName,
                    IssueLink = linkHelper.GenIssueLink(i.Id, i.EngagementTypeId, i.CompanyId),
                    Location = i.Location,
                    EntryDate = i.EntryDate?.ToString(Configurations.DateFormat) ?? string.Empty,
                    //Service = i.Service,
                    ServiceEngagementThemeNorm = i.ServiceEngagementThemeNorm,
                    StandardConclusion = (i.EngagementTypeId == 2 && i.Conclusion != null) ? i.Conclusion : "",
                    StandardLink = (i.EngagementTypeId == 2 && i.Conclusion != null) ? linkHelper.GenStandardLink(i.Id, i.TemplateId, i.CompanyId) : linkHelper.GenBaseLink(),
                    //EngagementThemeNorm = i.EngagementThemeNorm,
                    Recommendation = i.Recommendation != null? i.Recommendation.Replace("Resolved (Indication of Violation)", "Resolved") : null,
                    Confirmed = i.Confirmed != null && i.Confirmed.Value ? "Yes" : "",
                    Description = i.Description,
                    Response = i.Recommendation == "Engage" ? i.Response : "",
                    Progress = i.Recommendation == "Engage" ? i.Progress : "",
                    Milestone = GetMileStoneText(i.Id),
                    EngagementSince = i.EngagementSince?.ToString(Configurations.DateFormat) ?? string.Empty,
                    Development = i.Recommendation == "Engage" ? i.Development : "",
                    Endorsement = i.SignUpValue == 1 ? "Disclose" : i.SignUpValue == 2 ? "Non-dislose" : "",
                    GesCommentary = i.GesCommentary,
                    ChangeObjective = i.ChangeObjective,
                    EngagementActivity = i.EngagementActivity != null ? i.EngagementActivity?.ToString(Configurations.DateFormat) : "",
                    UNGPSalience = salienceScore.GetResult(i.SalientHumanRightsPotentialViolationTotalScore),
                    UNGPcompanyPreparedness = companyPreparednessScore.GetResult(i.TotalScoreForCompanyPreparedness)

                }).OrderBy(i => i.CompanyName).ThenBy(d => d.Issue);
        }

        public List<CompanyListViewModel> ExportAllCompaniesByPortfolio(long orgId, List<long> portfolioIds)
        {
            return GetAllCompanyByPortfolioListView(orgId, portfolioIds).OrderBy(d => d.CompanyName).ToList();
        }

        public IEnumerable<ExcelCaseProfile> ExportPrescreeningSearchAllCompaniesByPortfolioToExcel(long orgId, List<long> portfolioIds)
        {
            var accessableCompnaies = GetAllCompanyByPortfolioListView(orgId, portfolioIds);

            var caseProfiles = GetReportListViewModelForExportExcel(false, orgId, -1, null, null, null, null, null, null, null, null, null);

            var caseProfileViewModel = (from company in accessableCompnaies
                join caseProfile in caseProfiles on company.Id equals caseProfile.GesCompanyId
                select new
                {
                    company.CompanyId,
                    company.Isin,
                    company.CompanyName,
                    company.HomeCountry,
                    caseProfile.IssueName,
                    caseProfile.Location,
                    caseProfile.EntryDate,
                    caseProfile.EngagementThemeNorm,
                    caseProfile.Recommendation,
                    caseProfile.Confirmed,
                    caseProfile.Description,
                    caseProfile.ServiceEngagementThemeNorm,
                    caseProfile.Service,
                    caseProfile.EngagementTypeId,
                    caseProfile.Conclusion,
                    caseProfile.Id,
                    caseProfile.TemplateId,
                    caseProfile.Response,
                    caseProfile.Progress,
                    caseProfile.EngagementSince,
                    caseProfile.Development
                });

            return caseProfileViewModel.Distinct()
                .Select(i => new ExcelCaseProfile
                {
                    Isin = i.Isin,
                    CompanyName = i.CompanyName,
                    HomeCountry = i.HomeCountry,
                    Issue = i.IssueName,
                    Location = i.Location,
                    EntryDate = i.EntryDate?.ToString(Configurations.DateFormat) ?? string.Empty,
                    ServiceEngagementThemeNorm = i.ServiceEngagementThemeNorm,
                    StandardConclusion = (i.EngagementTypeId == 2 && i.Conclusion != null) ? i.Conclusion : "",
                    Recommendation = i.Recommendation != null ? i.Recommendation.Replace("Resolved (Indication of Violation)", "Resolved"): null,
                    Confirmed = i.Confirmed != null && i.Confirmed.Value ? "Yes" : "",
                    Description = i.Description,
                    Response = i.Response,
                    Progress = i.Progress,
                    Milestone = GetMileStoneText(i.Id),
                    EngagementSince = i.EngagementSince?.ToString(Configurations.DateFormat) ?? string.Empty,
                    Development = i.Development
                }).OrderBy(i => i.CompanyName).ThenBy(d => d.Issue);
        }

        public IEnumerable<ExcelCaseProfile> ExportScreeningReportToExcel(long clientId, long portfolioId,
            DateTime? fromDate, DateTime? toDate)
        {
            const long conventionsTypeId = 2;
            var salienceScore = new RangeHelper();
            salienceScore.Add(0, 3.99, "Low");
            salienceScore.Add(4, 7.99, "Medium");
            salienceScore.Add(8, 10, "High");

            var companyPreparednessScore = new RangeHelper();
            companyPreparednessScore.Add(0, 3.99, "Low");
            companyPreparednessScore.Add(4, 7.99, "Medium");
            companyPreparednessScore.Add(8, 10, "High");
            
            var notIncaseReport = new long[]
            {
                3,5,9,10
            };

            var query = from p in _dbContext.I_PortfoliosI_Companies
                join pf in _dbContext.I_Portfolios on p.I_Portfolios_Id equals pf.I_Portfolios_Id
                join c1 in _dbContext.I_Companies on p.I_Companies_Id equals c1.I_Companies_Id
                join c2 in _dbContext.I_Companies on c1.MasterI_Companies_Id ?? c1.I_Companies_Id equals c2
                    .I_Companies_Id
                join g in _dbContext.I_GesCompanies on c2.I_Companies_Id equals g.I_Companies_Id
                join cr in _dbContext.I_GesCaseReports on g.I_GesCompanies_Id equals cr.I_GesCompanies_Id
                join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on cr.I_GesCaseReports_Id equals cret
                    .I_GesCaseReports_Id
//                join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
//                    et.I_EngagementTypes_Id into etg
//                from et in etg.DefaultIfEmpty()
                
                
                join et in ( from et in _dbContext.I_EngagementTypes 
                    join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc
                        .I_EngagementTypeCategories_Id 
                    where et.I_EngagementTypes_Id == 2 
                    select new
                    {
                        et.I_EngagementTypes_Id,
                        etc.Name
                    } ) on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into etg
                from et in etg.DefaultIfEmpty()
                
//                join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc
//                    .I_EngagementTypeCategories_Id into etcg
//                from etc in etcg.DefaultIfEmpty()

                join hqloc in (from gct1 in _dbContext.Countries
                    join cont in _dbContext.Regions on gct1.RegionId equals cont.Id
                        select new
                        {
                            gct1.Id,
                            gct1.Name,
                            gct1.Alpha3Code,
                            HQRegion = cont.Name
                        }) on c1.CountryOfIncorporationId equals hqloc.Id into homeLocation
                from hqloc in homeLocation.DefaultIfEmpty()

                join acloc in (from gct2 in _dbContext.Countries
                    join cont in _dbContext.Regions on gct2.RegionId equals cont.Id
                    select new
                    {
                        gct2.Id,
                        gct2.Name,
                        gct2.Alpha3Code,
                        IncidentRegion = cont.Name
                    }) on cr.CountryId equals acloc.Id into accidentLocation
                from acloc in accidentLocation.DefaultIfEmpty()
                
                join con in _dbContext.I_GesCaseReportStatuses on cr.I_GesCaseReportStatuses_Id equals con
                    .I_GesCaseReportStatuses_Id into conclusion
                from con in conclusion.DefaultIfEmpty()

                join rec in _dbContext.I_GesCaseReportStatuses on cr.NewI_GesCaseReportStatuses_Id equals rec
                    .I_GesCaseReportStatuses_Id into recommendation
                from rec in recommendation.DefaultIfEmpty()
                join m in _dbContext.SubPeerGroups on c2.SubPeerGroupId equals m.Id into msci
                from m in msci.DefaultIfEmpty()
                join n in _dbContext.I_NormAreas on cr.I_NormAreas_Id equals n.I_NormAreas_Id into norm
                from n in norm.DefaultIfEmpty()
                join rpt in _dbContext.I_GesCaseReportSourceTypes on cr.I_GesCaseReportSourceTypes_Id equals rpt
                    .I_GesCaseReportSourceTypes_Id into reportType
                from rpt in reportType.DefaultIfEmpty()
                join respt in _dbContext.I_ResponseStatuses on cr.I_ResponseStatuses_Id equals respt
                    .I_ResponseStatuses_Id into responseStatuses
                from respt in responseStatuses.DefaultIfEmpty()
                join ppt in _dbContext.I_ProgressStatuses on cr.I_ProgressStatuses_Id equals ppt.I_ProgressStatuses_Id
                    into progressStatuses
                from ppt in progressStatuses.DefaultIfEmpty()
                join gp in (from gp in _dbContext.I_GesCaseReportsI_ProcessStatuses
                    where gp.I_ProcessStatuses_Id == 2
                    select gp) on cr.I_GesCaseReports_Id equals gp.I_GesCaseReports_Id into ggp
                from gp in ggp.DefaultIfEmpty()
                join af in (from ac in _dbContext.I_ActivityForms
                        join op in _dbContext.I_EngagementActivityOptions on ac.I_EngagementActivityOptions_Id equals op
                            .I_EngagementActivityOptions_Id
                        where ac.G_Organizations_Id == clientId
                        select new
                        {
                            ac.I_GesCaseReports_Id,
                            op.ShortName
                        }
                    ) on cr.I_GesCaseReports_Id equals af.I_GesCaseReports_Id
                    into activityForms
                from af in activityForms.DefaultIfEmpty()

                join gcm in (
                        from gmt in _dbContext.I_GesCommentary
                        group gmt by gmt.I_GesCaseReports_Id
                        into g
                        select g.OrderByDescending(d => d.CommentaryModified).FirstOrDefault()
                    ) on cr.I_GesCaseReports_Id equals gcm.I_GesCaseReports_Id
                    into gcmg
                from gcm in gcmg.DefaultIfEmpty()

                join grce in _dbContext.I_GesCaseReportsExtra on cr.I_GesCaseReports_Id equals grce.I_GesCaseReports_Id
                    into grceg
                from grce in grceg.DefaultIfEmpty()

                join dl in (from x in _dbContext.I_GesCompanyDialogues
                        where x.ClassA
                        group x by x.I_GesCaseReports_Id
                        into g
                        select g.OrderByDescending(d => d.ContactDate).FirstOrDefault()
                    ) on cr.I_GesCaseReports_Id equals dl.I_GesCaseReports_Id
                    into dlg
                from dl in dlg.DefaultIfEmpty()
                join ungp in _dbContext.GesUNGPAssessmentForm on cr.I_GesCaseReports_Id equals ungp.I_GesCaseReports_Id
                    into ungpg
                from ungp in ungpg.DefaultIfEmpty()

                where p.I_Portfolios_Id == portfolioId && cr.IsBurmaEngCase == false && cret.I_EngagementTypes_Id == 2 && cr.I_GesCaseReportStatuses_Id != null &&
                      (!notIncaseReport.Contains((long)cr.I_GesCaseReportStatuses_Id) || cr.ConclusionChanged > fromDate)
                     
                orderby c2.Name
                        select new
                {
                    pf.Name,
                    c1.Id,
                    c1.Isin,
                    CompanyName = c1.Name,
                    MsciIndustry = m.Name,
                    cr.ReportIncident,
                    Norm = n.Name, //Involvement
                    hqloc.Alpha3Code,
                    HQLocation = hqloc.Name,
                    hqloc.HQRegion,
                    IncidentLocation = acloc.Name,
                    acloc.IncidentRegion,
                    cr.I_GesCaseReports_Id,
                    cr.EntryDate,
                    cr.NewI_GesCaseReportStatuses_IdModified,
                    cr.ConclusionChanged,
                    cr.NewI_GesCaseReportStatuses_Id,
                    cr.I_GesCaseReportStatuses_Id,
                    cr.Summary,
                    cr.ClosingIncidentAnalysisConclusion,
                    cr.I_ResponseStatuses_Id,
                    cr.I_ProgressStatuses_Id,
                    conclusionName = con.Name,
                    conclusionId = cr.I_GesCaseReportStatuses_Id,
                    recommendationName = rec.Name,
                    recommendationId = cr.NewI_GesCaseReportStatuses_Id,
                    af.ShortName,
                    cr.Created,
                    cr.ProcessGoal,
                    cr.ProcessGoalModified,
                    EngagementActivity = dl == null ? null : dl.ContactDate,
                    ProgressGrade = cr.I_ProgressStatuses_Id != null && rec.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase)? cr.I_ProgressStatuses_Id: 0,
                    ResponseGrade = cr.I_ResponseStatuses_Id != null && rec.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase)? cr.I_ResponseStatuses_Id: 0,
                    GesCommentary = gcm == null ? "" : gcm.Description,
                    gcm.CommentaryModified,
                    homeCountry = hqloc.Name,
                    EngagementTypeId = et == null ? 0 : et.I_EngagementTypes_Id,
                    EngagementTypeCategory = et.Name,
                    EngagementType = et.Name,
                    Confirmed = et.I_EngagementTypes_Id == conventionsTypeId ? cr.Confirmed : new bool?(),
                    Response = respt.ShortName,
                    Progress = ppt.ShortName,
                    TotalScoreForCompanyPreparedness = ungp != null ? ungp.TotalScoreForCompanyPreparedness : null,
                    SalientHumanRightsPotentialViolationTotalScore =
                        ungp != null ? ungp.SalientHumanRightsPotentialViolationTotalScore : null
                };

            var portfolio = query.ToList().Distinct().Select(pf => new ExcelCaseProfile
            {
                PortfolioName = pf.Name,
                Isin = pf.Isin,
                CompanyName = pf.CompanyName,
                SustainalyticsID = pf.Id,
                Issue = pf.ReportIncident,
                Norm = pf.Name, //Involvement
                Location = pf.Alpha3Code,
                EntryDate = pf.EntryDate?.ToString(Configurations.DateFormat) ?? string.Empty,
                New = (pf.EntryDate > fromDate || pf.ConclusionChanged > fromDate) ? "NEW" : "",
                GlobalEthicalStandard = GetGlobalEthicalStandardValue(pf.I_GesCaseReports_Id, pf.ConclusionChanged,
                    fromDate, pf.conclusionId, pf.conclusionName),
                New2 =
                    (pf.EntryDate > fromDate || pf.NewI_GesCaseReportStatuses_IdModified > fromDate) ? "NEW" : "",
                EngagementForum = GetEngagementForum(pf.I_GesCaseReports_Id, pf.NewI_GesCaseReportStatuses_IdModified,
                    fromDate, pf.recommendationId, pf.recommendationName),
                Endorsement = pf.ShortName,
                EngagementSince =  GetEngagementSince(pf.I_GesCaseReports_Id)?.ToString(Configurations.DateFormat) ?? string.Empty,
                //ChangeObjective = pf.ProcessGoal,
                EngagementActivity = pf.EngagementActivity?.ToString(Configurations.DateFormat) ?? string.Empty,
                EngagementResponse = pf.NewI_GesCaseReportStatuses_Id == 7 ? pf.ShortName : null,
                EngagementProgress = pf.NewI_GesCaseReportStatuses_Id == 7 ? pf.ShortName : null,
                DevelopmentGrade = DataHelper.CalcDevelopmentGrade(pf.ProgressGrade ?? 0, pf.ResponseGrade ?? 0),
                Development =
                    DataHelper.ConvertDevelopmentGradeToText(
                        DataHelper.CalcDevelopmentGrade(pf.ProgressGrade ?? 0, pf.ResponseGrade ?? 0)),
                
                Description = pf.Summary,
                HomeCountry = pf.homeCountry,
                Recommendation = pf.recommendationName != null?pf.recommendationName + GetRecommendationText(GetRecommendationBefore(pf.I_GesCaseReports_Id)) : "",
                EngagementThemeNorm = DataHelper.GetEngagementThemeNorm(pf.EngagementTypeCategory, pf.Norm,
                    pf.EngagementType, pf.EngagementTypeId),
                Confirmed = pf.Confirmed != null && pf.Confirmed.Value ? "Yes" : "",
                Response = pf.recommendationName == "Engage" ? pf.Response : "",
                Progress = pf.recommendationName == "Engage" ? pf.Progress : "",
                UNGPSalience = salienceScore.GetResult(pf.SalientHumanRightsPotentialViolationTotalScore),
                UNGPcompanyPreparedness = companyPreparednessScore.GetResult(pf.TotalScoreForCompanyPreparedness),
                HQLocation = pf.HQLocation,
                HQRegion = pf.HQRegion,
                IncidentLocation = pf.IncidentLocation,
                IncidentRegion = pf.IncidentRegion,
                MSCISector = pf.MsciIndustry,
                GesCommentary = pf.CommentaryModified != null? pf.CommentaryModified?.ToString(Configurations.DateFormat) + ": " +  pf.GesCommentary :  pf.GesCommentary,
                ChangeObjective = pf.ProcessGoalModified != null? pf.ProcessGoalModified?.ToString(Configurations.DateFormat) + ": " +  pf.ProcessGoal :  pf.ProcessGoal,
                Milestone = GetFirstMileStoneText(pf.I_GesCaseReports_Id)
            });

            return portfolio.ToList();
        }

        private string GetGlobalEthicalStandardValue(long caseReportId, DateTime? conclusionChanged, DateTime? fromDate, long? conclusionId, string conclusionName)
        {
            var conclusionBeforeValue = GetConclusionBefore(caseReportId, conclusionId);

            var query = (from st in _dbContext.I_GesCaseReportStatuses
                where st.I_GesCaseReportStatuses_Id == conclusionBeforeValue
                select new
                {
                    st.Name
                }).FirstOrDefault();

            if (query != null && conclusionChanged > fromDate)
            {
                return conclusionName.replace(" (EV)", "").replace(" (Confirmed Violation)", "")
                           .replace(" (Indication of Violation)", "") + " (" + query.Name.replace(" (EV)", "")
                           .replace(" (Confirmed Violation)", "").replace(" (Indication of Violation)", "") + ")";
            }

            return conclusionName;
        }
        
        private string GetEngagementForum(long caseReportId, DateTime? recommendationChanged, DateTime? fromDate, long? conclusionId, string recommendationName)
        {
            var conclusionBeforeValue = GetConclusionBefore(caseReportId, conclusionId);

            var query = (from st in _dbContext.I_GesCaseReportStatuses
                where st.I_GesCaseReportStatuses_Id == conclusionBeforeValue
                select new
                {
                    st.Name
                }).FirstOrDefault();

            if (query != null && recommendationChanged > fromDate)
            {
                return recommendationName.replace(" (EV)", "").replace(" (Confirmed Violation)", "")
                           .replace(" (Indication of Violation)", "") + " (" + query.Name.replace(" (EV)", "")
                           .replace(" (Confirmed Violation)", "").replace(" (Indication of Violation)", "") + ")";
            }

            return recommendationName;
        }

        private long? GetConclusionBefore(long caseReportId, long? currentConclusionId)
        {
            var query = (from craudit in _dbContext.I_GesCaseReports_Audit
                where craudit.I_GesCaseReports_Id == caseReportId &&
                      craudit.I_GesCaseReportStatuses_Id != currentConclusionId
                      orderby craudit.AuditDateTime descending 
                select new
                {
                    craudit.I_GesCaseReportStatuses_Id
                }).FirstOrDefault();

            return  query?.I_GesCaseReportStatuses_Id;
        }

        public List<CompanyListViewModel> GetCompaniesByPortfolioIdsListView(long orgId, List<long> portfolioIds, DateTime? fromDate)
        {
            var query = from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.MasterI_Companies_Id ?? c.I_Companies_Id equals gc.I_Companies_Id into gcg
                from gc in gcg.DefaultIfEmpty()
                
                join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into hcg
                from hc in hcg.DefaultIfEmpty()
                
                join mt in _dbContext.I_Companies on c.MasterI_Companies_Id ?? c.I_Companies_Id equals mt.I_Companies_Id into mtc
                from mt in mtc.DefaultIfEmpty()
                
                join ms in _dbContext.SubPeerGroups on mt.SubPeerGroupId equals ms.Id into msg
                from ms in msg.DefaultIfEmpty()
                join c1 in (
                    from c in _dbContext.I_Companies
                    join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                    join po in _dbContext.I_PortfoliosG_Organizations on pc.I_Portfolios_Id equals
                    po.I_Portfolios_Id
//                    join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on
//                    po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
//                    join gs in _dbContext.G_OrganizationsG_Services on pos.G_Services_Id equals gs.G_Services_Id
                    where portfolioIds.Contains(po.I_Portfolios_Id)
                    group c by new
                    {
                        CompanyId = c.I_Companies_Id
                    }
                    into g
                    select new
                    {
                        g.Key.CompanyId
                    }

                ) on c.I_Companies_Id equals c1.CompanyId
                
                
                
                group c by new
                {
                    c.I_Companies_Id,
                    GesCompanyId = gc != null ? gc.I_GesCompanies_Id : -1,
                    SustainalyticsID = c.Id,
                    c.Name,
                    HomeCountry = hc.Name,
                    c.Isin,
                    c.Sedol,
                    MsciIndustry = ms.Name
                }
                into g
                select new CompanyListViewModel
                {
                    Id = g.Key.GesCompanyId,
                    CompanyIssueName = g.Key.Name,
                    HomeCountry = g.Key.HomeCountry,
                    CompanyId = g.Key.I_Companies_Id,
                    CompanyName = g.Key.Name.Trim(),
                    Isin = g.Key.Isin,
                    Sedol = g.Key.Sedol,
                    MsciIndustry = g.Key.MsciIndustry,
                    SustainalyticsID = g.Key.SustainalyticsID
                };

            var listData = query.ToList();

            //number of cases
            var caseCount = (from gcr in _dbContext.I_GesCaseReports
                join get in _dbContext.I_GesCaseReportsI_EngagementTypes on gcr.I_GesCaseReports_Id equals get.I_GesCaseReports_Id

                join b in _dbContext.G_Services on get.I_EngagementTypes_Id equals b.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on b.G_Services_Id equals os.G_Services_Id
                where os.G_Organizations_Id == orgId &&
                      gcr.ShowInClient
                group gcr by gcr.I_GesCompanies_Id
                into grp
                select new { GesCompanyId = grp.Key, numCases = grp.Count() }
            ).ToList();

            //number of alert
            var alertsCount = (from na in _dbContext.I_NaArticles
                group na by na.I_Companies_Id
                into gna
                select new { CompanyId = gna.Key, numAlerts = gna.Count() }
            ).FromCache().ToList();
            
            return (from q in listData
                join nc in caseCount on q.Id equals nc.GesCompanyId into ncg
                from nc in ncg.DefaultIfEmpty()
                join a in alertsCount on q.CompanyId equals a.CompanyId ?? 0 into ag
                from a in ag.DefaultIfEmpty()
                
                select new CompanyListViewModel
                {
                    Id = q.Id,
                    CompanyIssueName = q.CompanyIssueName,
                    HomeCountry = q.HomeCountry,
                    CompanyId = q.CompanyId,
                    CompanyName = q.CompanyName,
                    CompanyLink = "",
                    NumCases = nc?.numCases ?? 0,
                    NumAlerts = a?.numAlerts ?? 0,
                    Isin = q.Isin,
                    Sedol = q.Sedol,
                    MsciIndustry = q.MsciIndustry,
                    Conclusion = GetConclusionText(q.Id, fromDate),
                    Recommendation = GetRecommendationText(q.Id, fromDate),
                    ChangeSince = GetChangeSince(q.Id, fromDate),
                    SustainalyticsID = q.SustainalyticsID
                }).ToList();
        }

        #endregion

        private string GetConclusionText(long companyId, DateTime? fromDate)
        {
            //conclusion of company
            var inCaseReportStatuses = new long[]
            {
                1, 2, 3, 5, 10, 11
            };
            
            var notIncaseReport = new long[]
            {
                3,5,9,10
            };
            
            
            var conclusion = (from cr in _dbContext.I_GesCaseReports
                join cs in _dbContext.I_GesCaseReportStatuses on cr.I_GesCaseReportStatuses_Id equals cs
                    .I_GesCaseReportStatuses_Id
                join et in _dbContext.I_GesCaseReportsI_EngagementTypes on cr.I_GesCaseReports_Id equals et
                    .I_GesCaseReports_Id
                where cr.I_GesCompanies_Id == companyId && inCaseReportStatuses.Contains(cs.I_GesCaseReportStatuses_Id) 
                                                        && (!notIncaseReport.Contains((long)cr.I_GesCaseReportStatuses_Id) || cr.ConclusionChanged > fromDate)
                                                        && et.I_EngagementTypes_Id == 2
                                                        
                                                        
                orderby cs.CompoundOrder descending
                select new
                {
                    cr.I_GesCaseReports_Id,
                    cs.I_GesCaseReportStatuses_Id,
                    cs.Name
                }).ToList();

            var latestConclusion = conclusion.FirstOrDefault();

            
            var sameConclusionCount = 0;

            if (latestConclusion != null)
            {
                sameConclusionCount = conclusion.Count(x =>
                    x.I_GesCaseReports_Id != latestConclusion.I_GesCaseReports_Id &&
                    x.I_GesCaseReportStatuses_Id == latestConclusion.I_GesCaseReportStatuses_Id);
            }
            
            var conclusionValue = latestConclusion?.I_GesCaseReportStatuses_Id ?? 0;

            var result = "";

            switch (conclusionValue)
            {
                case (long) ConclusionType.IndicationOfViolation:
                    result = "!";
                    break;
                case (long) ConclusionType.Alert:
                    result = "Alert";
                    break;
                case (long) ConclusionType.ConfirmedViolation:
                    result = "X";
                    break;
                case (long) ConclusionType.Archived:
                    result = "Archived";
                    break;
                case (long) ConclusionType.Resolved:
                    result = "Resolved";
                    break;
                default:
                    return "";
            }

            if (sameConclusionCount > 0)
            {
                result = $"{result}*";
            }

            return result;
        }

        
        private string GetRecommendationText(long companyId, DateTime? fromDate)
        {
            var inCaseReportStatuses = new long[]
            {
                6, 7, 8, 3, 10, 5, 9
            };
            
            var notIncaseReport = new long[]
            {
                3,5,9,10
            };
            
            //recommendation of company
            var recommendation = (from cr in _dbContext.I_GesCaseReports
                join cs in _dbContext.I_GesCaseReportStatuses on cr.NewI_GesCaseReportStatuses_Id equals cs
                    .I_GesCaseReportStatuses_Id
                join et in _dbContext.I_GesCaseReportsI_EngagementTypes on cr.I_GesCaseReports_Id equals et
                    .I_GesCaseReports_Id
                where cr.I_GesCompanies_Id == companyId && inCaseReportStatuses.Contains((long)cr.NewI_GesCaseReportStatuses_Id)
                                                            && (!notIncaseReport.Contains((long)cr.I_GesCaseReportStatuses_Id) || cr.ConclusionChanged > fromDate) && et.I_EngagementTypes_Id == 2

                orderby cs.CompoundOrder descending
                select new
                {
                    cr.I_GesCaseReports_Id,
                    cr.NewI_GesCaseReportStatuses_Id,
                    cs.Name
                }).FirstOrDefault();
            
            var recommendationValue = recommendation?.NewI_GesCaseReportStatuses_Id ?? 0;
            
            return GetRecommendationText(recommendationValue);
        }

        private DateTime? GetEngagementSince(long caseReportId)
        {
            var recommendationChanged = GetRecommendationChanged(caseReportId);

            var entryDate = _dbContext.I_GesCaseReports.Where(es =>
                    es.I_GesCaseReports_Id == caseReportId && es.NewI_GesCaseReportStatuses_Id == 7)
                .Select(es => new
                {
                    es.EntryDate
                }).FirstOrDefault()?.EntryDate?.AddDays(4);

            return recommendationChanged ?? entryDate;
            
        }

        private DateTime? GetRecommendationChanged(long caseReportId)
        {

            var currentRecommendation = _dbContext.I_GesCaseReports.Where(cr => cr.I_GesCaseReports_Id == caseReportId)
                .Select(cr => new {cr.NewI_GesCaseReportStatuses_Id}).FirstOrDefault()?.NewI_GesCaseReportStatuses_Id;


            var recommendationChanged = (from rc in _dbContext.I_GesCaseReports_Audit
                where rc.I_GesCaseReports_Id == caseReportId &&
                      rc.NewI_GesCaseReportStatuses_Id != currentRecommendation
                orderby rc.AuditDateTime descending
                select new
                {
                    rc.AuditDateTime
                }).FirstOrDefault();

            return recommendationChanged?.AuditDateTime;
        }

        private long GetRecommendationBefore(long caseReportId)
        {
            var currentRecommendation = _dbContext.I_GesCaseReports.Where(cr => cr.I_GesCaseReports_Id == caseReportId)
                .Select(cr => new {cr.NewI_GesCaseReportStatuses_Id}).FirstOrDefault()?.NewI_GesCaseReportStatuses_Id;


            var recommendationBefore = (from rb in _dbContext.I_GesCaseReports_Audit
                where rb.I_GesCaseReports_Id == caseReportId &&
                      rb.NewI_GesCaseReportStatuses_Id != currentRecommendation
                orderby rb.AuditDateTime descending
                select new
                {
                    rb.NewI_GesCaseReportStatuses_Id
                }).FirstOrDefault();

            return recommendationBefore?.NewI_GesCaseReportStatuses_Id ?? 0;
        }

        private string GetRecommendationText(long recommendationValue)
        {
            string result;
            
            switch (recommendationValue)
            {
                case (long) RecommendationType.Evaluate:
                    result = "Evaluate";
                    break;
                case (long) RecommendationType.Engage:
                    result = "Engage";
                    break;
                case (long) RecommendationType.Disengage:
                    result = "Disengage";
                    break;
                case (long) RecommendationType.Archived:
                    result = "Archived";
                    break;
                case (long) RecommendationType.Resolved:
                    result = "Resolved";
                    break;
                case (long) RecommendationType.ResolvedIndicationOfViolation:
                    result = "Resolved (Indication Of Violation)";
                    break;
                default:
                    return "";
            }
            
            return " (" + result + ")";
        }
        
        private string GetChangeSince(long companyId, DateTime? fromDate)
        {
            var changeSince = (from cr in _dbContext.I_GesCaseReports
                join cs in _dbContext.I_GesCaseReportStatuses on cr.NewI_GesCaseReportStatuses_Id equals cs
                    .I_GesCaseReportStatuses_Id
                join et in _dbContext.I_GesCaseReportsI_EngagementTypes on cr.I_GesCaseReports_Id equals et
                    .I_GesCaseReports_Id
                where cr.I_GesCompanies_Id == companyId && ((cr.NewI_GesCaseReportStatuses_Id == 6
                                                             || cr.NewI_GesCaseReportStatuses_Id == 7
                                                             || cr.NewI_GesCaseReportStatuses_Id == 8
                                                             || cr.NewI_GesCaseReportStatuses_Id == 3
                                                             || cr.NewI_GesCaseReportStatuses_Id == 10
                                                             || cr.NewI_GesCaseReportStatuses_Id == 9
                                                             || cr.NewI_GesCaseReportStatuses_Id == 5)
                                                            && (cr.I_GesCaseReportStatuses_Id != 3
                                                                || cr.I_GesCaseReportStatuses_Id != 5
                                                                || cr.I_GesCaseReportStatuses_Id != 9
                                                                || cr.I_GesCaseReportStatuses_Id != 10 || cr.ConclusionChanged > fromDate) 
                                                        && et.I_EngagementTypes_Id == 2
                                                        && (cr.EntryDate > fromDate || cr.ConclusionChanged > fromDate || cr.NewI_GesCaseReportStatuses_IdModified > fromDate))
                orderby cs.CompoundOrder descending
                select new
                {
                    cr.NewI_GesCaseReportStatuses_Id
                }).ToList();

            var changeSinceCount = changeSince?.Count??0;

            return changeSinceCount > 0 ? "NEW" : "";
        }
        
       

        private List<CompanyListViewModel> GetAllCompanyByPortfolioListView(long orgId, List<long> portfolioIds)
        {
            bool isportfolioSearch = portfolioIds != null && portfolioIds.Count > 0;

            var query = from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id into gcg
                from gc in gcg.DefaultIfEmpty()
                join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into hcg
                from hc in hcg.DefaultIfEmpty()

                join ms in _dbContext.SubPeerGroups on c.I_Msci_Id equals ms.Id into msg
                from ms in msg.DefaultIfEmpty()
                join c1 in (
                    from c in _dbContext.I_Companies
                    join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                    join po in _dbContext.I_PortfoliosG_Organizations on pc.I_Portfolios_Id equals
                    po.I_Portfolios_Id
                    join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on
                    po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
                    join gs in _dbContext.G_OrganizationsG_Services on pos.G_Services_Id equals gs.G_Services_Id
                    where po.G_Organizations_Id == orgId && gs.G_Organizations_Id == orgId
                          && (isportfolioSearch == false || portfolioIds.Contains(po.I_PortfoliosG_Organizations_Id))
                    group c by new
                    {
                        CompanyId = c.I_Companies_Id
                    }
                    into g
                    select new
                    {
                        g.Key.CompanyId
                    }

                ) on c.I_Companies_Id equals c1.CompanyId
                group c by new
                {
                    c.I_Companies_Id,
                    GesCompanyId = gc != null ? gc.I_GesCompanies_Id : -1,
                    c.Name,
                    HomeCountry = hc.Name,
                    c.Isin,
                    c.Sedol,
                    MsciIndustry = ms.Name
                }
                into g
                select new CompanyListViewModel
                {
                    Id = g.Key.GesCompanyId,
                    CompanyIssueName = g.Key.Name,
                    HomeCountry = g.Key.HomeCountry,
                    CompanyId = g.Key.I_Companies_Id,
                    CompanyName = g.Key.Name.Trim(),
                    Isin = g.Key.Isin,
                    Sedol = g.Key.Sedol,
                    MsciIndustry = g.Key.MsciIndustry
                };

            var listData = query.ToList();

            //number of cases
            var caseCount = (from gcr in _dbContext.I_GesCaseReports
                join get in _dbContext.I_GesCaseReportsI_EngagementTypes on gcr.I_GesCaseReports_Id equals get.I_GesCaseReports_Id

                join b in _dbContext.G_Services on get.I_EngagementTypes_Id equals b.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on b.G_Services_Id equals os.G_Services_Id
                where os.G_Organizations_Id == orgId &&
                      gcr.ShowInClient
                group gcr by gcr.I_GesCompanies_Id
                into grp
                select new { GesCompanyId = grp.Key, numCases = grp.Count() }
            ).ToList();

            //number of alert
            var alertsCount = (from na in _dbContext.I_NaArticles
                group na by na.I_Companies_Id
                into gna
                select new { CompanyId = gna.Key, numAlerts = gna.Count() }
            ).FromCache().ToList();

            return (from q in listData
                join nc in caseCount on q.Id equals nc.GesCompanyId into ncg
                from nc in ncg.DefaultIfEmpty()
                join a in alertsCount on q.CompanyId equals a.CompanyId ?? 0 into ag
                from a in ag.DefaultIfEmpty()
                select new CompanyListViewModel
                {
                    Id = q.Id,
                    CompanyIssueName = q.CompanyIssueName,
                    HomeCountry = q.HomeCountry,
                    CompanyId = q.CompanyId,
                    CompanyName = q.CompanyName,
                    CompanyLink = "",
                    NumCases = nc?.numCases ?? 0,
                    NumAlerts = a?.numAlerts ?? 0,
                    Isin = q.Isin,
                    Sedol = q.Sedol,
                    MsciIndustry = q.MsciIndustry
                }).ToList();
        }

        private List<CaseReportListViewModel> GetReportListViewModelForExportExcel(bool notshowclosecase, long orgId, long individualId, List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId)
        {
            const long conventionsTypeId = 2;
            recommendationId = recommendationId ?? new List<long?>();
            conclusionIds = conclusionIds ?? new List<long?>();
            responseIds = responseIds ?? new List<long?>();
            progressIds = progressIds ?? new List<long?>();
            locationIds = locationIds ?? new List<Guid?>();
            serviceIds = serviceIds ?? new List<long?>();

            var isSearchRecommendation = recommendationId.Any(i => i != null);
            var isSearchConclusion = conclusionIds.Any(i => i != null);
            var isSearchResponse = responseIds.Any(i => i != null);
            var isSearchProgress = progressIds.Any(i => i != null);
            var isSearchLocation = locationIds.Any(i => i != null);
            var isSearchServices = serviceIds.Any(i => i != null);

            var allCaseReports = (from rp in _dbContext.I_GesCaseReports
                join l in _dbContext.Countries on rp.CountryId equals l.Id into lg
                from l in lg.DefaultIfEmpty()
                join n in _dbContext.I_NormAreas on rp.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                from n in ng.DefaultIfEmpty()
                join rd in _dbContext.I_GesCaseReportStatuses on rp.NewI_GesCaseReportStatuses_Id equals
                rd.I_GesCaseReportStatuses_Id into rdg
                from rd in rdg.DefaultIfEmpty()
                join rx in _dbContext.I_GesCaseReportStatuses on rp.I_GesCaseReportStatuses_Id equals
                rx.I_GesCaseReportStatuses_Id into rdx
                from rx in rdx.DefaultIfEmpty()
                join response in _dbContext.I_ResponseStatuses on rp.I_ResponseStatuses_Id equals response.I_ResponseStatuses_Id into gresponse
                from response in gresponse.DefaultIfEmpty()
                join progress in _dbContext.I_ProgressStatuses on rp.I_ProgressStatuses_Id equals progress.I_ProgressStatuses_Id into gprogress
                from progress in gprogress.DefaultIfEmpty()


                join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                cret.I_GesCaseReports_Id
                join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                et.I_EngagementTypes_Id into etg
                from et in etg.DefaultIfEmpty()
                join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                from etc in etcg.DefaultIfEmpty()

                join ft in (from po in _dbContext.I_PortfoliosG_Organizations //check services in portfolio-services
                    join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id equals pos.I_PortfoliosG_Organizations_Id
                    join g in _dbContext.G_Services on pos.G_Services_Id equals g.G_Services_Id
                    join et in _dbContext.I_EngagementTypes on g.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                    join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                    join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                    join gc in _dbContext.I_GesCompanies on c.MasterI_Companies_Id != null ? c.MasterI_Companies_Id.Value : c.I_Companies_Id equals gc.I_Companies_Id
                    join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                    where po.G_Organizations_Id == orgId
                    group rp by new
                    {
                        rp.I_GesCaseReports_Id,
                        et.I_EngagementTypes_Id
                    } into g
                    select new
                    {
                        g.Key.I_GesCaseReports_Id,
                        g.Key.I_EngagementTypes_Id
                    }
                ) on new { rp.I_GesCaseReports_Id, et.I_EngagementTypes_Id } equals new { ft.I_GesCaseReports_Id, ft.I_EngagementTypes_Id }
                join s in _dbContext.G_Services on cret.I_EngagementTypes_Id equals s.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                join grce in _dbContext.I_GesCaseReportsExtra on rp.I_GesCaseReports_Id equals grce.I_GesCaseReports_Id into grceg
                from grce in grceg.DefaultIfEmpty()

                join gcm in (
                    from gmt in _dbContext.I_GesCommentary
                    group gmt by gmt.I_GesCaseReports_Id
                    into g
                    select g.OrderByDescending(d => d.CommentaryModified).FirstOrDefault()
                ) on rp.I_GesCaseReports_Id equals gcm.I_GesCaseReports_Id
                into gcmg
                from gcm in gcmg.DefaultIfEmpty()

                join dl in (from x in _dbContext.I_GesCompanyDialogues
                            where x.ClassA
                            group x by x.I_GesCaseReports_Id
                    into g
                            select g.OrderByDescending(d => d.ContactDate).FirstOrDefault()
                ) on rp.I_GesCaseReports_Id equals dl.I_GesCaseReports_Id
                into dlg
                from dl in dlg.DefaultIfEmpty()

                join gp in (from gp in _dbContext.I_GesCaseReportsI_ProcessStatuses where gp.I_ProcessStatuses_Id == 13 select gp) on rp.I_GesCaseReports_Id equals gp.I_GesCaseReports_Id into ggp
                from gp in ggp.DefaultIfEmpty()

                join su in _dbContext.GesCaseReportSignUp on new { I_GesCaseReports_Id = rp.I_GesCaseReports_Id, OrganizationId = orgId } equals new { I_GesCaseReports_Id = su.I_GesCaseReports_Id ?? -1, OrganizationId = su.G_Organizations_Id ?? -1 }
                into sugr
                from su in sugr.DefaultIfEmpty()

                join ri in _dbContext.I_GesCaseReportsG_Individuals on new { I_GesCaseReports_Id = rp.I_GesCaseReports_Id, IndividualId = individualId } equals new { I_GesCaseReports_Id = ri.I_GesCaseReports_Id, IndividualId = ri.G_Individuals_Id }
                into rig
                from ri in rig.DefaultIfEmpty()

                join ungp in _dbContext.GesUNGPAssessmentForm on rp.I_GesCaseReports_Id equals ungp.I_GesCaseReports_Id
                into ungpg from ungp in ungpg.DefaultIfEmpty()

                     where os.G_Organizations_Id == orgId && rp.ShowInClient && (notshowclosecase == false || (rp.NewI_GesCaseReportStatuses_Id != 3 && rp.NewI_GesCaseReportStatuses_Id != 9))

                      && (normId == null || rp.I_NormAreas_Id == normId)
                      && (!isSearchRecommendation || recommendationId.Contains(rp.NewI_GesCaseReportStatuses_Id)) && (!isSearchConclusion || conclusionIds.Contains(rp.I_GesCaseReportStatuses_Id))
                      && (!isSearchProgress || progressIds.Contains(rp.I_ProgressStatuses_Id)) && (!isSearchResponse || responseIds.Contains(rp.I_ResponseStatuses_Id))
                      && (!isSearchLocation || locationIds.Contains(rp.CountryId))
                      && (engagementTypeId == null || cret.I_EngagementTypes_Id == engagementTypeId)
                      && (!isSearchServices || serviceIds.Contains(s.G_Services_Id))
                select new CaseReportListViewModel
                {
                    Id = rp.I_GesCaseReports_Id,
                    GesCompanyId = rp.I_GesCompanies_Id,
                    IssueName = rp.ReportIncident,
                    Confirmed = et.I_EngagementTypes_Id == conventionsTypeId ? rp.Confirmed : new bool?(),
                    Location = l.Name,
                    TemplateId = (rp.I_GesCaseReportStatuses_Id == 1 || rp.I_GesCaseReportStatuses_Id == 11) ? 24 : 23, // Alert or Indication of violation => 24; else 23
                    Conclusion = rx.Name,
                    Recommendation = rd.Name,
                    EngagementTypeId = et.I_EngagementTypes_Id,
                    SortOrderEngagementType = et.SortOrder ?? 1,
                    SortOrderRecommendation = rd.SortOrder ?? 1,
                    Description = rp.Summary,
                    EntryDate = rp.EntryDate,
                    LastModified = rp.Modified,
                    Norm = n.Name,
                    EngagementType = et.Name,
                    EngagementTypeCategory = etc.Name,
                    Keywords = grce.Keywords,
                    ClosingDate = gp.DateChanged,
                    SignUpValue = su == null ? 0 : su.Active ? 1 : 2,
                    IsInFocusList = ri != null,
                    Response = response.ShortName,
                    Progress = progress.ShortName,
                    ProgressGrade = rp.I_ProgressStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ProgressStatuses_Id : 0,
                    ResponseGrade = rp.I_ResponseStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ResponseStatuses_Id : 0,
                    GesCommentary = gcm == null ? "" : gcm.Description,
                    CommentaryModified = gcm.CommentaryModified,
                    ChangeObjective = rp.ProcessGoal,
                    ProcessGoalModified = rp.ProcessGoalModified,
                    EngagementActivity = dl == null ? null : dl.ContactDate,
                    Created = rp.Created,
                    NewI_GesCaseReportStatuses_Id = rp.NewI_GesCaseReportStatuses_Id,
                    EngagementSince = rp.NewI_GesCaseReportStatuses_Id == 7 ? grce != null? grce.EngagementSince : rp.Created : null,
                    TotalScoreForCompanyPreparedness = ungp!=null? ungp.TotalScoreForCompanyPreparedness:null,
                    SalientHumanRightsPotentialViolationTotalScore = ungp != null ? ungp.SalientHumanRightsPotentialViolationTotalScore: null
                    
                }
            ).FromCache(DataHelper.GetCacheTags("GesCaseReport", orgId, individualId)).ToList();

            var result = from r in allCaseReports
                         group r by new
                {
                    r.Id,
                    r.GesCompanyId,
                    r.IssueName,
                    r.Confirmed,
                    r.Location,
                    r.TemplateId,
                    r.Conclusion,
                    r.Recommendation,
                    r.EngagementTypeId,
                    r.SortOrderEngagementType,
                    r.SortOrderRecommendation,
                    r.Description,
                    r.EntryDate,
                    r.LastModified,
                    r.Norm,
                    r.EngagementType,
                    r.EngagementTypeCategory,
                    r.Keywords,
                    r.ProgressGrade,
                    r.ResponseGrade,
                    r.ClosingDate,
                    r.SignUpValue,
                    r.IsInFocusList,
                    r.Response,
                    r.Progress,
                    r.EngagementSince,
                    r.GesCommentary,
                    r.CommentaryModified,
                    r.ChangeObjective,
                    r.ProcessGoalModified,
                    r.EngagementActivity,
                    r.Created,
                    r.NewI_GesCaseReportStatuses_Id,
                    r.TotalScoreForCompanyPreparedness,
                    r.SalientHumanRightsPotentialViolationTotalScore
                         }
                into g
                select new CaseReportListViewModel
                {
                    Id = g.Key.Id,
                    GesCompanyId = g.Key.GesCompanyId,
                    IssueName = g.Key.IssueName,
                    Confirmed = g.Key.Confirmed,
                    Location = g.Key.Location,
                    TemplateId = g.Key.TemplateId,
                    Conclusion = g.Key.Conclusion,
                    Recommendation = g.Key.Recommendation?.Replace("Resolved (Indication of Violation)", "Resolved"),
                    EngagementTypeId = g.Key.EngagementTypeId,
                    SortOrderEngagementType = g.Key.SortOrderEngagementType,
                    SortOrderRecommendation = g.Key.SortOrderRecommendation,
                    Description = g.Key.Description,
                    EntryDate = g.Key.EntryDate,
                    LastModified = g.Key.LastModified,
                    ServiceEngagementThemeNorm = DataHelper.GetEngagementThemeNorm(g.Key.EngagementTypeCategory, g.Key.Norm, g.Key.EngagementType, g.Key.EngagementTypeId??0),
                    Keywords = g.Key.Keywords,
                    ProgressGrade = g.Key.ProgressGrade,
                    ResponseGrade = g.Key.ResponseGrade,
                    ClosingDate = g.Key.ClosingDate,
                    SignUpValue = g.Key.SignUpValue,
                    IsInFocusList = g.Key.IsInFocusList,
                    DevelopmentGrade = DataHelper.CalcDevelopmentGrade(g.Key.ProgressGrade ?? 0, g.Key.ResponseGrade ?? 0),
                    Development = DataHelper.ConvertDevelopmentGradeToText(DataHelper.CalcDevelopmentGrade(g.Key.ProgressGrade ?? 0, g.Key.ResponseGrade ?? 0)),
                    Response = g.Key.Response,
                    Progress = g.Key.Progress,
                    GesCommentary = g.Key.GesCommentary,
                    ChangeObjective = g.Key.ChangeObjective,
                    EngagementActivity = g.Key.EngagementActivity,
                    EngagementSince = g.Key.EngagementSince,
                    TotalScoreForCompanyPreparedness = g.Key.TotalScoreForCompanyPreparedness,
                    SalientHumanRightsPotentialViolationTotalScore = g.Key.SalientHumanRightsPotentialViolationTotalScore
                };

            return result.ToList();
        }

        private string GetMileStoneText(long caseReportId)
        {
            StringBuilder bd = new StringBuilder();
            var querry = (from m in _dbContext.I_Milestones
                where m.I_GesCaseReports_Id == caseReportId
                orderby m.MilestoneModified descending
                select new
                {
                    m.Description,
                    m.MilestoneModified
                }).ToList();

            if (querry.Any())
            {
                foreach (var milestone in querry)
                {
                    bd.Append("- ");
                    bd.Append(milestone.MilestoneModified != null
                        ? milestone.MilestoneModified?.ToString(Configurations.DateFormat) + ": " + milestone.Description
                        : milestone.Description);
                    bd.Append("\n");
                }
            }

            return bd.ToString();
        }

        private string GetFirstMileStoneText(long caseReportId)
        {
            var bd = new StringBuilder();
            var querry = (from m in _dbContext.I_Milestones
                where m.I_GesCaseReports_Id == caseReportId
                orderby m.MilestoneModified descending
                select new
                {
                    m.Description,
                    m.MilestoneModified
                }).FirstOrDefault();

            if (querry == null) return null;
            
            bd.Append("- ");
            bd.Append(querry?.MilestoneModified != null
                ? querry.MilestoneModified?.ToString(Configurations.DateFormat) + ": " + querry.Description
                : querry?.Description);

            return bd.ToString();

        }

        public IEnumerable<ExcelCaseDetail> ExportEFStatusReport()
        {
            var query = from rp in _dbContext.I_GesCaseReports

                        join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id into ggc
                        from gc in ggc.DefaultIfEmpty()

                        join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id into gcc
                        from c in gcc.DefaultIfEmpty()

                        join ct in _dbContext.Countries on c.CountryOfIncorporationId equals ct.Id
                        into gct from ct in gct.DefaultIfEmpty()

                        join l in _dbContext.Countries on rp.CountryId equals l.Id
                        into gl
                        from l in gl.DefaultIfEmpty()

                        join n in _dbContext.I_NormAreas on rp.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                        from n in ng.DefaultIfEmpty()

                        join rd in _dbContext.I_GesCaseReportStatuses on rp.NewI_GesCaseReportStatuses_Id equals
                        rd.I_GesCaseReportStatuses_Id into rdg
                        from rd in rdg.DefaultIfEmpty()
                        join rx in _dbContext.I_GesCaseReportStatuses on rp.I_GesCaseReportStatuses_Id equals
                        rx.I_GesCaseReportStatuses_Id into rdx
                        from rx in rdx.DefaultIfEmpty()

                        join u in _dbContext.G_Users on rp.AnalystG_Users_Id equals u.G_Users_Id into gu from u in gu.DefaultIfEmpty()
                        join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id into gi from i in gi.DefaultIfEmpty()
               
                select new ExcelCaseDetail()
                {
                    CompanyId = c!= null? c.I_Companies_Id:0,
                    SustainalyticsID = c.Id,
                    CompanyName = c!=null? c.Name : "",
                    Isin = c!= null? c.Isin: "",
                    Analyst = i!= null?i.FirstName + " " + i.LastName: "",
                    IncidentId = rp.I_GesCaseReports_Id,
                    Incident = rp.ReportIncident,
                    HQLocation = ct.Name,
                    IncidentLocation = l.Name,
                    NormArea = n.Name,
                    Conclusion = rx.Name,
                    Recommendation = rd.Name
                };

            return query;
        }
    }
}
