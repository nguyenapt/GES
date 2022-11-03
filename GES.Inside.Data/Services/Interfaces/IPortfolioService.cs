using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IPortfolioService: IEntityService<I_Portfolios>
    {
        PaginatedResults<PortfolioViewModel> GetGesPortfolios(JqGridViewModel jqGridParams, long orgId);

        PaginatedResults<PortfolioControActivPresetListViewModel> GetControActivPresets(JqGridViewModel jqGridParams);

        PaginatedResults<CompanyPortfolioViewModel> GetCompanyListForPortfolioDetails(JqGridViewModel jqGridParams, long id);

        PaginatedResults<PendingCompanyViewModel> GetPendingCompanies(JqGridViewModel jqGridParams, long portfolioId);

        IEnumerable<PendingCompanyViewModel> GetPendingCompanies(long portfolioId);

        PaginatedResults<PortfolioControActivityViewModel> GetControActivities(JqGridViewModel jqGridParams, long portfolioId);

        String[] GetPortfolioTypes();

        IEnumerable<IdNameModel> GetPortfolioTypeItems();

        I_Portfolios GetById(long id);

        List<IdNameModel> GetPortfoliosWithTerm(string term, int limit, bool onlyStandardPortfolios = false);

        IEnumerable<I_Portfolios> GetStandardPortfolios();

        List<GesStandardUniverseViewModel> GetGesStandardUniverseCompanies(bool includeChildren = false);

        List<GesStandardUniverseViewModel> GetGesStandardUniversePendingCompanies();
        
        IEnumerable<PortfolioViewModel> GetGesPortfoliosByClientId(long orgId);

    }
}
