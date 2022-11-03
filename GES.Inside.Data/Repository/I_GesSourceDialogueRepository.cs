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
    public class I_GesSourceDialogueRepository : GenericRepository<I_GesSourceDialogues>, II_GesSourceDialogueRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesSourceDialogueRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public List<I_GesSourceDialogues> GetSourceDialogues(long caseProfileId)
        {
            return _dbContext.I_GesSourceDialogues.Where(x => x.I_GesCaseReports_Id == caseProfileId).ToList();
        }

        public I_GesSourceDialogues GetById(long id)
        {
            return _dbContext.I_GesSourceDialogues.FirstOrDefault(d => d.I_GesSourceDialogues_Id == id);
        }
    }
}
