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
    public class I_EngagementTypeNewsRepository : GenericRepository<I_EngagementTypeNews>, II_EngagementTypeNewsRepository
    {
        private readonly GesEntities _dbContext;
        public I_EngagementTypeNewsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_EngagementTypeNews GetById(long id)
        {
            return this.SafeExecute<I_EngagementTypeNews>(() => _entities.Set<I_EngagementTypeNews>().FirstOrDefault(d => d.I_EngagementTypeNews_Id == id));
        }

    }
}
