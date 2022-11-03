using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Linq;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.StoredProcedureParams;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Repository
{
    public class I_CompaniesRepository : GenericRepository<I_Companies>, II_CompaniesRepository
    {
        private readonly GesEntities _dbContext;
        private readonly IStoredProcedureRunner _storedProcedureRunner;
        
        public I_CompaniesRepository(GesEntities context, IGesLogger logger, IStoredProcedureRunner storedProcedureRunner) : base(context, logger)
        {
            _dbContext = context;
            _storedProcedureRunner = storedProcedureRunner;
        }

        public IEnumerable<I_Companies> GetCompaniesByIsins(IEnumerable<string> isins)
        {
            return _dbset.Where(c => isins.Contains(c.Isin));
        }
        
        public IEnumerable<CompanyBriefInfo> GetCompaniesByTerm(string term, int limit)
        {
            return _dbset.Where(o => o.Name.Contains(term))
                .Select(o => new CompanyBriefInfo
                {
                    CompanyId = o.I_Companies_Id,
                    MasterCompanyId = o.MasterI_Companies_Id,
                    Name = o.Name,
                    MasterCompanyName = o.I_Companies2.Name,
                    Isin = o.Isin
                })
                .OrderBy(o => o.Name).Take(limit);
        }

        public IQueryable<CompanyViewModel> GetCompaniesWithLocation()
        {
            var companies = from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id into gcg from gc in gcg.DefaultIfEmpty()
                join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                into grp from rp in grp.DefaultIfEmpty()
                where (rp.I_NormAreas_Id == null || rp.I_NormAreas_Id != 6)
                group rp by new
                {
                    c.I_Companies_Id,
                    c.Name,
                    c.MasterI_Companies_Id,
                    c.Isin,
                    c.Sedol,
                    c.Website,
                    Location = c.Countries.Name,
                    sortPath = c.I_Companies2 != null
                            ? c.I_Companies2.Name.Trim() + "~~" + c.Name.Trim()
                            : c.Name.Trim()
                }
                into g
                select new CompanyViewModel
                {
                    Id = g.Key.I_Companies_Id,
                    Name = g.Key.MasterI_Companies_Id != null ? "  >>  " + g.Key.Name : g.Key.Name,
                    Isin = g.Key.Isin,
                    MasterCompanyId = g.Key.I_Companies_Id.ToString(),
                    Sedol = g.Key.Sedol,
                    Location = g.Key.Location,
                    parent = g.Key.MasterI_Companies_Id,
                    NumberOfCases = g.Count(d=>d != null),
                    sortPath = g.Key.sortPath,
                    IsMasterCompany = g.Key.MasterI_Companies_Id == null || g.Key.MasterI_Companies_Id.Value == g.Key.I_Companies_Id
                };
            return companies;
        }

        public IEnumerable<CompanyListViewModel> CompaniesAdvancedSearchQuery(long orgId, long individualId, AdvancedSearchParams parameters)
        {
            return _storedProcedureRunner.Execute<CompanyListViewModel>("dbo.CompanyAdvancedSearch", parameters.BuildStoredProcedureParams(), DataHelper.GetCacheTags("CompanySearch", orgId, individualId));
        }

        public IEnumerable<CompanyListViewModel> CompaniesNormalSearchQuery(long orgId, long individualId, NormalSearchParams parameters)
        {
            return _storedProcedureRunner.Execute<CompanyListViewModel>("dbo.CompanyNormalSearch", parameters.BuildStoredProcedureParams(), DataHelper.GetCacheTags("CompanySearch", orgId, individualId));
        }

        public IEnumerable<CompanyListViewModel> CompaniesFocusListQuery(long orgId, long individualId, FocusListParams parameters)
        {
            return _storedProcedureRunner.Execute<CompanyListViewModel>("dbo.CompanyFocusList", parameters.BuildStoredProcedureParams(), DataHelper.GetCacheTags("CompanySearch", orgId, individualId));
        }

        public IEnumerable<CheckMasterCompanyViewModel> CheckMasterCompanyQuery(long orgId, long individualId, CheckMasterCompanyParams parameters)
        {
            return _storedProcedureRunner.Execute<CheckMasterCompanyViewModel>("dbo.SearchCheckMasterCompany", parameters.BuildStoredProcedureParams(), DataHelper.GetCacheTags("SearchCheckMasterCompany", orgId, individualId));
        }

        public List<CompanyListViewModel> GetFilteredCompaniesFromCache(long orgId, long individualId, bool onlyShowFocusList, string name,
            string isin, List<long> portfolioIds, bool notshowclosecase, List<long?> recommendationIds, List<long?> conclusionIds, List<long?> serviceIds,
            long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId,
            List<Guid?> homeCountryIds, long? sustainalyticsId)
        {
            name = name?.Trim();
            isin = isin?.Trim();

            var advanceSearch = IsAdvancedSearch(recommendationIds, conclusionIds, serviceIds, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, sustainalyticsId);

            if (onlyShowFocusList)
            {
                return CompaniesFocusListQuery(orgId, individualId, new FocusListParams { IndividualId = individualId, OrgId = orgId }).ToList();
            }

            if (advanceSearch)
            {
                return CompaniesAdvancedSearchQuery(orgId, individualId, new AdvancedSearchParams
                {
                    CompanyIssueName = name,
                    IndividualId = individualId,
                    Isin = isin,
                    OrgId = orgId,
                    NormId = normId ?? SqlInt64.Null,
                    EngagementTypeId = engagementTypeId ?? SqlInt64.Null,
                    RecommendationIds = string.Join(",", recommendationIds ?? new List<long?>()),
                    ConclusionIds = string.Join(",", conclusionIds ?? new List<long?>()),
                    ProgressIds = string.Join(",", progressIds ?? new List<long?>()),
                    ResponseIds = string.Join(",", responseIds ?? new List<long?>()),
                    ServiceIds = string.Join(",", serviceIds ?? new List<long?>()),
                    LocationIds = string.Join(",", locationIds ?? new List<Guid?>()),
                    HomeCountryIds = string.Join(",", homeCountryIds ?? new List<Guid?>()),
                    PortfoliosOrganizationIds = string.Join(",", portfolioIds),
                    IndustryIds = string.Join(",", industryIds),
                    IsHideClosedCases = notshowclosecase,
                    SustainalyticsId = sustainalyticsId ?? SqlInt64.Null
                }).ToList();
            }
            return CompaniesNormalSearchQuery(orgId, individualId, new NormalSearchParams
            {
                CompanyIssueName = name,
                OrgId = orgId,
                IndividualId = individualId,
                IsHideClosedCases = notshowclosecase,
                PortfolioIds = string.Join(",", portfolioIds),
                Isin = isin,
                HomeCountryIds = string.Join(",", homeCountryIds ?? new List<Guid?>()),
                IndustryIds = string.Join(",", industryIds ?? new List<long?>())
            }).ToList();
        }

        public int CountCompanyDialogueByCompany(long orgId, long companyId, EngagementStatisticType engagementStatisticType)
        {
            var result = from cd in _dbContext.I_GesCompanyDialogues
                join gc in _dbContext.I_GesCaseReports on cd.I_GesCaseReports_Id equals gc.I_GesCaseReports_Id
                join c in _dbContext.I_GesCompanies on gc.I_GesCompanies_Id equals c.I_GesCompanies_Id
                join ge in _dbContext.I_GesCaseReportsI_EngagementTypes on gc.I_GesCaseReports_Id equals ge.I_GesCaseReports_Id
                join s in _dbContext.G_Services on ge.I_EngagementTypes_Id equals s.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                select new
                {
                    c.I_Companies_Id,
                    cd.I_GesCompanyDialogues_Id,
                    cd.ShowInCsc,
                    cd.I_ContactTypes_Id,
                    os.G_Organizations_Id,
                    cd.ClassA
                };
            switch (engagementStatisticType)
            {
                case EngagementStatisticType.Contacts:
                    return result.Count(x => x.I_Companies_Id == companyId && x.ShowInCsc && x.G_Organizations_Id == orgId);
                case EngagementStatisticType.Meetings:
                    return result.Count(x => x.I_Companies_Id == companyId && x.ShowInCsc && x.I_ContactTypes_Id == (int)ContactType.Meeeting && x.G_Organizations_Id == orgId);
                case EngagementStatisticType.Dialogues:
                    return result.Count(x => x.I_Companies_Id == companyId && x.ShowInCsc && x.G_Organizations_Id == orgId);
                case EngagementStatisticType.ConferenceCalls:
                    return result.Count(x => x.I_Companies_Id == companyId && x.ShowInCsc && x.I_ContactTypes_Id == (int)ContactType.ConferenceCall && x.G_Organizations_Id == orgId);
                case EngagementStatisticType.Emails:
                    return result.Count(x => x.I_Companies_Id == companyId && x.ShowInCsc && x.ClassA && x.I_ContactTypes_Id == (int)ContactType.Email && x.G_Organizations_Id == orgId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(engagementStatisticType), engagementStatisticType, null);
            }
        }

        public int CountCompanyDialogueByCaseProfile(long orgId, long caseProfileId, EngagementStatisticType engagementStatisticType)
        {
            var result = from cd in _dbContext.I_GesCompanyDialogues
                join gc in _dbContext.I_GesCaseReports on cd.I_GesCaseReports_Id equals gc.I_GesCaseReports_Id
                join c in _dbContext.I_GesCompanies on gc.I_GesCompanies_Id equals c.I_GesCompanies_Id
                join ge in _dbContext.I_GesCaseReportsI_EngagementTypes on gc.I_GesCaseReports_Id equals ge.I_GesCaseReports_Id
                join s in _dbContext.G_Services on ge.I_EngagementTypes_Id equals s.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                select new
                {
                    cd.I_GesCompanyDialogues_Id,
                    cd.ShowInCsc,
                    cd.I_ContactTypes_Id,
                    os.G_Organizations_Id,
                    gc.I_GesCaseReports_Id,
                    cd.ClassA
                };
            switch (engagementStatisticType)
            {
                case EngagementStatisticType.Contacts:
                    return result.Count(x => x.ShowInCsc && x.G_Organizations_Id == orgId && caseProfileId == x.I_GesCaseReports_Id);
                case EngagementStatisticType.Meetings:
                    return result.Count(x => x.ShowInCsc && x.I_ContactTypes_Id == (int)ContactType.Meeeting && x.G_Organizations_Id == orgId && caseProfileId == x.I_GesCaseReports_Id);
                case EngagementStatisticType.ConferenceCalls:
                    return result.Count(x => x.ShowInCsc && x.I_ContactTypes_Id == (int)ContactType.ConferenceCall && x.G_Organizations_Id == orgId && caseProfileId == x.I_GesCaseReports_Id);
                case EngagementStatisticType.Emails:
                    return result.Count(x => x.ShowInCsc && x.ClassA && x.I_ContactTypes_Id == (int)ContactType.Email && x.G_Organizations_Id == orgId && caseProfileId == x.I_GesCaseReports_Id);
                default:
                    throw new ArgumentOutOfRangeException(nameof(engagementStatisticType), engagementStatisticType, null);
            }
        }

        public string GetLatestConferenceAndMeetingTime(long orgId, long caseProfileId)
        {
            var result = from cd in _dbContext.I_GesCompanyDialogues
                join gc in _dbContext.I_GesCaseReports on cd.I_GesCaseReports_Id equals gc.I_GesCaseReports_Id
                join c in _dbContext.I_GesCompanies on gc.I_GesCompanies_Id equals c.I_GesCompanies_Id
                join ge in _dbContext.I_GesCaseReportsI_EngagementTypes on gc.I_GesCaseReports_Id equals ge.I_GesCaseReports_Id
                join s in _dbContext.G_Services on ge.I_EngagementTypes_Id equals s.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                where (cd.I_ContactTypes_Id == (int)ContactType.Meeeting || cd.I_ContactTypes_Id == (int)ContactType.ConferenceCall) 
                    && os.G_Organizations_Id == orgId && caseProfileId == gc.I_GesCaseReports_Id && cd.ShowInCsc
                orderby cd.ContactDate descending 
                select new
                {
                    cd.ContactDate
                };
            return result.FirstOrDefault()?.ContactDate?.ToString(Configurations.DateFormat);
        }

        public long GetCompanyIdFromCaseProfile(long caseProfileId)
        {
            return SafeExecute(() =>
            {
                var companyId = (from gcr in _dbContext.I_GesCaseReports
                    join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                    join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                    where gcr.I_GesCaseReports_Id == caseProfileId
                    select c.I_Companies_Id).FirstOrDefault();
                return companyId;
            });
        }

        public IList<I_EngagementDiscussionPoints> GetEngagementDiscussionPointsByCompanyId(long companyId)
        {
            return SafeExecute(() =>
            {
                var discussionPoints = from dp in _dbContext.I_EngagementDiscussionPoints
                                       join c in _dbContext.I_Companies on dp.I_Companies_Id equals c.I_Companies_Id
                                       where dp.I_Companies_Id == companyId
                                       select dp;

                return discussionPoints.ToList();
            });
        }
        
        public string GetInvestorInitiatives(long companyId)
        {
            return SafeExecute(() => (from e in _dbContext.I_EngagementProfiles
                join c in _dbContext.I_Companies on e.I_Companies_Id equals c.I_Companies_Id
                where c.I_Companies_Id == companyId
                select e.InvestorInitiatives).FirstOrDefault());
        }

        public IList<I_EngagementOtherStakeholderViews> GetStakeholderViews(long companyId)
        {
            return SafeExecute(() => _dbContext.I_EngagementOtherStakeholderViews.Where(o => o.I_Companies_Id == companyId).ToList());
        }

        public I_Companies GetCompanyById(long? companyId)
        {
            return SafeExecute(() => _dbContext.I_Companies.FirstOrDefault(x => x.I_Companies_Id == companyId));
        }

        private static bool IsAdvancedSearch(List<long?> recommendationIds, List<long?> conclusionIds, List<long?> serviceIds, long? normId, 
            List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId, long? sustainalyticsId)
        {
            var isRecommendationSearch = recommendationIds != null && recommendationIds.Any(i => i != null);
            var isConclusionSearch = conclusionIds != null && conclusionIds.Any(i => i != null);
            var isResponseSearch = responseIds != null && responseIds.Any(i => i != null);
            var isProgressSearch = progressIds != null && progressIds.Any(i => i != null);
            var isLocationSearch = locationIds != null && locationIds.Any(i => i != null);
            //var isIndustrySearch = industryIds != null && industryIds.Any(i => i != null);
            var isServiceSearch = serviceIds != null && serviceIds.Any(i => i != null);

            return isRecommendationSearch || isConclusionSearch || isServiceSearch || normId != null ||
                isLocationSearch || isResponseSearch || isProgressSearch || engagementTypeId != null || sustainalyticsId != null;
        }

        public IList<ContactTypeViewModel> GetContactTypes()
        {
            return SafeExecute(() =>
            {
                var contactTypes = from ct in _dbContext.I_ContactTypes                                       
                                       select new ContactTypeViewModel()
                                       {
                                           ContactTypeId = ct.I_ContactTypes_Id,
                                           Name = ct.Name
                                       };

                return contactTypes.FromCache().ToList();
            });
        }

        public IList<CompanyManagementSystemModel> GetCompanyManagementSystems(long companyId)
        {
            return SafeExecute(() =>
            {
                var managementSystems = from cms in _dbContext.I_CompaniesI_ManagementSystems
                                        join ms in _dbContext.I_ManagementSystems on cms.I_ManagementSystems_Id equals ms.I_ManagementSystems_Id
                                        where cms.I_Companies_Id == companyId
                                        select new CompanyManagementSystemModel()
                                        {
                                            I_CompaniesI_ManagementSystems_Id = cms.I_CompaniesI_ManagementSystems_Id,
                                            I_Companies_Id = cms.I_Companies_Id,
                                            I_ManagementSystems_Id = cms.I_ManagementSystems_Id,
                                            ManagementSystemsName = ms.Name,
                                            Certification = cms.Certification,
                                            Coverage = cms.Coverage,
                                            Created = cms.Created,
                                            ModifiedByG_Users_Id = cms.ModifiedByG_Users_Id
                                        };

                return managementSystems.ToList();
            });
        }

        public IList<CompanyPortfolioModel> GetCompanyPortfolioModels(long companyId)
        {
            return SafeExecute(() =>
            {
                var result = (from c in _dbContext.I_Companies
                              join ic in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals ic.I_Companies_Id into ps
                              from ic in ps.DefaultIfEmpty()
                              join p in _dbContext.I_Portfolios on ic.I_Portfolios_Id equals p.I_Portfolios_Id into pi
                              from p in pi.DefaultIfEmpty()
                              join o in _dbContext.G_Organizations on p.G_Organizations_Id equals o.G_Organizations_Id into pg
                              from g in pg.DefaultIfEmpty()
                              where (c.I_Companies_Id == companyId && c.ApprovedGesCompany == false)
                              orderby p.Created
                              select new CompanyPortfolioModel
                              {
                                  PortfolioId = p.I_Portfolios_Id,
                                  PortfolioName = p.Name??"",
                                  OrganizationId = g.G_Organizations_Id,
                                  OrganizationName = g.Name ?? "",
                                  OrgNr = g.OrgNr ?? "",
                                  Phone = g.Phone ?? "",
                                  Created = p.Created
                              }).ToList();
                return result;
            });
        }

        public IList<CompanyPortfolioModel> GetCompanyFollowingPortfolioModels(long companyId)
        {
            return SafeExecute(() =>
            {
                var result = (from pc in _dbContext.I_PortfoliosI_Companies
                              join p in _dbContext.I_Portfolios on pc.I_Portfolios_Id equals p.I_Portfolios_Id
                              join po in _dbContext.I_PortfoliosG_Organizations on p.I_Portfolios_Id equals po.I_Portfolios_Id
                              join og in _dbContext.G_Organizations on po.G_Organizations_Id equals og.G_Organizations_Id
                              where (pc.I_Companies_Id == companyId)                              
                              select new CompanyPortfolioModel
                              {
                                  PortfolioName = p.Name ?? "",
                                  OrganizationName = og.Name ?? "",
                                  Created = p.Created,
                                  PortfolioTypeId = p.I_PortfolioTypes_Id
                              }).Distinct().ToList();
                return result;
            });
        }

        public IEnumerable<MsciFtseViewModel> GetMsciViewModels(long orgId, long individualId)
        {
            return _storedProcedureRunner.Execute<MsciFtseViewModel>("dbo.I_SelectMsci_New", null, DataHelper.GetCacheTags("CompanyMsci", orgId, individualId));
        }

        public IEnumerable<MsciFtseViewModel> GetFtseViewModels(long orgId, long individualId)
        {
            return _storedProcedureRunner.Execute<MsciFtseViewModel>("dbo.I_SelectFtse_New", null, DataHelper.GetCacheTags("CompanyFtse", orgId, individualId));
        }

        public IQueryable<CompanyViewModel> GetGesCompaniesWithLocation()
        {
            var companies = from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                select new CompanyViewModel
                {
                    Id = c.I_Companies_Id,
                    Name = c.MasterI_Companies_Id != null ? "  >>  " + c.Name : c.Name,
                    Isin = c.Isin,
                    MasterCompanyId = c.I_Companies_Id.ToString(),
                    Sedol = c.Sedol,
                    Location = c.Countries.Name,
                    parent = c.MasterI_Companies_Id,
                    sortPath = c.I_Companies2 != null
                        ? c.I_Companies2.Name.Trim() + "~~" + c.Name.Trim()
                        : c.Name.Trim(),
                    IsMasterCompany = c.MasterI_Companies_Id == null || c.MasterI_Companies_Id.Value == c.I_Companies_Id
                };
            return companies;
        }
    }
}
