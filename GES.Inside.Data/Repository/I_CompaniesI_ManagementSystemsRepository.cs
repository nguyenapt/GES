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
    public class I_CompaniesI_ManagementSystemsRepository : GenericRepository<I_CompaniesI_ManagementSystems>, II_CompaniesI_ManagementSystemsRepository
    {
        private readonly GesEntities _dbContext;
        public I_CompaniesI_ManagementSystemsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_CompaniesI_ManagementSystems GetById(long id)
        {
            return this.SafeExecute<I_CompaniesI_ManagementSystems>(() => _entities.Set<I_CompaniesI_ManagementSystems>().FirstOrDefault(d => d.I_CompaniesI_ManagementSystems_Id == id));
        }
    }
}
