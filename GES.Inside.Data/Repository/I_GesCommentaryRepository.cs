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
    public class I_GesCommentaryRepository : GenericRepository<I_GesCommentary>, II_GesCommentaryRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCommentaryRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }


        public IEnumerable<I_GesCommentary> GetGesCommentariesByCaseProfileId(long gesCaseReportsId){
                                        
            var commentaries = from n in _dbContext.I_GesCommentary
                where n.I_GesCaseReports_Id == gesCaseReportsId
                select n;

            return commentaries;
        }

        public I_GesCommentary GetById(long id)
        {
            return this.SafeExecute<I_GesCommentary>(() => _entities.Set<I_GesCommentary>().FirstOrDefault(d => d.I_GesCommentary_Id == id));
        }
    }
}
