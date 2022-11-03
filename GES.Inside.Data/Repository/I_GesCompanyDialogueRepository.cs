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
    public class I_GesCompanyDialogueRepository : GenericRepository<I_GesCompanyDialogues>, II_GesCompanyDialogueRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCompanyDialogueRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public List<I_GesCompanyDialogues> GetCompanyDialogues(long caseProfileId)
        {
            return _dbContext.I_GesCompanyDialogues.Where(x => x.I_GesCaseReports_Id == caseProfileId).ToList();
        }

        public I_GesCompanyDialogues GetById(long id)
        {
            return _dbContext.I_GesCompanyDialogues.FirstOrDefault(x => x.I_GesCompanyDialogues_Id == id);
        }
    }
}
