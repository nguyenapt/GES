using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Word;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Repository
{
    public class GesCaseProfileInvisibleEntitiesRepository : GenericRepository<GesCaseProfileInvisibleEntities>, IGesCaseProfileInvisibleEntitiesRepository
    {
        private readonly GesEntities _dbContext;
        public GesCaseProfileInvisibleEntitiesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public IEnumerable<GesCaseProfileInvisibleEntities> GetAllEntitiesByTemplateId(Guid gesCaseProfileTemplatesId)
        {
           
            return this.SafeExecute<IEnumerable<GesCaseProfileInvisibleEntities>>(() => _entities.Set<GesCaseProfileInvisibleEntities>().Where(i => i.GesCaseProfileTemplates_Id == gesCaseProfileTemplatesId));
        }
    }
}
