using System;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IExportExcelService
    {
        List<ExcelCaseProfile> GetDataForStandardCases(long orgId, List<long> portfolioIds, bool getNotShowedInClient = false);
        IEnumerable<ExcelCaseProfile> ExportPrescreeningSearchToExcel(long orgId, long individualId, bool onlyShowFocusList, string name, string isin, List<long> portfolioIds, bool notshowclosecase, bool isNew, string baseUrl, bool onlycompanieswithactivecase, List<long> companyIds,
            List<long?> recommendationId, List<long?> conclusionIds, List<long?> serviceId, long? normId, List<Guid?> locationIds, List<long?> responseIds, List<long?> progressIds, List<long?> industryIds, long? engagementTypeId,
            bool? isSearchCompany, bool? isSearchCaseName, bool? isSearchTag, List<Guid?> homeCountryIds, long? sustainalyticsId);
        IEnumerable<ExcelCaseProfile> ExportPrescreeningSearchAllCompaniesByPortfolioToExcel(long orgId, List<long> portfolioIds);
        List<CompanyListViewModel> ExportAllCompaniesByPortfolio(long orgId, List<long> portfolioIds);
        IEnumerable<ExcelCaseProfile> ExportScreeningReportToExcel(long clientId, long portfolioId, DateTime? fromDate, DateTime? toDate);
        List<CompanyListViewModel> GetCompaniesByPortfolioIdsListView(long orgId, List<long> portfolioIds, DateTime? fromDate);
        IEnumerable<ExcelCaseDetail> ExportEFStatusReport();
    }
}
