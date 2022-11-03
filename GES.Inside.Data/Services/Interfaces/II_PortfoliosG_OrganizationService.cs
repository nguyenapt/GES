using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfoliosG_OrganizationService : IEntityService<I_PortfoliosG_Organizations>
    {
        I_PortfoliosG_Organizations GetById(long id);

        List<I_PortfoliosG_Organizations> GetByPortfolioAndOrganizationIds(long portfolioId, long organizationId);

        long? GetOrganizationIdFromPortfolioOrganizationId(long poId);

        bool DeletePortfolioOrganization(long portfolioId, long organizationId);

        bool CheckExistPortfolioInPortfolioOrganization(long portfolioId);
    }
}