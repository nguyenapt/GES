using System;
using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_EngagementTypesRepository : IGenericRepository<I_EngagementTypes>
    {
        I_EngagementTypes GetById(long id);
        EngagementTypeViewModel GetEngagementTypeModel(long engagementTypeId, long orgId);
        void DeleteEngagementTypesGesDocument(Guid id);
        void AddEngagementTypesGesDocument(I_EngagementTypes_GesDocument engagementTypesGesDocument);
        IQueryable<EngagementTypeViewModel> GetEngagementTypes(long orgId);
        IEnumerable<EngagementTypeViewModel> GetEngagementTypeModelByCategoryId(long categoryId, long orgId);
    }
}
