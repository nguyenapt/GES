using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_CompaniesService : IEntityService<I_Companies>
    {
        IEnumerable<I_Companies> GetCompaniesByListIsin(IEnumerable<string> isinCodes);

        List<IdNameModel> GetCompaniesWithTerm(string term, int limit);

        IList<KeyValueObject<string, string>> GetCompaniesAndSubCompaniesWithTerm(string term, int limit);

        IEnumerable<I_Companies> GetCompaniesByListIds(IEnumerable<long> companyIds);

        PaginatedResults<CompanyViewModel> GetCompanies(JqGridViewModel jqGridParams);
        PaginatedResults<CompanyViewModel> GetMasterCompanies(JqGridViewModel jqGridParams);

        string GetMaximumCustomIsinCode();

        IEnumerable<I_Companies> GetCompaniesWithErrorIsinCode();

        IEnumerable<I_Companies> GetCompaniesWithWhiteSpaceIsinCode();

        IEnumerable<long> GetIndustryGroupIdsFromSectorIds(string sectorIds);

        IEnumerable<string> GetCountryCodesFromContinentIds(string continentIds);
        
        IEnumerable<IdNameModel> GetRecommendations();

        IEnumerable<IdNameModel> GetConclusions();
        
        IEnumerable<IdNameModel> GetProgresses();

        IEnumerable<IdNameModel> GetResponses();

        IEnumerable<Countries> GetAllCountries();        

        IEnumerable<Countries> AllCountries();

        IEnumerable<I_ManagementSystems> AllManagementSystems();

        IEnumerable<ManagementSystemModel> GetAllManagementSystems();
        IEnumerable<SubPeerGroupModel> GetAllSubPeerGroups();

        IEnumerable<I_Companies> GetAllMasterCompanies();

        IEnumerable<IdNameModel> GetPortfolioIndexes(long orgId);
        
        CompanyDetailViewModel GetCompanyDetailViewModel(long companyId, long orgId, long individualId, bool isExportPdf = false);

        List<AutoCompleteModel> GetAutoCompleteCompanyIssueName(long orgId, string inputText, int limit, bool searchTags, bool searchCases, bool searchCompanies);
        
        PaginatedResults<CompanyListViewModel> GetCompanyCasesGrid(JqGridViewModel jqGridParams, long orgId, long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notShowCloseCase, bool onlyCompaniesWithActiveCase,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId, List<Guid?> homeCountryIds, long? companyId, long? sustainalyticsId);

        List<CaseReportListViewModel> GetCaseReportsData(long gesCompanyId, long companyId, bool notShowCloseCase, long orgId, long individualId, bool onlyShowFocusList, bool companyInfocusList, string keyword, List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryId, long? engagementTypeId);

        List<CompanyListViewModel> ExportCompanies(long orgId, long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notshowclosecase, bool onlycompanieswithactivecase, bool isNew, string baseUrl, List<long> companyIds,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId, bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId);
        
        IEnumerable<SelectListItem> GetEngagmentThemeNorm(long orgId);
        IEnumerable<IdNameModel> GetAllIndustries();
        
        #region Nathalia's request
        List<long> GetListGescompanyIdsFromFilter(long orgId, long individualId, bool onlyShowFocusList,
            string name, string isin, List<long> portfolioIds, bool notshowclosecase, string baseUrl,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId,
            List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds,
            long? engagementTypeId,
            bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId);

        List<long> GetCaseReportsForFocusList(List<long> gesCompanyIds, bool notshowclosecase, long orgId,
            long individualId, bool onlyShowFocusList, bool companyInfocusList, string name,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId,
            List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds,
            long? engagementTypeId);

        List<CaseReportListViewModel> GetCasesDataByCompanyId(long gesCompanyId, long companyId, long orgId, bool notShowCloseCase);

        #endregion

        long? GetGesCompanyId(long companyId);

        ClientType GetClientType(long orgId);

        SearchCompanyResult CheckCompanyInPortfolios(long companyId, long orgId);
        PaginatedResults<CompanyViewModel>  GetGesCompanies(JqGridViewModel jqGridParams);
        
        IEnumerable<I_Companies> GetCompaniesBySustainalyticsIDList(IEnumerable<long> ids);
    }
}
