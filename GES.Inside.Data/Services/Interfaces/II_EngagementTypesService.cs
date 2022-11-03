using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_EngagementTypesService : IEntityService<I_EngagementTypes>
    {
        I_EngagementTypes GetById(long id);

        EngagementTypeViewModel GetEngagementTypeModel(long engagementTypeId, long orgId);
        IEnumerable<I_EngagementTypeCategories> AllEngagementTypeCategories();
        PaginatedResults<GesContact> GetGesContacts(JqGridViewModel jqGridParams, long orgId);
        PaginatedResults<EngagementTypeViewModel> GetEngagementTypes(JqGridViewModel jqGridParams, long orgId);
        IEnumerable<EngagementTypeViewModel> GetEngagementTypeModelByCategoryId(long categoryId, long orgId);
        IEnumerable<EngagementTypeViewModel> GetAllActiveEngagementType(long orgId);
        IEnumerable<GesDocTypes> GetGesDocTypes();
        
    }
}
