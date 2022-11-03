using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class I_TimelineItemsService : EntityService<GesEntities, I_TimelineItems>, II_TimelineItemsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_TimeLineItemsRepository _timeLineItemsRepository;

        public I_TimelineItemsService(IUnitOfWork<GesEntities> unitOfWork, II_TimeLineItemsRepository timeLineItemsRepository, IGesLogger logger)
            : base(unitOfWork, logger, timeLineItemsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _timeLineItemsRepository = timeLineItemsRepository;
        }


        public I_TimelineItems GetById(long id)
        {
            return _timeLineItemsRepository.GetById(id);
        }

        public IEnumerable<I_TimelineItems> GetTimelineItemsByEngagementTypesId(long engagementTypesId)
        {
            var timelines = from t in _dbContext.I_TimelineItems
                       where t.I_EngagementTypes_Id == engagementTypesId
                select t;

            return timelines;
        }
    }
}
