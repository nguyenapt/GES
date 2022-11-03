using System;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_ConventionsService : IEntityService<I_Conventions>
    {
        I_Conventions GetById(long id);
        IEnumerable<ConventionModel> GetAllConventions();
        PaginatedResults<ConventionModel> GetAllConventionsForGrid(JqGridViewModel jqGridParams);
        IEnumerable<I_ConventionCategories> AllConventionCategories();
    }
}
