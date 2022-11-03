using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_EngagementTypeNewsRepository : IGenericRepository<I_EngagementTypeNews>
    {
        I_EngagementTypeNews GetById(long id);

    }
}
