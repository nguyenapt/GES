using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfoliosG_OrganizationsI_ControversialActivitesService : IEntityService<I_PortfoliosG_OrganizationsI_ControversialActivites>
    {
        IEnumerable<I_PortfoliosG_OrganizationsI_ControversialActivites> GetByPortfolioOrgId(long portfolioOrgId);
        bool RemovePortfolioOrgControversialByPortfolioOrgId(long portfolioOrgId);
    }
}
