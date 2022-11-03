using System;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using GES.Inside.Data.Models;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Repository
{
    public class I_EngagementTypesRepository : GenericRepository<I_EngagementTypes>, II_EngagementTypesRepository
    {
        private readonly GesEntities _dbContext;

        public I_EngagementTypesRepository(GesEntities context, IGesLogger logger)
            : base(context, logger)
        {
            _dbContext = context;
        }

        public I_EngagementTypes GetById(long id)
        {
            return this.SafeExecute<I_EngagementTypes>(() => _entities.Set<I_EngagementTypes>()
                .FirstOrDefault(d => d.I_EngagementTypes_Id == id));
        }

        public EngagementTypeViewModel GetEngagementTypeModel(long engagementTypeId, long orgId)
        {
            var query = from e in _dbContext.I_EngagementTypes
                        join s in _dbContext.G_Services on e.I_EngagementTypes_Id equals s.I_EngagementTypes_Id into sg
                        from s in sg.DefaultIfEmpty()
                        join g in _dbContext.G_OrganizationsG_Services on new { s.G_Services_Id, G_Organizations_Id = orgId }
                        equals new { g.G_Services_Id, g.G_Organizations_Id }
                        into gg
                        from g in gg.DefaultIfEmpty()
                        join u in _dbContext.G_Users on e.ContactG_Users_Id equals u.G_Users_Id into ug
                        from u in ug.DefaultIfEmpty()
                        join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id into ig
                        from i in ig.DefaultIfEmpty()
                        where e.I_EngagementTypes_Id == engagementTypeId
                        select new EngagementTypeViewModel
                        {
                            Name = e.Name,
                            Description = e.Description,
                            GesReports = e.GesReports,
                            Goal = e.Goal,
                            I_EngagementTypeCategories_Id = e.I_EngagementTypeCategories_Id,
                            I_EngagementTypes_Id = e.I_EngagementTypes_Id,
                            LatestNews = e.LatestNews,
                            OtherInitiatives = e.OtherInitiatives,
                            NextStep = e.NextStep,
                            NonSubscriberInformation = e.NonSubscriberInformation,
                            ContactG_Users_Id = e.ContactG_Users_Id,
                            Sources = e.Sources,
                            Participants = e.Participants,
                            ServicesId = s != null ? s.G_Services_Id : 0,
                            IsSubscribe = g != null,
                            ContactFullName = i != null ? i.FirstName + " " + i.LastName : "",
                            ContactMobile = i != null ? i.Phone : "",
                            ContactEmail = i != null ? i.Email : "",
                            SortOrder = e.SortOrder,
                            Deactive = e.Deactive,
                            ThemeImagePath = e.ThemeImage,
                            IsShowInClientMenu = e.IsShowInClientMenu ?? true,
                            IsShowInCaseProfileTemplate = e.IsShowInCaseProfileTemplate ?? true,
                            Created = e.Created
                        };

            var engagementType = query.FirstOrDefault();
            if (engagementType != null)
            {
                engagementType.TimeLine = (from t in _dbContext.I_TimelineItems
                                           where t.I_EngagementTypes_Id == engagementTypeId
                                           select new EventListViewModel
                                           {
                                               EventDate = t.Date ?? DateTime.Now,
                                               Heading = t.Description,
                                               EngagementTypeId = engagementTypeId,
                                               Id = t.I_TimelineItems_Id,
                                               Created = t.Created
                                           }
                ).OrderByDescending(d => d.EventDate).ToList();

                engagementType.KPIs = (from k in _dbContext.I_Kpis
                                       where k.I_EngagementTypes_Id == engagementTypeId
                                       select new KpiViewModel
                                       {
                                           KpiId = k.I_Kpis_Id,
                                           KpiName = k.Name,
                                           KpiDescription = k.Description,
                                           Created = k.Created
                                       }).OrderBy(p => p.KpiId).ToList();

                engagementType.EngagementTypeNews = (from n in _dbContext.I_EngagementTypeNews
                                                     where n.I_EngagementTypes_Id == engagementTypeId
                                                     select new EngagementTypeNewsViewModel()
                                                     {
                                                         EngagementTypeNewsId = n.I_EngagementTypeNews_Id,
                                                         EngagementTypeNewsDescription = n.Description,
                                                         Created = n.Created
                                                     }).OrderBy(p => p.Created).ToList();

                engagementType.EngagementTypeDocuments = (from d in _dbContext.I_EngagementTypes_GesDocument
                                                          join gd in _dbContext.GesDocument on d.DocumentId equals gd.DocumentId
                                                          where d.EngagementTypeId == engagementTypeId
                                                          select new EngagementTypeGesDocumentViewModel()
                                                          {
                                                              Id = d.Id,
                                                              DocumentId = d.DocumentId,
                                                              Name = gd.Name,
                                                              FileName = gd.FileName,
                                                              DocumentTypeId = d.DocumentTypeId,
                                                              Created = d.Created,
                                                              ReportSection = gd.Metadata01,
                                                              HashCodeDocument = gd.HashCodeDocument
                                                          }).OrderByDescending(p => p.Name).ToList();
            }

            return engagementType;
        }

        
        
        public void DeleteEngagementTypesGesDocument(Guid id)
        {
            var document = _entities.Set<I_EngagementTypes_GesDocument>().FirstOrDefault(d => d.DocumentId == id);
            if (document != null) _entities.Set<I_EngagementTypes_GesDocument>().Remove(document);
        }

        public void AddEngagementTypesGesDocument(I_EngagementTypes_GesDocument engagementTypesGesDocument)
        {
            _entities.Set<I_EngagementTypes_GesDocument>().Add(engagementTypesGesDocument);
        }
        
        public IQueryable<EngagementTypeViewModel> GetEngagementTypes(long orgId)
        {
            var engagementTypes = from e in _dbContext.I_EngagementTypes
                                  join s in _dbContext.G_Services on e.I_EngagementTypes_Id equals s.I_EngagementTypes_Id into sg
                                  from s in sg.DefaultIfEmpty()
                                      //join g in _dbContext.G_OrganizationsG_Services on new { s.G_Services_Id, G_Organizations_Id = orgId } equals new { g.G_Services_Id, g.G_Organizations_Id }
                                      //    into gg
                                      //  from g in gg.DefaultIfEmpty()
                                  join c in _dbContext.I_EngagementTypeCategories on e.I_EngagementTypeCategories_Id equals c
                                      .I_EngagementTypeCategories_Id
                                  join u in _dbContext.G_Users on e.ContactG_Users_Id equals u.G_Users_Id into ug
                                  from u in ug.DefaultIfEmpty()
                                  join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id into ig
                                  from i in ig.DefaultIfEmpty()
                                  select new EngagementTypeViewModel
                                  {
                                      Name = e.Name,
                                      Description = e.Description,
                                      GesReports = e.GesReports,
                                      Goal = e.Goal,
                                      I_EngagementTypeCategories_Id = e.I_EngagementTypeCategories_Id,
                                      CatalogName = c.Name,
                                      I_EngagementTypes_Id = e.I_EngagementTypes_Id,
                                      LatestNews = e.LatestNews,
                                      OtherInitiatives = e.OtherInitiatives,
                                      NextStep = e.NextStep,
                                      NonSubscriberInformation = e.NonSubscriberInformation,
                                      ContactG_Users_Id = e.ContactG_Users_Id,
                                      Sources = e.Sources,
                                      Participants = e.Participants,
                                      ServicesId = s != null ? s.G_Services_Id : 0,
                                      //  IsSubscribe = g != null,
                                      ContactFullName = i != null ? i.FirstName + " " + i.LastName : "",
                                      ContactMobile = i != null ? i.Phone : "",
                                      ContactEmail = i != null ? i.Email : "",
                                      SortOrder = e.SortOrder,
                                      ThemeImage = e.ThemeImage,
                                      Deactive = e.Deactive,
                                      IsShowInClientMenu = e.IsShowInClientMenu ?? false,
                                      IsShowInCaseProfileTemplate = e.IsShowInCaseProfileTemplate ?? false,
                                      Created = e.Created
                                  };

            return engagementTypes;
        }

        public IEnumerable<EngagementTypeViewModel> GetEngagementTypeModelByCategoryId(long categoryId, long orgId)
        {
            var engagementTypes = from e in _dbContext.I_EngagementTypes
                                  join s in _dbContext.G_Services on e.I_EngagementTypes_Id equals s.I_EngagementTypes_Id into sg
                                  from s in sg.DefaultIfEmpty()
                                  join c in _dbContext.I_EngagementTypeCategories on e.I_EngagementTypeCategories_Id equals c
                                      .I_EngagementTypeCategories_Id
                                  join u in _dbContext.G_Users on e.ContactG_Users_Id equals u.G_Users_Id into ug
                                  from u in ug.DefaultIfEmpty()
                                  join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id into ig
                                  from i in ig.DefaultIfEmpty()
                                  join g in _dbContext.G_OrganizationsG_Services on new { s.G_Services_Id, G_Organizations_Id = orgId } equals new { g.G_Services_Id, g.G_Organizations_Id }
                                      into gg
                                  from g in gg.DefaultIfEmpty()
                                  where e.I_EngagementTypeCategories_Id == categoryId  &&  (e.IsShowInClientMenu != null && e.IsShowInClientMenu.Value)
                                  select new EngagementTypeViewModel
                                  {
                                      Name = e.Name,
                                      Description = e.Description,
                                      GesReports = e.GesReports,
                                      Goal = e.Goal,
                                      I_EngagementTypeCategories_Id = e.I_EngagementTypeCategories_Id,
                                      CatalogName = c.Name,
                                      I_EngagementTypes_Id = e.I_EngagementTypes_Id,
                                      LatestNews = e.LatestNews,
                                      OtherInitiatives = e.OtherInitiatives,
                                      NextStep = e.NextStep,
                                      NonSubscriberInformation = e.NonSubscriberInformation,
                                      ContactG_Users_Id = e.ContactG_Users_Id,
                                      Sources = e.Sources,
                                      Participants = e.Participants,
                                      ServicesId = s != null ? s.G_Services_Id : 0,
                                      IsSubscribe = g != null,
                                      ContactFullName = i != null ? i.FirstName + " " + i.LastName : "",
                                      ContactMobile = i != null ? i.Phone : "",
                                      ContactEmail = i != null ? i.Email : "",
                                      SortOrder = e.SortOrder,
                                      ThemeImage = e.ThemeImage,
                                      Deactive = e.Deactive,
                                      IsShowInClientMenu = e.IsShowInClientMenu ?? false,
                                      Created = e.Created
                                  };

            return engagementTypes;
        }
    }
}
