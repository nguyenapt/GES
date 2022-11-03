using System;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Enumeration;
using GES.Inside.Data.Models.StoredProcedureParams;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGSSResearchCompaniesRepository : IGenericRepository<I_Companies>
    {

        IQueryable<GssResearchCompanyViewModel> GetGssResearchCompanies();

    }
}
