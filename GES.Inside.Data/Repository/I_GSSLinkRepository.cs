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
    public class GSSLinkRepository : GenericRepository<I_GSSLink>, II_GSSLinkRepository
    {
        private readonly GesEntities _dbContext;
        public GSSLinkRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_GSSLink GetById(Guid id)
        {
            return this.SafeExecute<I_GSSLink>(() => _entities.Set<I_GSSLink>().FirstOrDefault(d => d.I_GSSLink_Id == id));
        }
    }
}
