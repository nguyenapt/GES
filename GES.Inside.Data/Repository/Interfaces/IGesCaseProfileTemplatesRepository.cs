using GES.Inside.Data.DataContexts;
using System;
using System.Collections;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCaseProfileTemplatesRepository : IGenericRepository<GesCaseProfileTemplates>
    {
        IList<GesCaseProfileTemplatesViewModel> GetGesCaseProfileInvisibleEntitiesViews(long engagementTypesId, long gesCaseReportStatusesId);
        IList<GesCaseProfileTemplatesViewModel> GetGesCaseProfileInvisibleEntitiesClientViews(long engagementTypesId, long gesCaseReportStatusesId);
        PaginatedResults<GesCaseProfileTemplatesViewModel> GetCaseProfilesUITemplate(JqGridViewModel jqGridParams);
        GesCaseProfileTemplatesViewModel GetViewModelById(Guid templateId);
        IEnumerable<I_GesCaseProfileEntities> GetGesCaseProfileEntities();
        IEnumerable<GesCaseProfileEntitiesGroupViewModel> I_GesCaseProfileEntitiesGroup();
        IEnumerable<GesCaseProfileEntitiesGroupViewModel> I_GesCaseProfileEntitiesGroupClient();        
        GesCaseProfileTemplates GetById(Guid gesCaseProfileTemplatesId);
        bool CheckExistedTemplate(long? engagementTypesId, long? recomendationId);
    }
}
