using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_EngagementTypeNewsService : IEntityService<I_EngagementTypeNews>
    {
        I_EngagementTypeNews GetById(long id);
        IEnumerable<I_EngagementTypeNews> GetEngagementTypesNewsByEngagementTypesId(long engagementTypesId);
    }
}
