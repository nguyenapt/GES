using System;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_NormsService : IEntityService<I_Norms>
    {
        I_Norms GetById(long id);
        IEnumerable<NormModel> GetAllNorms();
        PaginatedResults<NormModel> GetAllConventionsForGrid(JqGridViewModel jqGridParams);
    }
}
