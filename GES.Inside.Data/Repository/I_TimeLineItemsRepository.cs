using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_TimeLineItemsRepository : GenericRepository<I_TimelineItems>, II_TimeLineItemsRepository
    {
        private readonly GesEntities _dbContext;
        public I_TimeLineItemsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_TimelineItems GetById(long id)
        {
            return this.SafeExecute<I_TimelineItems>(() => _entities.Set<I_TimelineItems>().FirstOrDefault(d => d.I_TimelineItems_Id == id));
        }

    }
}
