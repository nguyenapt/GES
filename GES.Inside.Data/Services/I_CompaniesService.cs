using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using GES.Common.Resources;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.StoredProcedureParams;

namespace GES.Inside.Data.Services
{
    public class I_CompaniesService : EntityService<GesEntities, I_Companies>, II_CompaniesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_CompaniesRepository _companiesRepository;
        private readonly II_GesCaseReportsRepository _gesCaseReportsRepository;
        private readonly IDocumentService _documentService;

        public I_CompaniesService(IUnitOfWork<GesEntities> unitOfWork, II_CompaniesRepository companiesRepository, II_GesCaseReportsRepository gesCaseReportsRepository, IDocumentService documentService, IGesLogger logger)
            : base(unitOfWork, logger, companiesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _companiesRepository = companiesRepository;
            _gesCaseReportsRepository = gesCaseReportsRepository;
            _documentService = documentService;
        }

        public List<IdNameModel> GetCompaniesWithTerm(string term, int limit)
        {
            var query = _companiesRepository.GetCompaniesByTerm(term, limit);
            return SafeExecute(() => query.Select(o => new IdNameModel
            {
                Id = o.MasterCompanyId ?? o.CompanyId,
                Name = o.MasterCompanyId != null ? 
                    $"#{o.MasterCompanyId}. {o.MasterCompanyName} >> {o.Name}"
                    : $"#{o.CompanyId}. {o.Name}"
            }).ToList());
        }

        public IList<KeyValueObject<string, string>> GetCompaniesAndSubCompaniesWithTerm(string term, int limit)
        {
            var query = _companiesRepository.GetCompaniesByTerm(term, limit);
            return SafeExecute(() => query.Select(o => new KeyValueObject<string,string>
            { 
                Key = o.Isin,
                Value = o.MasterCompanyId != null ?
                    $"#{o.CompanyId}. {o.Name} ({o.MasterCompanyName})" 
                    : $"#{o.CompanyId}. {o.Name}"
            }).ToList());
        }

        public IEnumerable<I_Companies> GetCompaniesByListIsin(IEnumerable<string> isinCodes)
        {
            const int maximumBatchSize = 5000;
            var next = 0;
            var isinList = isinCodes.ToList();
            var numberOfIsin = isinList.Count;

            if (numberOfIsin == 0)
                return new List<I_Companies>();

            var companiesByIsin = new List<I_Companies>();

            SafeExecute(() =>
            {
                while (true)
                {
                    var batchSize = (next + maximumBatchSize) <= numberOfIsin ? maximumBatchSize : numberOfIsin - next;
                    if (batchSize <= 0)
                        break;

                    var isinRange = isinList.GetRange(next, batchSize);
                    var companies = _companiesRepository.FindBy(d => d.Isin != null && isinRange.Contains(d.Isin));
                    companiesByIsin.AddRange(companies);
                    next += maximumBatchSize;
                }
            });

            return companiesByIsin;
        }

        public IEnumerable<I_Companies> GetCompaniesByListIds(IEnumerable<long> companyIds)
        {
            return SafeExecute(() => _companiesRepository.FindBy(d => companyIds.Contains(d.I_Companies_Id)));
        }

        public PaginatedResults<CompanyViewModel> GetCompanies(JqGridViewModel jqGridParams)
        {
            const int maximumRowsCanGetRelatedRows = 2000;
            var companies = _companiesRepository.GetCompaniesWithLocation();
            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CompanyViewModel>(jqGridParams);
                var filteredCompanies = string.IsNullOrEmpty(finalRules) ? companies : companies.Where(finalRules);

                if (filteredCompanies.Count() < maximumRowsCanGetRelatedRows)
                {
                    var companyIdsWithParentIds = SafeExecute(() => filteredCompanies.Select(d => new {Id = d.Id, parentId = d.parent}).ToList());
                    var companyIds = companyIdsWithParentIds.Select(d => d.Id).ToList();
                    var parentCompanyIds = companyIdsWithParentIds.Where(d => d.parentId != null).Select(d => d.parentId).Distinct().ToList();
                    var masterCompantIds = companyIdsWithParentIds.Where(d => d.parentId == null).Select(d => d.Id).ToList();

                    companies = companies.Where(c => parentCompanyIds.Contains(c.Id) || companyIds.Contains(c.Id) || (c.parent != null && masterCompantIds.Contains(c.parent.Value)));
                }
                else
                {
                    companies = filteredCompanies;
                }
            }

            return companies.OrderBy(x => x.sortPath).ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public PaginatedResults<CompanyViewModel> GetMasterCompanies(JqGridViewModel jqGridParams)
        {
            var companies = _companiesRepository.GetCompaniesWithLocation().Where(x => x.parent == null);

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CompanyViewModel>(jqGridParams);
                companies = string.IsNullOrEmpty(finalRules) ? companies : companies.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        companies = sortDir == "asc"
                            ? companies.OrderBy(x => x.Name)
                            : companies.OrderByDescending(x => x.Name);
                        break;
                    case "mastercompanyid":
                        companies = sortDir == "asc"
                            ? companies.OrderBy(x => x.MasterCompanyId).ThenBy(d => d.Name)
                            : companies.OrderByDescending(x => x.MasterCompanyId).ThenBy(d => d.Name);
                        break;
                    case "isin":
                        companies = sortDir == "asc"
                            ? companies.OrderBy(x => x.Isin).ThenBy(d => d.Name)
                            : companies.OrderByDescending(x => x.Isin).ThenBy(d => d.Name);
                        break;
                    case "sedol":
                        companies = sortDir == "asc"
                            ? companies.OrderBy(x => x.Sedol).ThenBy(d => d.Name)
                            : companies.OrderByDescending(x => x.Sedol).ThenBy(d => d.Name);
                        break;
                    case "location":
                        companies = sortDir == "asc"
                            ? companies.OrderBy(x => x.Location).ThenBy(d => d.Name)
                            : companies.OrderByDescending(x => x.Location).ThenBy(d => d.Name);
                        break;
                }
            }

            return companies.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public string GetMaximumCustomIsinCode()
        {
            const string customIsinBegining = "C_";
            var latestCompany = SafeExecute(() => _companiesRepository.FindBy(d => d.Isin.StartsWith(customIsinBegining))
                .OrderByDescending(d => d.Isin)
                .FirstOrDefault());

            return latestCompany?.Isin;
        }

        public IEnumerable<I_Companies> GetCompaniesWithErrorIsinCode()
        {
            const int validIsinLength = 12;
            return SafeExecute(() => _companiesRepository.FindBy(d => d.Isin == null || d.Isin.Trim().Length != validIsinLength || d.Isin.Trim().Contains(" ")));
        }

        public IEnumerable<I_Companies> GetCompaniesWithWhiteSpaceIsinCode()
        {
            return SafeExecute(() => _companiesRepository.FindBy(d => d.Isin.Contains(" ")));
        }

        #region Clients site
        
        public PaginatedResults<CompanyListViewModel> GetCompanyCasesGrid(JqGridViewModel jqGridParams, long orgId,
            long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notShowCloseCase, bool onlyCompaniesWithActiveCase,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds,
            List<long?> industryIds, long? engagementTypeId, List<Guid?> homeCountryIds, long? companyId, long? sustainalyticsId)
        {
            var result = _companiesRepository.GetFilteredCompaniesFromCache(orgId, individualId, onlyShowFocusList, name, isin, portfolioIds, notShowCloseCase, ProcessRecommendation(recommendationId), conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, homeCountryIds, sustainalyticsId);
            
            result = SortResults(jqGridParams, name, result, companyId);

            var messageIfNotFound = "";

            if (result.Count == 0 && (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrEmpty(isin) )&&
                portfolioIds.Count == 0 && recommendationId.Count ==0 && conclusionIds.Count==0 && serviceId.Count ==0 && locationIds.Count ==0 && responseIds.Count ==0 && CheckCompanyInGesUniverse(name, isin))
            {
                messageIfNotFound = Resources.CompanyOutsideOfPortfolio;
            }

            if (result.Count == 1 && (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrEmpty(isin)))
            {
                var listSub = _companiesRepository.CheckMasterCompanyQuery(orgId, individualId, new CheckMasterCompanyParams{CompanyIssueName = name, Isin = isin, OrgId = orgId, PortfolioIds = string.Join(",", portfolioIds) }).ToList();

                foreach (var checkMasterCompanyViewModel in listSub)
                {
                    if(result.Any(d=>d.CompanyId == checkMasterCompanyViewModel.MasterCompanyId && d.CompanyName.ToLower() != checkMasterCompanyViewModel.SubCompanyName.ToLower()))
                    {
                        var company = result.FirstOrDefault(d => d.CompanyId == checkMasterCompanyViewModel.MasterCompanyId &&
                                                       d.CompanyName != checkMasterCompanyViewModel.SubCompanyName);

                        if (company != null)
                        {
                            messageIfNotFound = string.Format("Sustainalytics covers {0} through research on a related entity in our research universe, {1}. Please see below available research for {1}.", checkMasterCompanyViewModel.SubCompanyName, company.CompanyName);
                        }

                        break;
                    }
                }
            }
            return result.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows, messageIfNotFound);
        }

        private bool CheckCompanyInGesUniverse(string companyName, string isin)
        {
            var querry = from p in _dbContext.I_Portfolios
                         join po in _dbContext.I_PortfoliosG_Organizations on p.I_Portfolios_Id equals po.I_Portfolios_Id
                join pc in _dbContext.I_PortfoliosI_Companies on p.I_Portfolios_Id equals pc.I_Portfolios_Id
                join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                where po.G_Organizations_Id == 3485 && c.MasterI_Companies_Id == null && ((companyName != null && (c.Name.Contains(companyName.Trim()) || c.Isin == companyName.Trim())) || (isin != null && c.Isin == isin.Trim())) && (from x in _dbContext.I_PortfoliosG_OrganizationsG_Services
                                                                                                 join os in _dbContext.G_OrganizationsG_Services on x.G_Services_Id equals os.G_Services_Id
                                                                                                 join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                                                                                                 where x.I_PortfoliosG_Organizations_Id == po.I_PortfoliosG_Organizations_Id
                                                                                                 && (s.I_EngagementTypes_Id != null || s.G_Services_Id == (long)GesService.GesGlobalEthicalStandard)
                                                                                                 && os.G_Organizations_Id == 3485
                                                                                                 select x.I_PortfoliosG_OrganizationsG_Services_Id).Any()
                select c.I_Companies_Id;

            return querry.Any();
        }

        private List<long?> ProcessRecommendation(List<long?> recommendationIds)
        {
            if (recommendationIds != null &&
                recommendationIds.Any(d => d.HasValue && d.Value == (long) RecommendationType.Resolved))
            {
                recommendationIds.Add((long)RecommendationType.ResolvedIndicationOfViolation);
            }
            return recommendationIds;
        }

        private static List<CompanyListViewModel> SortResults(JqGridViewModel jqGridParams, string keyword, List<CompanyListViewModel> result, long? companyId)
        {
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)) return result;

            switch (sortCol)
            {
                case "companyissuename":
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.CompanyName));
                case "homecountry":
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.HomeCountry));
                case "numcases":
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.NumCases));
                case "numalerts":
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.NumAlerts));
                case "isinfocuslist":
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.IsInFocusList));
                case "marketcap":
                    if (!string.IsNullOrEmpty(keyword) && keyword.Length > 0)
                    {
                        UpdateSortOrder(keyword, result, companyId);
                        
                        return result.OrderBy(x => x.SortOrder)
                            .ThenByDescending(d => d.MarketCap)
                            .ThenBy(d => d.CompanyName)
                            .ToList();
                    }
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.CompanyName));
                default:
                    return GetSortedResults(result, sortDir, nameof(CompanyListViewModel.CompanyName));
            }
        }

        private static List<CompanyListViewModel> GetSortedResults(IList<CompanyListViewModel> result, string sortDir, string sortBy)
        {
            return sortDir == "asc"
                ? result.OrderBy(x => x.GetValueByPropertyName(sortBy)).ThenBy(d => d.CompanyName).ToList()
                : result.OrderByDescending(x => x.GetValueByPropertyName(sortBy)).ThenByDescending(d => d.CompanyName).ToList();
        }

        private static void UpdateSortOrder(string keyword, IList<CompanyListViewModel> result, long? companyId)
        {
            var companyIdSelected = companyId ?? 0;
            foreach (var company in result)
            {
                if (companyIdSelected > 0 && company.CompanyId == companyIdSelected)
                {
                    company.SortOrder = -1;
                }
                else
                {
                    company.SortOrder = GetSortOrderByName(keyword, company.CompanyName);
                }
            }
        }

        private static int GetSortOrderByName(string keyword, string companyName)
        {
            return companyName.StartsWith(keyword, StringComparison.OrdinalIgnoreCase)
                ? (int)SortOrderByKeyword.FirstOrder
                : companyName.Contains(" " + keyword + " ", StringComparison.OrdinalIgnoreCase)
                    ? (int)SortOrderByKeyword.SecondOrder
                    : companyName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        ? (int)SortOrderByKeyword.ThirdOrder
                        : (int)SortOrderByKeyword.FourthOrder;
        }
        
        public List<CaseReportListViewModel> GetCaseReportsData(long gesCompanyId, long companyId, bool notshowclosecase, long orgId, long individualId, bool onlyShowFocusList, bool companyInfocusList, string keyword, List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId)
        {
            var caseReportsForCompany = GetReportListViewModelForSubGrid(new List<long> {gesCompanyId}, companyId, notshowclosecase, orgId, individualId, keyword, recommendationId, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId);
            return caseReportsForCompany.Where(d => (!onlyShowFocusList || companyInfocusList || d.IsInFocusList)) 
                .OrderByDescending(d => d.SortOrderEngagementType)
                .ThenBy(d => d.SortOrderRecommendation)
                .ThenBy(d => d.IssueName).ToList();
        }

        private bool IsGlobalEthicalStandard(long orgId)
        {
            var hasGlobalEthicalService = (from o in _dbContext.G_OrganizationsG_Services
                where o.G_Organizations_Id == orgId && o.G_Services_Id == (long)GesService.GesGlobalEthicalStandard
                select o.G_OrganizationsG_Services_Id);

            var hasBusinessConductService = (from os in _dbContext.G_OrganizationsG_Services
                join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                where os.G_Organizations_Id == orgId && et.I_EngagementTypeCategories_Id ==
                      (long)EngagementTypeCategoryEnum.BusinessConduct
                select os.G_OrganizationsG_Services_Id
            );

            return hasGlobalEthicalService.Any() && !hasBusinessConductService.Any();
        }

        public ClientType GetClientType(long orgId)
        {
            var hasBusinessConductService = (from os in _dbContext.G_OrganizationsG_Services
                join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                where os.G_Organizations_Id == orgId && et.I_EngagementTypeCategories_Id ==
                      (long)EngagementTypeCategoryEnum.BusinessConduct
                select os.G_OrganizationsG_Services_Id
            );

            if(hasBusinessConductService.Any()) return ClientType.BusinessConduct;

            var hasStewardshipAndRisk = (from os in _dbContext.G_OrganizationsG_Services
                                         join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                                         join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                         where os.G_Organizations_Id == orgId
                                         select os.G_OrganizationsG_Services_Id
            );

            var hasOtherEngagment = hasStewardshipAndRisk.Any();

            var hasGlobalEthicalService = (from o in _dbContext.G_OrganizationsG_Services
                where o.G_Organizations_Id == orgId && o.G_Services_Id == (long)GesService.GesGlobalEthicalStandard
                select o.G_OrganizationsG_Services_Id);

            var hasGlobalEthical = hasGlobalEthicalService.Any();

            if(hasOtherEngagment && hasGlobalEthical) return ClientType.GlobalEthicalStandardAndOtherEngagmement;
            else if(hasGlobalEthical) return ClientType.GlobalEthicalStandardOnly;

            return ClientType.Other;
        }

        private List<CaseReportListViewModel> GetReportListViewModelForSubGrid(IList<long> gesCompanyId, long companyId, bool notshowclosecase, long orgId, long individualId, string keyword, List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId)
        {
            long conventionsTypeId = 2;

            recommendationId = ProcessRecommendation(recommendationId);
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

                                  join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                                    cret.I_GesCaseReports_Id
                                  join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                                      et.I_EngagementTypes_Id into etg
                                  from et in etg.DefaultIfEmpty()
                                  join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                                  from etc in etcg.DefaultIfEmpty()

                                  join fm in _dbContext.G_ForumMessages on rp.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg from fm in fmg.DefaultIfEmpty()
                                  join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg from fmt in fmtg.DefaultIfEmpty()

                                  join s in _dbContext.G_Services on cret.I_EngagementTypes_Id equals s.I_EngagementTypes_Id into gs from s in gs.DefaultIfEmpty()

                                  join grce in _dbContext.I_GesCaseReportsExtra on rp.I_GesCaseReports_Id equals grce.I_GesCaseReports_Id into grceg
                                  from grce in grceg.DefaultIfEmpty()

                                  join gp in (from gp in _dbContext.I_GesCaseReportsI_ProcessStatuses where gp.I_ProcessStatuses_Id == (long)ProcessStatus.Closed select gp) on rp.I_GesCaseReports_Id equals gp.I_GesCaseReports_Id into ggp
                                  from gp in ggp.DefaultIfEmpty()

                                  join su in _dbContext.GesCaseReportSignUp on new { I_GesCaseReports_Id = rp.I_GesCaseReports_Id, OrganizationId = orgId } equals new { I_GesCaseReports_Id = su.I_GesCaseReports_Id ?? -1, OrganizationId = su.G_Organizations_Id ?? -1 }
                                  into sugr
                                  from su in sugr.DefaultIfEmpty()

                                  join ri in _dbContext.I_GesCaseReportsG_Individuals on new { I_GesCaseReports_Id = rp.I_GesCaseReports_Id, IndividualId = individualId } equals new { I_GesCaseReports_Id = ri.I_GesCaseReports_Id, IndividualId = ri.G_Individuals_Id }
                                  into rig
                                  from ri in rig.DefaultIfEmpty()

                                  where gesCompanyId.Contains(rp.I_GesCompanies_Id) && rp.ShowInClient && (notshowclosecase == false || (rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.Resolved && rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.ResolvedIndicationOfViolation && rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.ArchivedOfRecommendation && rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.ResolvedIndicationOfViolation))

                                  && (normId == null || rp.I_NormAreas_Id == normId)
                                   && (!isSearchRecommendation || recommendationId.Contains(rp.NewI_GesCaseReportStatuses_Id)) && (!isSearchConclusion || conclusionIds.Contains(rp.I_GesCaseReportStatuses_Id))
                                   && (!isSearchProgress || progressIds.Contains(rp.I_ProgressStatuses_Id)) && (!isSearchResponse || responseIds.Contains(rp.I_ResponseStatuses_Id))
                                   && (!isSearchLocation || locationIds.Contains(rp.CountryId))
                                  && (engagementTypeId == null || cret.I_EngagementTypes_Id == engagementTypeId)
                                  && (!isSearchServices || (s != null && serviceIds.Contains(s.G_Services_Id)))
                                  select new CaseReportListViewModel
                                  {
                                      Id = rp.I_GesCaseReports_Id,
                                      GesCompanyId = rp.I_GesCompanies_Id,
                                      IssueName = rp.ReportIncident,
                                      Confirmed = et.I_EngagementTypes_Id == conventionsTypeId ? rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ConfirmedViolation : new bool?(),
                                      Location = l.Name,
                                      TemplateId = (rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.IndicationOfViolation || rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.Alert) ? (long)ReportingTemplates.StandardIndicationViolations : (long)ReportingTemplates.StandardConfirmedViolations,
                                      Conclusion = rx.Name,
                                      Recommendation = rd.Name,
                                      EngagementTypeId = et.I_EngagementTypes_Id,
                                      SortOrderEngagementType = et.SortOrder ?? 1,
                                      SortOrderRecommendation = rd.SortOrder ?? 1,
                                      Description = rp.Summary,
                                      EntryDate = rp.EntryDate ?? rp.Created,
                                      LastModified = rp.Modified,
                                      Norm = n.Name,
                                      NormId = rp.I_NormAreas_Id,
                                      EngagementType = et.Name,
                                      EngagementTypeCategory = etc.Name,
                                      Keywords = grce.Keywords,
                                      ProgressGrade = rp.I_ProgressStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ProgressStatuses_Id : 0,
                                      ResponseGrade = rp.I_ResponseStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ResponseStatuses_Id : 0,
                                      ClosingDate = gp.DateChanged,
                                      SignUpValue = su == null ? (int)SignUpValue.None : su.Active ? (int)SignUpValue.Active : (int)SignUpValue.Passive,
                                      IsInFocusList = ri != null,
                                      EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                                      ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                                      ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null,
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
                             r.NormId,
                             r.EngagementType,
                             r.EngagementTypeCategory,
                             r.Keywords,
                             r.ProgressGrade,
                             r.ResponseGrade,
                             r.ClosingDate,
                             r.SignUpValue,
                             r.IsInFocusList,
                             r.EngagementTypeCategoriesId,
                             r.ParentForumMessagesId,
                             r.ForumMessagesTreeId
                         }
                into g
                         select new CaseReportListViewModel
                         {
                             Id = g.Key.Id,
                             GesCompanyId = g.Key.GesCompanyId,
                             IssueName = g.Key.IssueName,
                             Confirmed = g.Key.Confirmed??false,
                             Location = g.Key.Location,
                             TemplateId = g.Key.TemplateId,
                             Conclusion = g.Key.Conclusion,
                             Recommendation = g.Key.Recommendation != null? g.Key.Recommendation.Replace("Resolved (Indication of Violation)", "Resolved"): null,
                             EngagementTypeId = g.Key.EngagementTypeId,
                             SortOrderEngagementType = g.Key.SortOrderEngagementType,
                             SortOrderRecommendation = g.Key.SortOrderRecommendation,
                             Description = g.Key.Description,
                             EntryDate = g.Key.EntryDate,
                             LastModified = g.Key.LastModified,
                             ServiceEngagementThemeNorm = DataHelper.GetEngagementThemeNorm(g.Key.EngagementTypeCategory, g.Key.Norm, g.Key.EngagementType, g.Key.EngagementTypeId ?? 0),
                             NormId = g.Key.NormId,
                             Keywords = g.Key.Keywords,
                             ProgressGrade = g.Key.ProgressGrade,
                             ResponseGrade = g.Key.ResponseGrade,
                             ClosingDate = g.Key.ClosingDate,
                             SignUpValue = g.Key.SignUpValue,
                             IsInFocusList = g.Key.IsInFocusList,
                             DevelopmentGrade = DataHelper.CalcDevelopmentGrade(g.Key.ProgressGrade ?? 0, g.Key.ResponseGrade ?? 0),
                             CompanyId = companyId,
                             KeywordMatched = !string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(g.Key.Keywords) && g.Key.Keywords.Contains(keyword),
                             EngagementTypeCategoriesId = g.Key.EngagementTypeCategoriesId,
                             ParentForumMessagesId = g.Key.ParentForumMessagesId,
                             ForumMessagesTreeId = g.Key.ForumMessagesTreeId
                         };
            var caseReports = result.ToList();
            _gesCaseReportsRepository.UpdateCaseReportConfirmed(caseReports);
            _gesCaseReportsRepository.UpdateCaseReportKPI(caseReports);
            _gesCaseReportsRepository.UpdateCaseReportSubscribed(caseReports, orgId, gesCompanyId.FirstOrDefault());
            return caseReports;
        }

        public List<CaseReportListViewModel> GetCasesDataByCompanyId(long gesCompanyId, long companyId, long orgId, bool notShowCloseCase)
        {
            return GetReportListViewModel(orgId, gesCompanyId, companyId, notShowCloseCase);
        }

        private List<CaseReportListViewModel> GetReportListViewModel(long orgId, long gesCompanyId, long companyId, bool notShowCloseCase)
        {
            const long conventionsTypeId = 2;

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

                                  join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                                    cret.I_GesCaseReports_Id
                                  join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                                      et.I_EngagementTypes_Id into etg
                                  from et in etg.DefaultIfEmpty()
                                  join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                                  from etc in etcg.DefaultIfEmpty()

                                  join fm in _dbContext.G_ForumMessages on rp.G_ForumMessages_Id equals fm.G_ForumMessages_Id into fmg from fm in fmg.DefaultIfEmpty()
                                  join fmt in _dbContext.G_ForumMessages_Tree on fm.G_ForumMessages_Id equals fmt.G_ForumMessages_Id into fmtg from fmt in fmtg.DefaultIfEmpty()

                                  join grce in _dbContext.I_GesCaseReportsExtra on rp.I_GesCaseReports_Id equals grce.I_GesCaseReports_Id into grceg
                                  from grce in grceg.DefaultIfEmpty()

                                  join gp in (from gp in _dbContext.I_GesCaseReportsI_ProcessStatuses where gp.I_ProcessStatuses_Id == (long)ProcessStatus.Closed select gp) on rp.I_GesCaseReports_Id equals gp.I_GesCaseReports_Id into ggp
                                  from gp in ggp.DefaultIfEmpty()

                                  where rp.I_GesCompanies_Id == gesCompanyId && rp.ShowInClient && (notShowCloseCase == false || (rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.Resolved && rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.ResolvedIndicationOfViolation && rp.NewI_GesCaseReportStatuses_Id != (long)GesCaseReportStatus.ArchivedOfRecommendation))

                                  select new CaseReportListViewModel
                                  {
                                      Id = rp.I_GesCaseReports_Id,
                                      GesCompanyId = rp.I_GesCompanies_Id,
                                      IssueName = rp.ReportIncident,
                                      Confirmed = et.I_EngagementTypes_Id == conventionsTypeId ? rp.Confirmed : new bool?(),
                                      Location = l.Name,
                                      TemplateId = (rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.IndicationOfViolation || rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.Alert) ? (long)ReportingTemplates.StandardIndicationViolations : (long)ReportingTemplates.StandardConfirmedViolations,
                                      Conclusion = rx.Name,
                                      Recommendation = rd.Name,
                                      IsResolved = rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.Resolved || rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ResolvedIndicationOfViolation,
                                      IsArchived = rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ArchivedOfRecommendation,
                                      EngagementTypeId = et.I_EngagementTypes_Id,
                                      SortOrderEngagementType = et.SortOrder ?? 1,
                                      SortOrderRecommendation = rd.SortOrder ?? 1,
                                      Description = rp.Summary,
                                      EntryDate = rp.EntryDate ?? rp.Created,
                                      LastModified = rp.Modified,
                                      Norm = n.Name,
                                      NormId = rp.I_NormAreas_Id,
                                      EngagementType = et.Name,
                                      EngagementTypeCategory = etc.Name,
                                      Keywords = grce.Keywords,
                                      ProgressGrade = rp.I_ProgressStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ProgressStatuses_Id : 0,
                                      ResponseGrade = rp.I_ResponseStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ResponseStatuses_Id : 0,
                                      ClosingDate = gp.DateChanged,
                                      EngagementTypeCategoriesId = etc != null ? (long?)etc.I_EngagementTypeCategories_Id : null,
                                      ParentForumMessagesId = fm != null ? fm.ParentG_ForumMessages_Id ?? (long?)fm.G_ForumMessages_Id : null,
                                      ForumMessagesTreeId = fmt != null ? (long?)fmt.G_ForumMessages_Tree_Id : null
                                  }
                                  ).ToList();

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
                             r.IsResolved,
                             r.IsArchived,
                             r.EngagementTypeId,
                             r.SortOrderEngagementType,
                             r.SortOrderRecommendation,
                             r.Description,
                             r.EntryDate,
                             r.LastModified,
                             r.Norm,
                             r.NormId,
                             r.EngagementType,
                             r.EngagementTypeCategory,
                             r.Keywords,
                             r.ProgressGrade,
                             r.ResponseGrade,
                             r.ClosingDate,
                             r.IsInFocusList,
                             r.EngagementTypeCategoriesId,
                             r.ParentForumMessagesId,
                             r.ForumMessagesTreeId
                         }
                into g
                         select new CaseReportListViewModel
                         {
                             Id = g.Key.Id,
                             GesCompanyId = g.Key.GesCompanyId,
                             CompanyId = companyId,
                             IssueName = g.Key.IssueName,
                             Confirmed = g.Key.Confirmed,
                             Location = g.Key.Location,
                             TemplateId = g.Key.TemplateId,
                             Conclusion = g.Key.Conclusion,
                             Recommendation = g.Key.Recommendation?.Replace("Resolved (Indication of Violation)", "Resolved"),
                             IsResolved = g.Key.IsResolved,
                             IsArchived = g.Key.IsArchived,
                             EngagementTypeId = g.Key.EngagementTypeId,
                             SortOrderEngagementType = g.Key.SortOrderEngagementType,
                             SortOrderRecommendation = g.Key.SortOrderRecommendation,
                             Description = g.Key.Description,
                             EntryDate = g.Key.EntryDate,
                             LastModified = g.Key.LastModified,
                             ServiceEngagementThemeNorm = DataHelper.GetEngagementThemeNorm(g.Key.EngagementTypeCategory, g.Key.Norm, g.Key.EngagementType, g.Key.EngagementTypeId ?? 0),
                             NormId = g.Key.NormId,
                             Keywords = g.Key.Keywords,
                             ProgressGrade = g.Key.ProgressGrade,
                             ResponseGrade = g.Key.ResponseGrade,
                             ClosingDate = g.Key.ClosingDate,
                             //SignUp = g.Key.SignUp,
                             IsInFocusList = g.Key.IsInFocusList,
                             DevelopmentGrade = DataHelper.CalcDevelopmentGrade(g.Key.ProgressGrade ?? 0, g.Key.ResponseGrade ?? 0),
                             EngagementTypeCategoriesId = g.Key.EngagementTypeCategoriesId,
                             ParentForumMessagesId = g.Key.ParentForumMessagesId,
                             ForumMessagesTreeId = g.Key.ForumMessagesTreeId
                         };

            var caseReports = result.ToList();
            _gesCaseReportsRepository.UpdateCaseReportSubscribed(caseReports, orgId, gesCompanyId);
            return caseReports;
        }

        public IEnumerable<long> GetIndustryGroupIdsFromSectorIds(string sectorIds)
        {
            //TODO:MENDO
            //var ids = ConvertStringToListIds(sectorIds);
            //if (!ids.Any()) return new List<int>();

            //var result =
            //    from m in _dbContext.SubPeerGroups
            //    where ids.Contains((long)m.Id)
            //    select m.Id;
            //return result;
            return new List<long>();
        }
        public IEnumerable<string> GetCountryCodesFromContinentIds(string continentIds)
        {
            var result =
                from m in _dbContext.Countries
                where continentIds.Contains(m.RegionId.ToString())
                select m.Alpha3Code;
            return result;
        }

        private IList<long> ConvertStringToListIds(string idStr)
        {
            var ids = idStr.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(long.Parse);
            return ids as long[] ?? ids.ToArray();
        }
        
        public IEnumerable<IdNameModel> GetRecommendations()
        {
            var listId = new List<long> { 6, 7, 8, 3, 9 };

            return from gc in _dbContext.I_GesCaseReportStatuses
                    where listId.Contains(gc.I_GesCaseReportStatuses_Id)
                    orderby gc.SortOrder descending
                    select new IdNameModel { Id = gc.I_GesCaseReportStatuses_Id, Name = gc.Name };
        }

        public IEnumerable<IdNameModel> GetConclusions()
        {
            var result = (from gc in _dbContext.I_GesCaseReportStatuses
                          join cr in _dbContext.I_GesCaseReports on gc.I_GesCaseReportStatuses_Id equals cr.I_GesCaseReportStatuses_Id
                          where cr.ShowInClient
                          orderby gc.SortOrder descending
                          select new { Id = gc.I_GesCaseReportStatuses_Id, gc.Name, gc.SortOrder }).Distinct();

            var rs = result.OrderByDescending(d => d.SortOrder).Select(d => new IdNameModel { Id = d.Id, Name = d.Name });
            return rs;
        }
        
        public IEnumerable<IdNameModel> GetProgresses()
        {
            var result = from p in _dbContext.I_ProgressStatuses
                         orderby p.I_ProgressStatuses_Id descending
                         select new IdNameModel { Id = p.I_ProgressStatuses_Id, Name = p.ShortName };

            return result;
        }

        public IEnumerable<IdNameModel> GetResponses()
        {
            var result = from r in _dbContext.I_ResponseStatuses
                         orderby r.I_ResponseStatuses_Id descending
                         select new IdNameModel { Id = r.I_ResponseStatuses_Id, Name = r.ShortName };

            return result;
        }

        public IEnumerable<Countries> GetAllCountries()
        {
            var countries = AllCountries().OrderBy(c => c.Name).ToList();
            return countries;
        }

        public IEnumerable<Countries> AllCountries()
        {
            var result = _dbContext.Countries.FromCache();

            return result;
        }


        public IEnumerable<I_ManagementSystems> AllManagementSystems()
        {
            var result = _dbContext.I_ManagementSystems.FromCache();

            return result;
        }

        public IEnumerable<ManagementSystemModel> GetAllManagementSystems()
        {
            var managementSystems = AllManagementSystems().Select(c => new ManagementSystemModel
            {
                I_ManagementSystems_Id = c.I_ManagementSystems_Id,
                Name = c.Name,
                Category = c.Category
            });
            return managementSystems;
        }

        public IEnumerable<SubPeerGroupModel> GetAllSubPeerGroups()
        {
            var managementSystems = _dbContext.SubPeerGroups.Select(c => new SubPeerGroupModel
            {
                Id = c.Id,
                Name = c.Name,
                PeerGroupId = c.PeerGroupId
            });
            return managementSystems;
        }

        public IEnumerable<I_Companies> GetAllMasterCompanies()
        {
            return _dbContext.I_Companies.Where(d => d.MasterI_Companies_Id == null);
        }

        public IEnumerable<IdNameModel> GetPortfolioIndexes(long orgId)
        {
            var result = from po in _dbContext.I_PortfoliosG_Organizations
                         join p in _dbContext.I_Portfolios on po.I_Portfolios_Id equals p.I_Portfolios_Id
                         where po.G_Organizations_Id == orgId
                         && (from x in _dbContext.I_PortfoliosG_Organizations
                             join y in _dbContext.I_PortfoliosG_OrganizationsG_Services on x.I_PortfoliosG_Organizations_Id equals y.I_PortfoliosG_Organizations_Id
                             join z in _dbContext.G_OrganizationsG_Services on y.G_Services_Id equals z.G_Services_Id
                             join t in _dbContext.G_Services on z.G_Services_Id equals t.G_Services_Id
                             where x.I_PortfoliosG_Organizations_Id == po.I_PortfoliosG_Organizations_Id && (t.G_Services_Id == (long)GesService.GesGlobalEthicalStandard || t.I_EngagementTypes_Id != null)
                             select t.G_Services_Id
                             ).Any()
                         select new IdNameModel { Id = po.I_PortfoliosG_Organizations_Id, Name = p.Name };

            return result.Distinct().OrderBy(d => d.Name);
        }

        public IEnumerable<IdNameModel> GetAllIndustries()
        {
            //const int validMsciCodeLength = 4;

            var result = from i in _dbContext.SubPeerGroups
                         //where i.Code.Length == validMsciCodeLength
                         select new IdNameModel { Id = i.Id, Name = i.Name };

            return result;
        }

        public IEnumerable<SelectListItem> GetEngagmentThemeNorm(long orgId)
        {
            var result = new List<SelectListItem>();

            var engagementTypes = GetEngagementTypes();

            var businessConducts = engagementTypes
                .Where(d => d.I_EngagementTypeCategories_Id == (long) EngagementTypeCategoryEnum.BusinessConduct)
                .ToList();

            SetBussinessConduct(businessConducts, orgId, result);

            var governance = engagementTypes
                .Where(d => d.I_EngagementTypeCategories_Id == (long)EngagementTypeCategoryEnum.Governance)
                .Select(d => d.I_EngagementTypes_Id).ToList();

            GetGovernanceType(governance, result, orgId);

            var stewardShipAndRisk = engagementTypes
                .Where(d => d.I_EngagementTypeCategories_Id == (long)EngagementTypeCategoryEnum.StewardshipAndRisk)
                .Select(d=>d.I_EngagementTypes_Id).ToList();

            AddSnRItems(stewardShipAndRisk,result, orgId);

            var otherTypes = engagementTypes
                .Where(d => d.I_EngagementTypeCategories_Id != (long)EngagementTypeCategoryEnum.StewardshipAndRisk && d.I_EngagementTypeCategories_Id != (long)EngagementTypeCategoryEnum.BusinessConduct && d.I_EngagementTypeCategories_Id != (long)EngagementTypeCategoryEnum.Governance)
                .Select(d => d.I_EngagementTypes_Id).ToList();
            AddOtherGroupThemeNorm(otherTypes, result, orgId);
            return result;
        }

        private void SetBussinessConduct(List<I_EngagementTypes> businessConducts, long orgId, List<SelectListItem> result)
        {
            foreach (var engagementTypese in businessConducts)
            {
                if (engagementTypese.I_EngagementTypes_Id == (long)EngagementTypeEnum.Conventions)
                {
                    GetUsingNormGroupListItem(ServiceSelectListItemConfiguration.ServiceIds_UsingNormGroup1,
                        ServiceSelectListItemConfiguration.NormIds_Group1,
                        result, orgId);
                }
                else if (engagementTypese.I_EngagementTypes_Id == (long)EngagementTypeEnum.BusinessConductExtendedTaxation)
                {
                    GetUsingNormGroupListItem(ServiceSelectListItemConfiguration.ServiceIds_UsingNormGroup2,
                        ServiceSelectListItemConfiguration.NormIds_Group2,
                        result, orgId, false);
                }
                else
                {
                    SetBussinessConductSingleItems(engagementTypese.I_EngagementTypes_Id, result, orgId);
                }
            }

            if (IsGlobalEthicalStandard(orgId))
            {
                GetUsingNormGroupListItem(
                    ServiceSelectListItemConfiguration.ServiceIds_GlobalEthicalStandard,
                    ServiceSelectListItemConfiguration.NormIds_Group1,
                    result, orgId);
            }
        }

        private List<I_EngagementTypes> GetEngagementTypes()
        {
            var query = from t in _dbContext.I_EngagementTypes
                where t.IsShowInClientMenu == true
                select t;

            return query.ToList();
        }

        private void GetGovernanceType(List<long> governanceIds, List<SelectListItem> result, long orgId)
        {
            var governanceType = new SelectListItem();
            var query = (from s in _dbContext.G_Services
                join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                where os.G_Organizations_Id == orgId && governanceIds.Contains(et.I_EngagementTypes_Id)
                select new
                {
                    et.Name,
                    s.G_Services_Id
                }
            ).OrderBy(d => d.Name).ToList();

            var governaneItems = (from x in query
                select new SelectListItem
                {
                    Text = string.Format(Resources.CorporateGovernance, DataHelper.FormatServiceName(x.Name)),
                    Value = $"{x.G_Services_Id}--"
                }).ToList();

            if (query.Count > 1)
            {
                governanceType.Text = Resources.GovernanceAll;
                governanceType.Value = string.Join(",", query.Select(d => d.G_Services_Id)) + "--";
                result.Add(governanceType);
            }

            result.AddRange(governaneItems);
        }

        private void AddSnRItems(List<long> snRTypeId,List<SelectListItem> result, long orgId)
        {
            var allSnR = new SelectListItem();            
            var query = (from s in _dbContext.G_Services
                         join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                         join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                         where os.G_Organizations_Id == orgId && snRTypeId.Contains(et.I_EngagementTypes_Id)
                         select new
                         {
                             et.Name,
                             s.G_Services_Id
                         }
                ).OrderBy(d => d.Name).ToList();

            var snrItems = (from x in query
                            select new SelectListItem
                            {
                                Text = string.Format(Resources.StewardshipAndRiskEngagementType, DataHelper.FormatServiceName(x.Name)),
                                Value = $"{x.G_Services_Id}--"
                            }).ToList();

            if (query.Count > 1)
            {
                allSnR.Text = Resources.StewardshipAndRiskEngagementTypeAll;
                allSnR.Value = string.Join(",", query.Select(d => d.G_Services_Id)) + "--";
                result.Add(allSnR);
            }

            result.AddRange(snrItems);
        }

        private void AddOtherGroupThemeNorm(List<long> engagementTypeId, List<SelectListItem> result, long orgId)
        {
            var query = (from s in _dbContext.G_Services
                join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id
                join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                where os.G_Organizations_Id == orgId && engagementTypeId.Contains(et.I_EngagementTypes_Id)
                select new
                {
                    CatetoryId = etc.I_EngagementTypeCategories_Id,
                    CategoryName = etc.Name,
                    et.Name,
                    s.G_Services_Id
                }
            ).OrderBy(d => d.CatetoryId).ThenBy(e=>e.Name).ToList();

            long categoryId = -1;


            foreach (var item in query)
            {
                if (categoryId != item.CatetoryId)
                {
                    categoryId = item.CatetoryId;
                    result.Add(new SelectListItem()
                    {
                        Text = $"{DataHelper.FormatServiceName(item.CategoryName)} - All",
                        Value = $"{string.Join(",", query.Where(d=>d.CatetoryId == categoryId).Select(d=>d.G_Services_Id).ToList())}--"
                    });
                }

                result.Add(new SelectListItem()
                {
                    Text = $"{DataHelper.FormatServiceName(item.CategoryName)} - {DataHelper.FormatServiceName(item.Name)}",
                    Value = $"{item.G_Services_Id}--"
                });
            }
        }

        private void SetBussinessConductSingleItems(long engagementTypeId ,List<SelectListItem> result, long orgId)
        {
            var service =  (from s in _dbContext.G_Services
                                    join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                    join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                                    where os.G_Organizations_Id == orgId && et.I_EngagementTypes_Id == engagementTypeId
                                    select 
                                    s
                                ).FirstOrDefault();

            if (service != null)
            {
                result.Add(new SelectListItem
                {
                    Text = $@"Global Standards - {service.Name}",
                    Value = $"{service.G_Services_Id}--"
                });
            }
        }

        private void GetUsingNormGroupListItem(long[] serviceIds, long[] normGroupIds, List<SelectListItem> result, long orgId, bool addSearchAll = true)
        {
            foreach (var serviceId in serviceIds)
            {
                var service = (from s in _dbContext.G_Services
                               join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                               where os.G_Organizations_Id == orgId && s.G_Services_Id == serviceId
                               select s).FirstOrDefault();

                if (service == null) continue;

                if (addSearchAll)
                {
                    result.Add(new SelectListItem
                    {
                        Text = DataHelper.FormatServiceName(service.Name) + " - All",
                        Value = $"{serviceId}--"
                    });
                }

                var selectItems = _dbContext.I_NormAreas
                                             .Where(i => normGroupIds.Contains(i.I_NormAreas_Id))
                                             .OrderBy(i => i.Name)
                                             .ToList()
                                             .Select(i => new SelectListItem
                                             {
                                                 Text = DataHelper.FormatServiceName(service.Name) + " - " + i.Name,
                                                 Value = $"{serviceId}--{i.I_NormAreas_Id}"
                                             });
                result.AddRange(selectItems);
            }
        }
        
        public CompanyDetailViewModel GetCompanyDetailViewModel(long companyId, long orgId, long individualId, bool isExportPdf = false)
        {
            var result = from c in _dbContext.I_Companies
                         join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id into gcg
                         from gc in gcg.DefaultIfEmpty()
                         join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into hcg
                         from hc in hcg.DefaultIfEmpty()
                         join m in _dbContext.SubPeerGroups on c.SubPeerGroupId equals m.Id into mg
                         from m in mg.DefaultIfEmpty()
                         join pgs in _dbContext.PeerGroups on m.PeerGroupId equals pgs.Id into pgm
                         from pgs in pgm.DefaultIfEmpty()
                         join cw in _dbContext.I_GesCompanyWatcher on new { GesCompanyId = gc.I_GesCompanies_Id, IndividualId = individualId } equals new { GesCompanyId = cw.I_GesCompanies_Id, IndividualId = cw.G_Individuals_Id } into cwg
                         from cw in cwg.DefaultIfEmpty()
                         join p in _dbContext.GesCompanyProfiles on c.Isin equals p.Isin into pg from p in pg.DefaultIfEmpty()
                         from mc in _dbContext.I_Companies.Where(mc => mc.I_Companies_Id == c.MasterI_Companies_Id).DefaultIfEmpty()
                         where c.I_Companies_Id == companyId
                         select new CompanyDetailViewModel
                         {             
                             Industry = pgs.Name,
                             CompanyId = c.I_Companies_Id,
                             GesCompanyId = gc.I_GesCompanies_Id,
                             CompanyName = c.Name,
                             Isin = c.Isin,
                             Sedol = c.Sedol,
                             CountryId = c.CountryOfIncorporationId,
                             CountryCode = hc.Alpha3Code.ToLower(),
                             Country = hc.Name,
                             MsciIndustry = m.Name,
                             Website = c.Website,
                             Overview = c.Description,
                             KeyEsgIssues = m.Name ,//msci.MostMaterialRisk,
                             Msci_Id = c.I_Msci_Id,
                             IsInFocusList = cw != null,
                             Distribution = p.Distribution,
                             KeyIssues = p.KeyIssues,
                             Grade = p.Grade,
                             PrimeThreshold = p.PrimeThreshold,
                             IsPrime = p.IsPrime,
                             BbgID = c.BbgID,
                             UnGlobalCompact = c.UnGlobalCompact,
                             GriAlignedDisclosure = c.GriAlignedDisclosure,
                             OtherName1 = c.FtseName,
                             OtherName2 = c.SixName,
                             OtherName3 = c.OtherName,
                             OldName = c.OldName,
                             MediaName = c.MediaName,
                             MasterCompanyId = c.MasterI_Companies_Id,
                             MasterCompanyName = mc.Name,
                             //MostMaterialRisk = c.MostMaterialRisk,
                             TransparencyDisclosure = c.TransparencyDisclosure,
                             Ftse_Id = c.I_Ftse_Id,
                             //FTSE = c.I_FtseGroups_Id,
                             //MSCI = c.I_Msci_Id,
                             InformationSource = c.InformationSource,
                             ListSource = c.ListSource,
                             SecurityDescription = c.SecurityDescription,
                             SubPeerGroupId = c.SubPeerGroupId,
                             SubPeerGroup = pgs.Name,
                             IsParkedForGssResearchSince = c.IsParkedForGssResearchSince
                             //CountryRegId = c.CountryRegG_Countries_Id
                         };

            var companyDetail = result?.FirstOrDefault();
            
            
            if (companyDetail != null)
            {
                //TODO:MENDO
                //if (companyDetail.Msci_Id != null)
                //{
                //    companyDetail.MSCI = (from m in _dbContext.SubPeerGroups
                //                          where m.Id == companyDetail.SubPeerGroupId
                //                          select new MsciFtseViewModel()
                //                          {
                //                              Id = m.I_Msci_Id,
                //                              Name = m.Name,
                //                              parentId = m.ParentI_Msci_Id
                //                          }).FirstOrDefault();
                //}

                //if (companyDetail.Ftse_Id != null)
                //{
                //    companyDetail.FTSE = (from m in _dbContext.SubPeerGroups
                //                          where m.I_Ftse_Id == companyDetail.Ftse_Id
                //                          select new MsciFtseViewModel()
                //                          {
                //                              Id = m.I_Ftse_Id,
                //                              Name = m.Name,
                //                              parentId = m.ParentI_Ftse_Id
                //                          }).FirstOrDefault();
                //}
                companyDetail.Dialogues = _companiesRepository.CountCompanyDialogueByCompany(orgId, companyId, EngagementStatisticType.Dialogues);
                companyDetail.Contacts = _companiesRepository.CountCompanyDialogueByCompany(orgId, companyId, EngagementStatisticType.Contacts);
                companyDetail.Meetings = _companiesRepository.CountCompanyDialogueByCompany(orgId, companyId, EngagementStatisticType.Meetings);
                companyDetail.ConferenceCalls = _companiesRepository.CountCompanyDialogueByCompany(orgId, companyId, EngagementStatisticType.ConferenceCalls);
                companyDetail.Correspondence = _companiesRepository.CountCompanyDialogueByCompany(orgId, companyId, EngagementStatisticType.Emails);
                companyDetail.CompanyManagementSystems = _companiesRepository.GetCompanyManagementSystems(companyId);
                companyDetail.CompanyPortfolios = _companiesRepository.GetCompanyPortfolioModels(companyId);
                var companyFollowingPortfolios = _companiesRepository.GetCompanyFollowingPortfolioModels(companyId);
                if (companyFollowingPortfolios != null && companyFollowingPortfolios.Any())
                {
                    companyDetail.CompanyFollowingStandardPortfolios = companyFollowingPortfolios.Where(x=>x.PortfolioTypeId == 2).OrderByDescending(x => x.Created).ToList();
                    companyDetail.CompanyFollowingOtherPortfolios = companyFollowingPortfolios.Where(x => x.PortfolioTypeId == 3).OrderByDescending(x => x.Created).ToList();
                    companyDetail.CompanyFollowingCustomerPortfolios = companyFollowingPortfolios.Where(x => x.PortfolioTypeId == 1).OrderByDescending(x => x.Created).ToList();
                }

                //TODO:MENDO
                //if (companyDetail.Msci_Id != null)
                //{
                //    var listAllMsci = _dbContext.I_Msci.FromCache().ToList();

                //    var msciItem = CommonProcess.GetMsciSector(listAllMsci, companyDetail.Msci_Id.Value);
                //    companyDetail.GicsSector = msciItem.Sector;
                //}
                
                companyDetail.Documents = GetDocuments(companyId)?.ToList();

            }
            return companyDetail;
        }

        public List<AutoCompleteModel> GetAutoCompleteCompanyIssueName(long orgId, string inputText, int limit, bool searchTags, bool searchCases, bool searchCompanies)
        {
            var maximumItems = limit / 3;
            var result = new List<AutoCompleteModel>();
            if (searchCompanies)
            {
                result = GetSuggestedCompanies(orgId, inputText, maximumItems);
            }
            
            if (searchCases)
            {
                var caseProfiles = GetSuggestedCaseProfiles(orgId);
                var casesNames = GetSuggestedCaseProfiles(orgId, inputText, maximumItems);
                result.AddRange(casesNames);
                
                if (searchTags)
                {
                    result.AddRange(GetSuggestedTags(inputText, caseProfiles, maximumItems));
                }
            }

            return result;
        }

        private static List<AutoCompleteModel> GetSuggestedTags(string keyWord, IList<AutoCompleteModel> caseProfiles, int partOfLimit)
        {
            var keywordStr = caseProfiles.Where(d => !d.Id.Contains(keyWord, StringComparison.OrdinalIgnoreCase) && (d.Name != null && d.Name.Contains(keyWord, StringComparison.OrdinalIgnoreCase)))
                    .Select(d => d.Name);

            var keywordsAndCounts = string.Join(", ", keywordStr).Split(',')
                .Where(i => !string.IsNullOrEmpty(i) && i.Contains(keyWord, StringComparison.OrdinalIgnoreCase))
                .GroupBy(x => x).OrderByDescending(g => g.Count());

            var initTags = keywordsAndCounts.Select(g => new AutoCompleteModel
            {
                Id = g.Key,
                Name = "Tags",
                SortOrder = GetSortOrderByName(keyWord, g.Key),
                SortCategory = g.Count()
            }).ToList();

            return initTags.Distinct().OrderBy(d => d.SortOrder)
                    .ThenByDescending(d => d.SortCategory)
                    .ThenBy(d => d.Id)
                    .Skip(0)
                    .Take(partOfLimit)
                    .ToList();
            
        }

        private IList<AutoCompleteModel> GetSuggestedCaseProfiles(long orgId)
        {
            var queryCases = (from pog in _dbContext.I_PortfoliosG_Organizations
                join po in _dbContext.I_PortfoliosI_Companies on pog.I_Portfolios_Id equals po.I_Portfolios_Id
                join c in _dbContext.I_Companies on po.I_Companies_Id equals c.I_Companies_Id
                join mc in _dbContext.I_Companies on c.MasterI_Companies_Id != null
                    ? c.MasterI_Companies_Id.Value
                    : c.I_Companies_Id equals mc.I_Companies_Id
                join gc in _dbContext.I_GesCompanies on mc.I_Companies_Id equals gc.I_Companies_Id
                join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on pog.I_PortfoliosG_Organizations_Id
                equals pos.I_PortfoliosG_Organizations_Id
                join gs in _dbContext.G_OrganizationsG_Services on pos.G_Services_Id equals gs.G_Services_Id
                join s in _dbContext.G_Services on gs.G_Services_Id equals s.G_Services_Id
                join cr in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals cr.I_GesCompanies_Id
                join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on new
                {
                    ReportId = cr.I_GesCaseReports_Id,
                    EngageMentypeId = s.I_EngagementTypes_Id
                } equals new {ReportId = cret.I_GesCaseReports_Id, EngageMentypeId = (long?) cret.I_EngagementTypes_Id}
                where gs.G_Organizations_Id == orgId && pog.G_Organizations_Id == orgId && cr.ShowInClient &&
                      cr.ReportIncident != null && cr.ReportIncident != "Temporary Dialogue Case" && c.MasterI_Companies_Id == null
                              group cr by new
                {
                    ReportIncident = cr.ReportIncident.Trim() + " (" + mc.Name + ")",
                    CompanyName = mc.Name,
                    ReportId = cr.I_GesCaseReports_Id,
                    Marketcap = c.MarketCap,
                    CompanyId = c.I_Companies_Id
                }
                into g
                select new
                {
                    g.Key.ReportIncident,
                    g.Key.CompanyName,
                    g.Key.ReportId,
                    g.Key.Marketcap,
                    g.Key.CompanyId
                }).FromCache().ToList();


            var caseReportExtra = (from e in _dbContext.I_GesCaseReportsExtra
                select new
                {
                    e.I_GesCaseReports_Id,
                    e.Keywords
                }).FromCache().ToList();

            return (from c in queryCases
                join ex in caseReportExtra on c.ReportId equals ex.I_GesCaseReports_Id into exg
                from ex in exg.DefaultIfEmpty()
                group c by new
                {
                    c.ReportIncident,
                    c.CompanyName,
                    ex?.Keywords,
                    c.Marketcap,
                    c.CompanyId
                }
                into g
                select new AutoCompleteModel
                { 
                   Id = g.Key.ReportIncident,
                   CompanyName = g.Key.CompanyName,
                   Name = g.Key.Keywords,
                   MarketCap = g.Key.Marketcap,
                   CompanyId = g.Key.CompanyId.ToString()
                }).ToList();
        }

        private List<AutoCompleteModel> GetSuggestedCompanies(long orgId, string inputText, int partOfLimit)
        {
            var qrCompany = (from pog in _dbContext.I_PortfoliosG_Organizations
                join po in _dbContext.I_PortfoliosI_Companies on pog.I_Portfolios_Id equals po.I_Portfolios_Id
                join c in _dbContext.I_Companies on po.I_Companies_Id equals c.I_Companies_Id
                join mc in _dbContext.I_Companies on c.MasterI_Companies_Id != null
                    ? c.MasterI_Companies_Id.Value
                    : c.I_Companies_Id equals mc.I_Companies_Id
                where pog.G_Organizations_Id == orgId &&
                      //(mc.MasterI_Companies_Id == null || mc.MasterI_Companies_Id == mc.I_Companies_Id) &&
                      mc.Id >= 1000000000 &&
                      mc.Name != null
                      && (from x in _dbContext.I_PortfoliosG_OrganizationsG_Services
                          join os in _dbContext.G_OrganizationsG_Services on x.G_Services_Id equals os.G_Services_Id
                          join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                          where x.I_PortfoliosG_Organizations_Id == pog.I_PortfoliosG_Organizations_Id
                                && (s.I_EngagementTypes_Id != null || s.G_Services_Id == (long)GesService.GesGlobalEthicalStandard)
                                && os.G_Organizations_Id == 3485
                          select x.I_PortfoliosG_OrganizationsG_Services_Id).Any()
                group mc by new
                {
                    Name = mc.Name.Trim(),
                    IsMasterCompany = (mc.MasterI_Companies_Id == null || mc.MasterI_Companies_Id == mc.I_Companies_Id),
                    mc.MarketCap
                }
                into g
                select g.Key
            ).FromCache();

            var companiesNames =
                qrCompany.Where(d => d.Name.Contains(inputText, StringComparison.OrdinalIgnoreCase))
                    .Select(
                        d =>
                            new AutoCompleteModel
                            {
                                Id = d.Name,
                                Name = "Companies",
                                SortCategory = d.IsMasterCompany ? 1 : 2,
                                MarketCap = d.MarketCap,
                                SortOrder = GetSortOrderByName(inputText, d.Name)
                            })
                    .ToList();

            return companiesNames.OrderBy(d => d.SortCategory)
                .ThenBy(d => d.SortOrder)
                .ThenByDescending(d => d.MarketCap)
                .ThenBy(d => d.Id)
                .Skip(0)
                .Take(partOfLimit)
                .ToList();
        }


        private List<AutoCompleteModel> GetSuggestedCaseProfiles(long orgId, string inputText, int partOfLimit)
        {
            var queryCases = GetSuggestedCaseProfiles(orgId);

            // Group cases
            var casesNames =
                queryCases.Where(d => d.Id.Contains(inputText, StringComparison.OrdinalIgnoreCase))
                    .Select(d => new AutoCompleteModel
                    {
                        Id = d.Id,
                        Name = "Cases",
                        SortOrder = GetSortOrderByName(inputText, d.Id),
                        MarketCap = d.MarketCap,
                        CompanyId = d.CompanyId
                    }).ToList();

            return casesNames.Distinct().OrderBy(d => d.SortOrder).ThenByDescending(d => d.MarketCap).Skip(0).Take(partOfLimit).ToList();
        }

        public List<CompanyListViewModel> ExportCompanies(long orgId,
            long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notshowclosecase,
            bool onlycompanieswithactivecase, bool isNew, string baseUrl, List<long> companyIds,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId,
            bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId)
        {
            var linkHelper = new LinkHelper(isNew, baseUrl);

            var result = _companiesRepository.GetFilteredCompaniesFromCache(orgId, individualId, onlyShowFocusList, name, isin, portfolioIds, notshowclosecase, ProcessRecommendation(recommendationId), conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, homeCountryIds, sustainalyticsId);

            if (companyIds.Count > 0)
            {
                result = result.Where(d => companyIds.Contains(d.Id)).ToList();
            }

            foreach (var item in result)
            {
                item.CompanyLink = linkHelper.GenCompanyLink(item.CompanyId);
            }

            result = result.Where(d => onlycompanieswithactivecase == false || d.NumCasesActive > 0).OrderBy(d => d.CompanyName).ToList();

            return result;
        }
        
        #endregion

        public List<long> GetListGescompanyIdsFromFilter(long orgId, long individualId, bool onlyShowFocusList,
            string name, string isin, List<long> portfolioIds, bool notshowclosecase, string baseUrl,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId,
            List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds,
            long? engagementTypeId,
            bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId)
        {
            var result = _companiesRepository.GetFilteredCompaniesFromCache(orgId, individualId, onlyShowFocusList, name, isin, portfolioIds, notshowclosecase, ProcessRecommendation(recommendationId), conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, homeCountryIds, sustainalyticsId);

            return result.Select(d => d.Id).ToList();
        }
        
        public List<long> GetCaseReportsForFocusList(List<long> gesCompanyIds, bool notshowclosecase, long orgId, long individualId, bool onlyShowFocusList, bool companyInfocusList, string name, List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId)
        {
            var caseReports = GetReportListViewModelForSubGrid(gesCompanyIds, 0, notshowclosecase, orgId, individualId, null, recommendationId, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId);
            return caseReports.Select(d => d.Id).ToList();
        }

        public long? GetGesCompanyId(long companyId)
        {
            return SafeExecute(() => _companiesRepository.FindBy(d => d.I_Companies_Id == companyId).FirstOrDefault()?.I_GesCompanies.FirstOrDefault()?.I_GesCompanies_Id);
        }

        public SearchCompanyResult CheckCompanyInPortfolios(long companyId, long orgId)
        {
            var querry = from po in _dbContext.I_PortfoliosG_Organizations
                join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                where po.G_Organizations_Id == orgId && (pc.I_Companies_Id == companyId || c.MasterI_Companies_Id == companyId)
                select po.I_Portfolios_Id;

            if (querry.Any()) return SearchCompanyResult.Matched;

            var querry2 = from po in _dbContext.I_PortfoliosG_Organizations
                join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                where po.G_Organizations_Id == 3485 && (pc.I_Companies_Id == companyId || c.MasterI_Companies_Id == companyId)
                select po.I_Portfolios_Id;

            if (querry2.Any()) return SearchCompanyResult.OutsidePortfolio;

            return SearchCompanyResult.NotFound;
        }

        public PaginatedResults<CompanyViewModel> GetGesCompanies(JqGridViewModel jqGridParams)
        {
            const int maximumRowsCanGetRelatedRows = 2000;
            var companies = _companiesRepository.GetGesCompaniesWithLocation();
            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CompanyViewModel>(jqGridParams);
                var filteredCompanies = string.IsNullOrEmpty(finalRules) ? companies : companies.Where(finalRules);

                if (filteredCompanies.Count() < maximumRowsCanGetRelatedRows)
                {
                    var companyIdsWithParentIds = SafeExecute(() => filteredCompanies.Select(d => new {Id = d.Id, parentId = d.parent}).ToList());
                    var companyIds = companyIdsWithParentIds.Select(d => d.Id).ToList();
                    var parentCompanyIds = companyIdsWithParentIds.Where(d => d.parentId != null).Select(d => d.parentId).Distinct().ToList();
                    var masterCompantIds = companyIdsWithParentIds.Where(d => d.parentId == null).Select(d => d.Id).ToList();

                    companies = companies.Where(c => parentCompanyIds.Contains(c.Id) || companyIds.Contains(c.Id) || (c.parent != null && masterCompantIds.Contains(c.parent.Value)));
                }
                else
                {
                    companies = filteredCompanies;
                }
            }

            return companies.OrderBy(x => x.sortPath).ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public IEnumerable<I_Companies> GetCompaniesBySustainalyticsIDList(IEnumerable<long> ids)
        {
            const int maximumBatchSize = 5000;
            var next = 0;
            var idList = ids.ToList();
            var numberOfId = idList.Count;

            if (numberOfId == 0)
                return new List<I_Companies>();

            var companiesById = new List<I_Companies>();

            SafeExecute(() =>
            {
                while (true)
                {
                    var batchSize = (next + maximumBatchSize) <= numberOfId ? maximumBatchSize : numberOfId - next;
                    if (batchSize <= 0)
                        break;

                    var idRange = idList.GetRange(next, batchSize);
                    var companies = _companiesRepository.FindBy(d => idRange.Contains(d.Id));
                    companiesById.AddRange(companies);
                    next += maximumBatchSize;
                }
            });

            return companiesById;
        }

        private IEnumerable<DocumentViewModel> GetDocuments(long companyId)
        {
            return _documentService.GetDocumentsByCompanyId(companyId);
        }
    }
}
