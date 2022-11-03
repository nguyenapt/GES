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
    public interface IGSSResearchCompaniesService : IEntityService<I_Companies>
    {
        PaginatedResults<GssResearchCompanyViewModel> GetGSSResearchCompanies(JqGridViewModel jqGridParams);
    }
}
