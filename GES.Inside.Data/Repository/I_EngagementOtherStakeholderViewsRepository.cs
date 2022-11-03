using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository
{
    public class I_EngagementOtherStakeholderViewsRepository : GenericRepository<I_EngagementOtherStakeholderViews>, II_EngagementOtherStakeholderViewsRepository
    {
        private readonly GesEntities _dbContext;
        public I_EngagementOtherStakeholderViewsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }


        public IList<OtherStakeholderViewModel> GetStakeholderViews(long companyId)
        {
            return SafeExecute(() =>
            {
                var stakeholderViews = from ot in _dbContext.I_EngagementOtherStakeholderViews
                                       where ot.I_Companies_Id == companyId
                                       select new OtherStakeholderViewModel() {
                                           OtherStakeholderViewsId = ot.I_EngagementOtherStakeholderViews_Id,
                                           CompanyId = ot.I_Companies_Id,
                                           Name = ot.Name,
                                           Description = ot.Description,
                                           Url = ot.Url,
                                           Created = ot.Created
                                       };
                return stakeholderViews.ToList();
            });
        }


        public I_EngagementOtherStakeholderViews GetById(long id)
        {
            return this.SafeExecute<I_EngagementOtherStakeholderViews>(() => _entities.Set<I_EngagementOtherStakeholderViews>().Find(id));
        }
    }
}
