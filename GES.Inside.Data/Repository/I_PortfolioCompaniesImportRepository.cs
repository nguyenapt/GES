using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Extensions;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_PortfolioCompaniesImportRepository : GenericRepository<I_PortfolioCompaniesImport>, II_PortfolioCompaniesImportRepository
    {
        private DbContext _context;
        public I_PortfolioCompaniesImportRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void AddBatch(List<I_PortfolioCompaniesImport> entities)
        {
            this.SafeExecute(() =>
            {
                _context.BulkInsert(entities);
                _context.SaveChanges();
            });
        }
    }
}
