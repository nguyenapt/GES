using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_EngagementOtherStakeholderViewsRepository : IGenericRepository<I_EngagementOtherStakeholderViews>
    {
        IList<OtherStakeholderViewModel> GetStakeholderViews(long companyId);

        I_EngagementOtherStakeholderViews GetById(long id);
    }
}
