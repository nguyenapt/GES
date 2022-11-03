using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_EngagementDiscussionPointsRepository : IGenericRepository<I_EngagementDiscussionPoints>
    {
        IList<DiscussionPointsViewModel> GetEngagementDiscussionPointsByCompanyId(long companyId);

        I_EngagementDiscussionPoints GetById(long id);
    }
}
