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
using com.sun.org.apache.bcel.@internal.generic;
using DocumentFormat.OpenXml.Office2010.Word;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Repository
{
    public class GesCaseProfileTemplatesRepository : GenericRepository<GesCaseProfileTemplates>, IGesCaseProfileTemplatesRepository
    {
        private readonly GesEntities _dbContext;
        public GesCaseProfileTemplatesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }


        public IList<GesCaseProfileTemplatesViewModel> GetGesCaseProfileInvisibleEntitiesViews(long engagementTypesId, long gesCaseReportStatusesId)
        {
            return SafeExecute(() =>
            {
                var caseProfileInvisibleEntitiesViews = from t1 in _dbContext.GesCaseProfileInvisibleEntities
                                                        join t2 in _dbContext.GesCaseProfileTemplates on t1.GesCaseProfileTemplates_Id equals t2.GesCaseProfileTemplates_Id
                                                        join t3 in _dbContext.I_GesCaseProfileEntities on t1.GesCaseProfileEntity_Id equals t3.GesCaseProfileEntity_Id
                                                        where t2.I_EngagementTypes_Id == engagementTypesId 
                                                              && t2.I_GesCaseReportStatuses_Id == gesCaseReportStatusesId 
                                                              && (t1.InVisibleType==1 || t1.InVisibleType==3)
                                                        select new GesCaseProfileTemplatesViewModel()
                                                        {
                                                            EntityName = t3.Name,
                                                            EntityCodeType = t3.Type
                                                        };


                if (caseProfileInvisibleEntitiesViews.ToList().Count != 0)
                    return caseProfileInvisibleEntitiesViews.ToList();

                var defaultTemplateEntitiesView = from t1 in _dbContext.GesCaseProfileInvisibleEntities
                    join t2 in _dbContext.GesCaseProfileTemplates on t1.GesCaseProfileTemplates_Id equals t2.GesCaseProfileTemplates_Id
                    join t3 in _dbContext.I_GesCaseProfileEntities on t1.GesCaseProfileEntity_Id equals t3.GesCaseProfileEntity_Id
                    where t2.I_EngagementTypes_Id == null 
                          && t2.I_GesCaseReportStatuses_Id == null 
                          && (t1.InVisibleType==1 || t1.InVisibleType==3)
                    select new GesCaseProfileTemplatesViewModel()
                    {
                        EntityName = t3.Name,
                        EntityCodeType = t3.Type
                    };
                return defaultTemplateEntitiesView.ToList();

            });
        }

        public IList<GesCaseProfileTemplatesViewModel> GetGesCaseProfileInvisibleEntitiesClientViews(long engagementTypesId, long gesCaseReportStatusesId)
        {
            return SafeExecute(() =>
            {
                var caseProfileInvisibleEntitiesViews = from t1 in _dbContext.GesCaseProfileInvisibleEntities
                                                        join t2 in _dbContext.GesCaseProfileTemplates on t1.GesCaseProfileTemplates_Id equals t2.GesCaseProfileTemplates_Id
                                                        join t3 in _dbContext.I_GesCaseProfileEntities on t1.GesCaseProfileEntity_Id equals t3.GesCaseProfileEntity_Id
                                                        where t2.I_EngagementTypes_Id == engagementTypesId && t2.I_GesCaseReportStatuses_Id == gesCaseReportStatusesId && (t1.InVisibleType == 2 || t1.InVisibleType == 3)
                                                        select new GesCaseProfileTemplatesViewModel()
                                                        {
                                                            EntityName = t3.Name,
                                                            EntityCodeType = t3.Type
                                                            
                                                        };
                if (caseProfileInvisibleEntitiesViews.ToList().Count != 0)
                return caseProfileInvisibleEntitiesViews.ToList();
                
                var defaultTemplateEntitiesView =  from t1 in _dbContext.GesCaseProfileInvisibleEntities
                    join t2 in _dbContext.GesCaseProfileTemplates on t1.GesCaseProfileTemplates_Id equals t2.GesCaseProfileTemplates_Id
                    join t3 in _dbContext.I_GesCaseProfileEntities on t1.GesCaseProfileEntity_Id equals t3.GesCaseProfileEntity_Id
                    where t2.I_EngagementTypes_Id == null && t2.I_GesCaseReportStatuses_Id == null && (t1.InVisibleType == 2 || t1.InVisibleType == 3)
                    select new GesCaseProfileTemplatesViewModel()
                    {
                        EntityName = t3.Name,
                        EntityCodeType = t3.Type
                                                            
                    };
                
                return defaultTemplateEntitiesView.ToList();
            });
        }

        public PaginatedResults<GesCaseProfileTemplatesViewModel> GetCaseProfilesUITemplate(JqGridViewModel jqGridParams)
        {
            var services = from t1 in _dbContext.GesCaseProfileTemplates
                           join t2 in _dbContext.I_EngagementTypes on t1.I_EngagementTypes_Id equals t2.I_EngagementTypes_Id into t21
                           from t2 in t21.DefaultIfEmpty()
                           join t3 in _dbContext.I_GesCaseReportStatuses on t1.I_GesCaseReportStatuses_Id equals t3.I_GesCaseReportStatuses_Id into t31
                           from t3 in t31.DefaultIfEmpty()
                           select new GesCaseProfileTemplatesViewModel
                           {
                               GesCaseProfileTemplatesId = t1.GesCaseProfileTemplates_Id,
                               TemplateName = t1.Name,
                               TemplateDescription = t1.Description,
                               EngagementType = t2.Name,
                               Recomendation = t3.Name
                           };

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesCaseProfileTemplatesViewModel>(jqGridParams);
                services = string.IsNullOrEmpty(finalRules) ? services : services.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir))
                return services.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);

            switch (sortCol)
            {
                case "templatename":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.TemplateName)
                        : services.OrderByDescending(x => x.TemplateName);
                    break;
                case "engagementtype":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.EngagementType).ThenBy(d => d.TemplateName)
                        : services.OrderByDescending(x => x.EngagementType).ThenBy(d => d.TemplateName);
                    break;
                case "recomendation":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.Recomendation).ThenBy(d => d.TemplateName)
                        : services.OrderByDescending(x => x.Recomendation).ThenBy(d => d.TemplateName);
                    break;
            }

            return services.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public GesCaseProfileTemplatesViewModel GetViewModelById(Guid templateId)
        {
            return SafeExecute(() =>
            {
                var caseProfileInvisibleEntitiesViews = (from t1 in _dbContext.GesCaseProfileTemplates
                                                         where t1.GesCaseProfileTemplates_Id == templateId
                                                         select new GesCaseProfileTemplatesViewModel()
                                                         {
                                                             GesCaseProfileTemplatesId = t1.GesCaseProfileTemplates_Id,
                                                             TemplateName = t1.Name,
                                                             TemplateDescription = t1.Description,
                                                             EngagementTypeId = t1.I_EngagementTypes_Id,
                                                             RecomendationId = t1.I_GesCaseReportStatuses_Id

                                                         }).FirstOrDefault();

                var gesCaseProfileInvisibleEntitiesViewModels = (from et in _dbContext.GesCaseProfileInvisibleEntities
                                                                 join en in _dbContext.I_GesCaseProfileEntities on et.GesCaseProfileEntity_Id equals en
                                                                     .GesCaseProfileEntity_Id
                                                                 where et.GesCaseProfileTemplates_Id == templateId
                                                                 select new GesCaseProfileInvisibleEntitiesViewModel()
                                                                 {
                                                                     GesCaseProfileInvisibleEntity_Id = et.GesCaseProfileInvisibleEntity_Id,
                                                                     GesCaseProfileEntity_Id = et.GesCaseProfileEntity_Id,
                                                                     GesCaseProfileTemplates_Id = et.GesCaseProfileTemplates_Id,
                                                                     EntityName = en.Name,
                                                                     EntityCodeType = en.Type,
                                                                     InVisibleType = et.InVisibleType
                                                                 }).ToList();

                if (caseProfileInvisibleEntitiesViews != null)
                {
                    caseProfileInvisibleEntitiesViews.ProfileInvisibleEntitiesViewModels =
                        gesCaseProfileInvisibleEntitiesViewModels;

                }

                return caseProfileInvisibleEntitiesViews;
            });
        }

        public IEnumerable<I_GesCaseProfileEntities> GetGesCaseProfileEntities()
        {
            return SafeExecute(() =>
            {
                var entities = _dbContext.I_GesCaseProfileEntities;

                return entities.OrderBy(x => x.Order);

            });
        }

        public IEnumerable<GesCaseProfileEntitiesGroupViewModel> I_GesCaseProfileEntitiesGroup()
        {
            return SafeExecute(() =>
            {
                var group = (from g in _dbContext.I_GesCaseProfileEntitiesGroup
                             where g.VisibleType ==1 || g.VisibleType ==3
                             select new GesCaseProfileEntitiesGroupViewModel()
                             {
                                 GesCaseProfileEntitiesGroupId = g.I_GesCaseProfileEntitiesGroup_Id,
                                 Name = g.Name,
                                 GroupType = g.GroupType,
                                 Order = g.Order,
                                 CaseProfileEntities = (from e in _dbContext.I_GesCaseProfileEntities
                                                        where e.I_GesCaseProfileEntitiesGroup_Id == g.I_GesCaseProfileEntitiesGroup_Id && (e.VisibleType == 1 || e.VisibleType == 3)
                                                        select new GesCaseProfileEntitiesViewModel()
                                                        {
                                                            id = e.Type,
                                                            name = e.Name,
                                                            GesCaseProfileEntity_Id = e.GesCaseProfileEntity_Id,
                                                            Order = e.Order
                                                        }).ToList().OrderBy(x => x.Order)
                             }).ToList();


                return group.OrderBy(x => x.Order);

            });
        }

        public IEnumerable<GesCaseProfileEntitiesGroupViewModel> I_GesCaseProfileEntitiesGroupClient()
        {
            return SafeExecute(() =>
            {
                var group = (from g in _dbContext.I_GesCaseProfileEntitiesGroup
                             where g.VisibleType == 2 || g.VisibleType == 3
                             select new GesCaseProfileEntitiesGroupViewModel()
                             {
                                 GesCaseProfileEntitiesGroupId = g.I_GesCaseProfileEntitiesGroup_Id,
                                 Name = g.Name,
                                 GroupType = g.GroupType,
                                 Order = g.Order,
                                 CaseProfileEntities = (from e in _dbContext.I_GesCaseProfileEntities
                                                        where e.I_GesCaseProfileEntitiesGroup_Id == g.I_GesCaseProfileEntitiesGroup_Id && (e.VisibleType == 2 || e.VisibleType == 3)
                                                        select new GesCaseProfileEntitiesViewModel()
                                                        {
                                                            id = e.Type,
                                                            name = e.Name,
                                                            GesCaseProfileEntity_Id = e.GesCaseProfileEntity_Id,
                                                            Order = e.Order
                                                        }).ToList().OrderBy(x => x.Order)
                             }).ToList();


                return group.OrderBy(x => x.Order);

            });
        }

        public GesCaseProfileTemplates GetById(Guid gesCaseProfileTemplatesId)
        {
            return this.SafeExecute<GesCaseProfileTemplates>(() =>
                _entities.Set<GesCaseProfileTemplates>().FirstOrDefault(d => d.GesCaseProfileTemplates_Id == gesCaseProfileTemplatesId));
        }

        public bool CheckExistedTemplate(long? engagementTypesId, long? recomendationId)
        {
            var template = _dbContext.GesCaseProfileTemplates.FirstOrDefault(x => x.I_EngagementTypes_Id == engagementTypesId && x.I_GesCaseReportStatuses_Id == recomendationId);

            return template != null;
        }
    }
}
