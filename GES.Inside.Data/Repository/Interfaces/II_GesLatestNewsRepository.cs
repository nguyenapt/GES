using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesLatestNewsRepository : IGenericRepository<I_GesLatestNews>
    {
        IEnumerable<I_GesLatestNews> GetGesLatestNewsByCaseProfileId(long caseProfileId);
        I_GesLatestNews GetById(long id);
    }
}
