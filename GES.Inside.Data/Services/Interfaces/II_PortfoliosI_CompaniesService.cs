using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfoliosI_CompaniesService : IEntityService<I_PortfoliosI_Companies>
    {
        IEnumerable<I_PortfoliosI_Companies> GetPortfolioCompaniesByPortfolioId(long portfolioId);

        bool RemovePortfolioCompaniesByPortfolioId(long portfolioId);

        int AddPortfolioCompaniesByList(List<I_PortfoliosI_Companies> listPortfoliosCompanieses);

        bool RemovePortfolioCompaniesByPortfolioIdAndCompanyID(long portfolioId, long companyId);

        void AddBatch(List<I_PortfoliosI_Companies> entities);

    }
}
