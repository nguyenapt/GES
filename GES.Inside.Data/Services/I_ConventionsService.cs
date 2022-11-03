using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class I_ConventionsService: EntityService<GesEntities, I_Conventions>, II_ConventionsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_ConventionsRepository _conventionsRepository;

        public I_ConventionsService(IUnitOfWork<GesEntities> unitOfWork, II_ConventionsRepository conventionsRepository, IGesLogger logger)
            : base(unitOfWork, logger, conventionsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _conventionsRepository = conventionsRepository;
        }

        public I_Conventions GetById(long id)
        {
            return this.SafeExecute<I_Conventions>(() => _conventionsRepository.FindBy(x => x.I_Conventions_Id == id).FirstOrDefault());
        }

        public IEnumerable<ConventionModel> GetAllConventions()
        {
            var query = from m in _dbContext.I_Conventions
                select new ConventionModel()
                {
                    I_Conventions_Id = m.I_Conventions_Id,
                    I_ConventionCategories_Id = m.I_ConventionCategories_Id,
                    Source = m.Source,
                    Code = m.Code,
                    Name = m.Name,
                    ShortName = m.ShortName,
                    Text = m.Text,
                    Type = m.Type,
                    Background = m.Background,
                    Guidelines = m.Guidelines,
                    Purpose = m.Purpose,
                    Administration = m.Administration,
                    GesCriteria = m.GesCriteria,
                    GesScope = m.GesScope,
                    GesRiskIndustry = m.GesRiskIndustry,
                    ManagementSystems = m.ManagementSystems,
                    Links = m.Links,
                    G_ManagedDocuments_Id = m.G_ManagedDocuments_Id   
                };

            return query.OrderBy(d => d.Name).ToList();

        }

        public PaginatedResults<ConventionModel> GetAllConventionsForGrid(JqGridViewModel jqGridParams)
        {
            var query = GetAllConventions();

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                        break;
                    case "shortname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                        break;
                    default:
                        break;
                }
            }

            if (!jqGridParams._search) return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            var finalRules = JqGridHelper.GetFilterRules<ConventionModel>(jqGridParams);
            query = string.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public IEnumerable<I_ConventionCategories> AllConventionCategories()
        {
            var result = _dbContext.I_ConventionCategories.FromCache();
            return result;
        }
    }
}
