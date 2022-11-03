using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfoliosI_ControversialActiviteService : IEntityService<I_PortfoliosI_ControversialActivites>
    {
        IEnumerable<I_PortfoliosI_ControversialActivites> GetByPortfolioId(long portfolioId);
    }
}
