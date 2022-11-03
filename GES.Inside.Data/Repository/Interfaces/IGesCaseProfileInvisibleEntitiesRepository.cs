using GES.Inside.Data.DataContexts;
using System;
using System.Collections;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCaseProfileInvisibleEntitiesRepository : IGenericRepository<GesCaseProfileInvisibleEntities>
    {
        IEnumerable<GesCaseProfileInvisibleEntities> GetAllEntitiesByTemplateId(Guid gesCaseProfileTemplatesId);
    }
}
