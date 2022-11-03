using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class I_EngagementTypesService : EntityService<GesEntities, I_EngagementTypes>, II_EngagementTypesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_EngagementTypesRepository _engagementTypesRepository;

        public I_EngagementTypesService(IUnitOfWork<GesEntities> unitOfWork, II_EngagementTypesRepository engagementTypesRepository, IGesLogger logger)
            : base(unitOfWork, logger, engagementTypesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _engagementTypesRepository = engagementTypesRepository;
        }


        public I_EngagementTypes GetById(long id)
        {
            return _engagementTypesRepository.GetById(id);
        }
        public EngagementTypeViewModel GetEngagementTypeModel(long engagementTypeId, long orgId)
        {
            return _engagementTypesRepository.GetEngagementTypeModel(engagementTypeId, orgId);
        }

        public IEnumerable<I_EngagementTypeCategories> AllEngagementTypeCategories()
        {
            var result = _dbContext.I_EngagementTypeCategories.FromCache();

            return result;
        }

        public PaginatedResults<EngagementTypeViewModel> GetEngagementTypes(JqGridViewModel jqGridParams, long orgId)
        {
            var engagementTypes = _engagementTypesRepository.GetEngagementTypes(orgId);

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<EngagementTypeViewModel>(jqGridParams);
                engagementTypes = string.IsNullOrEmpty(finalRules) ? engagementTypes : engagementTypes.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "catalogname":
                        engagementTypes = sortDir == "asc"
                            ? engagementTypes.OrderBy(x => x.CatalogName)
                            : engagementTypes.OrderByDescending(x => x.CatalogName);
                        break;
                    case "name":
                        engagementTypes = sortDir == "asc"
                            ? engagementTypes.OrderBy(x => x.Name).ThenBy(d => d.CatalogName)
                            : engagementTypes.OrderByDescending(x => x.Name).ThenBy(d => d.CatalogName);
                        break;
                    case "contact":
                        engagementTypes = sortDir == "asc"
                            ? engagementTypes.OrderBy(x => x.ContactFullName).ThenBy(d => d.CatalogName)
                            : engagementTypes.OrderByDescending(x => x.ContactFullName).ThenBy(d => d.CatalogName);
                        break;
                    case "sortorder":
                        engagementTypes = sortDir == "asc"
                            ? engagementTypes.OrderBy(x => x.SortOrder).ThenBy(d => d.CatalogName)
                            : engagementTypes.OrderByDescending(x => x.SortOrder).ThenBy(d => d.CatalogName);
                        break;
                    case "created":
                        engagementTypes = sortDir == "asc"
                            ? engagementTypes.OrderBy(x => x.Created).ThenBy(d => d.CatalogName)
                            : engagementTypes.OrderByDescending(x => x.Created).ThenBy(d => d.CatalogName);
                        break;
                }
            }

            return engagementTypes.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);


        }

        public IEnumerable<EngagementTypeViewModel> GetEngagementTypeModelByCategoryId(long categoryId,long orgId)
        {
            return _engagementTypesRepository.GetEngagementTypeModelByCategoryId(categoryId, orgId);
        }

        public IEnumerable<EngagementTypeViewModel> GetAllActiveEngagementType(long orgId)
        {
            var engagementTypes = _engagementTypesRepository.GetEngagementTypes(orgId);

            return engagementTypes;
        }

        public IEnumerable<GesDocTypes> GetGesDocTypes()
        {
            var result = _dbContext.GesDocTypes.FromCache();

            return result;
        }

        public PaginatedResults<GesContact> GetGesContacts(JqGridViewModel jqGridParams, long orgId)
        {

            
            var contacts = from u in _dbContext.G_Users
                join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id
                join s in _dbContext.G_UsersG_SecurityGroups on u.G_Users_Id equals s.G_Users_Id
                where s.G_SecurityGroups_Id == (int)SecurityGroups.Analyst
                select new GesContact
                {
                    UserId = u.G_Users_Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Email = i.Email,
                    JobTitle = i.JobTitle
                };

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesContact>(jqGridParams);
                contacts = string.IsNullOrEmpty(finalRules) ? contacts : contacts.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "userid":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.UserId)
                            : contacts.OrderByDescending(x => x.UserId);
                        break;
                    case "firstname":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.FirstName).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.FirstName).ThenBy(d => d.UserId);
                        break;
                    case "lastname":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.LastName).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.LastName).ThenBy(d => d.UserId);
                        break;
                    case "email":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.Email).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.Email).ThenBy(d => d.UserId);
                        break;
                    case "jobtitle":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.JobTitle).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.JobTitle).ThenBy(d => d.UserId);
                        break;
                }
            }

            return contacts.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);


        }
    }
}
