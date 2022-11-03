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
    public class GesCaseReports_Audit_DetailRepository : GenericRepository<GesCaseReports_Audit_Details>, IGesCaseReports_Audit_DetailRepository
    {
        private readonly GesEntities _dbContext;
        public GesCaseReports_Audit_DetailRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

    }
}
