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
    public class GesUNGPAssessmentFormResourcesRepository : GenericRepository<GesUNGPAssessmentFormResources>, IGesUNGPAssessmentFormResourcesRepository
    {
        private readonly GesEntities _dbContext;
        public GesUNGPAssessmentFormResourcesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public IEnumerable<GesUNGPAssessmentFormResources> GetGesUNGPAssessmentFormResourcesByFormId(Guid formId){
                                        
            var resourcesList = from n in _dbContext.GesUNGPAssessmentFormResources
                where n.GesUNGPAssessmentFormId == formId
                select n;

            return resourcesList;
        }

        public GesUNGPAssessmentFormResources GetById(Guid id)
        {
            return this.SafeExecute<GesUNGPAssessmentFormResources>(() => _entities.Set<GesUNGPAssessmentFormResources>().FirstOrDefault(d => d.GesUNGPAssessmentFormResourcesId == id));
        }
    }
}
