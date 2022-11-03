using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class G_Organizations_GesDocumentRepository : GenericRepository<G_Organizations_GesDocument>, IG_Organizations_GesDocumentRepository
    {
        private GesEntities _context;
        public G_Organizations_GesDocumentRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void AddBatch(List<G_Organizations_GesDocument> entities)
        {
            _context.BulkInsert(entities);
            _context.SaveChanges();
        }

    }
}
