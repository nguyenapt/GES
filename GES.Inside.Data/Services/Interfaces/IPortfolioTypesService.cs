using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IPortfolioTypesService: IEntityService<I_PortfolioTypes>
    {
        I_PortfolioTypes GetByName(string name);
    }
}
