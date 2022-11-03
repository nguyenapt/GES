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
    public class I_EngagementDiscussionPointsRepository : GenericRepository<I_EngagementDiscussionPoints>, II_EngagementDiscussionPointsRepository
    {
        private readonly GesEntities _dbContext;
        public I_EngagementDiscussionPointsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }        

        public IList<DiscussionPointsViewModel> GetEngagementDiscussionPointsByCompanyId(long companyId)
        {
            return SafeExecute(() =>
            {
                var discussionPoints = from dp in _dbContext.I_EngagementDiscussionPoints
                                       join c in _dbContext.I_Companies on dp.I_Companies_Id equals c.I_Companies_Id
                                       where dp.I_Companies_Id == companyId
                                       select new DiscussionPointsViewModel() {
                                           DiscussionPointsId = dp.I_EngagementDiscussionPoints_Id,
                                           CompanyId = dp.I_Companies_Id,
                                           Name = dp.Name,
                                           Description = dp.Description,
                                           Created = dp.Created
                                       };
                return discussionPoints.ToList();
            });
        }

        public I_EngagementDiscussionPoints GetById(long id)
        {
            return this.SafeExecute<I_EngagementDiscussionPoints>(() => _entities.Set<I_EngagementDiscussionPoints>().Find(id));
        }
    }
}
