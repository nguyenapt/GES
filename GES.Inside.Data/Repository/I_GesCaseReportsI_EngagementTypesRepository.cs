using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.DataContexts;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportsI_EngagementTypesRepository : GenericRepository<I_GesCaseReportsI_EngagementTypes>, II_GesCaseReportsI_EngagementTypesRepository
    {
        public I_GesCaseReportsI_EngagementTypesRepository(GesEntities dbContext, IGesLogger logger)
           : base(dbContext, logger)
        {
        }
        
        public I_GesCaseReportsI_EngagementTypes FindById(long id)
        {
            return this.SafeExecute<I_GesCaseReportsI_EngagementTypes>(() => _entities.Set<I_GesCaseReportsI_EngagementTypes>().Find(id));
        }

        //TODO men.do Consider to change relationship many-many to one-many
        public IEnumerable<I_GesCaseReportsI_EngagementTypes> GetByCaseProfileId(long caseProfileId)
        {
            return this.SafeExecute<IEnumerable<I_GesCaseReportsI_EngagementTypes>>(() => _entities.Set<I_GesCaseReportsI_EngagementTypes>().Where(i => i.I_GesCaseReports_Id == caseProfileId));
        }
    }
}