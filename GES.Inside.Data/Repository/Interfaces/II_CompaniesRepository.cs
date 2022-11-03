using System;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Enumeration;
using GES.Inside.Data.Models.StoredProcedureParams;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_CompaniesRepository : IGenericRepository<I_Companies>
    {
        IEnumerable<I_Companies> GetCompaniesByIsins(IEnumerable<string> isins);
        IEnumerable<CompanyBriefInfo> GetCompaniesByTerm(string term, int limit);
        IQueryable<CompanyViewModel> GetCompaniesWithLocation();
        IEnumerable<CompanyListViewModel> CompaniesAdvancedSearchQuery(long orgId, long individualId, AdvancedSearchParams parameters);
        IEnumerable<CompanyListViewModel> CompaniesNormalSearchQuery(long orgId, long individualId, NormalSearchParams parameters);
        IEnumerable<CompanyListViewModel> CompaniesFocusListQuery(long orgId, long individualId, FocusListParams parameters);
        List<CompanyListViewModel> GetFilteredCompaniesFromCache(long orgId, long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notshowclosecase, List<long?> recommendationIds, List<long?> conclusionIds, List<long?> serviceIds, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId, List<Guid?> homeCountryIds, long? sustainalyticsId);
        int CountCompanyDialogueByCompany(long orgId, long companyId, EngagementStatisticType engagementStatisticType);
        int CountCompanyDialogueByCaseProfile(long orgId, long caseProfileId, EngagementStatisticType engagementStatisticType);
        string GetLatestConferenceAndMeetingTime(long orgId, long caseProfileId);
        long GetCompanyIdFromCaseProfile(long caseProfileId);
        IList<I_EngagementDiscussionPoints> GetEngagementDiscussionPointsByCompanyId(long companyId);
        string GetInvestorInitiatives(long companyId);
        IList<I_EngagementOtherStakeholderViews> GetStakeholderViews(long companyId);
        I_Companies GetCompanyById(long? companyId);

        IList<ContactTypeViewModel> GetContactTypes();

        IList<CompanyManagementSystemModel> GetCompanyManagementSystems(long companyId);

        IList<CompanyPortfolioModel> GetCompanyPortfolioModels(long companyId);

        IList<CompanyPortfolioModel> GetCompanyFollowingPortfolioModels(long companyId);

        IEnumerable<MsciFtseViewModel> GetMsciViewModels(long orgId, long individualId);

        IEnumerable<MsciFtseViewModel> GetFtseViewModels(long orgId, long individualId);
        IQueryable<CompanyViewModel> GetGesCompaniesWithLocation();

        IEnumerable<CheckMasterCompanyViewModel> CheckMasterCompanyQuery(long orgId, long individualId, CheckMasterCompanyParams parameters);
    }
}
