using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using EntityFramework.BulkInsert.Extensions;

namespace GES.Inside.Data.Repository
{
    public class I_PortfoliosI_CompaniesRepository : GenericRepository<I_PortfoliosI_Companies>, II_PortfoliosI_CompaniesRepository
    {
        private DbContext _context;
        public I_PortfoliosI_CompaniesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void AddBatch(List<I_PortfoliosI_Companies> entities)
        {
            this.SafeExecute(() =>
            {
                _context.BulkInsert(entities);
                _context.SaveChanges();
            });
        }
    }
}
