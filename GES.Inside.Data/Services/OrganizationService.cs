using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Services
{
    public class OrganizationService : EntityService<GesEntities, G_Organizations>, IOrganizationService
    {
        private GesEntities _dbContext;
        private IG_OrganizationsRepository _organizationRepository;

        public OrganizationService(IUnitOfWork<GesEntities> unitOfWork, IG_OrganizationsRepository organizationRepository, IGesLogger logger)
            : base(unitOfWork, logger, organizationRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _organizationRepository = organizationRepository;
        }

        public G_Organizations GetById(long id)
        {
            return this.SafeExecute(() => _organizationRepository.GetById(id));
        }

        public PaginatedResults<ClientViewModel> GetClients(JqGridViewModel jqGridParams, bool isAllOrganizations = false)
        {
            IEnumerable<ClientViewModel> query = null;
            if(isAllOrganizations)
                query = this.SafeExecute(this._organizationRepository.GetAllOrganizations);
            else
                query = this.SafeExecute(this._organizationRepository.GetAllClients);


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
                    case "status":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                        break;
                    case "industry":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Industry)
                            : query.OrderByDescending(x => x.Industry);
                        break;
                    case "progressstatus":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ProgressStatus)
                            : query.OrderByDescending(x => x.ProgressStatus);
                        break;
                    case "country":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Country)
                            : query.OrderByDescending(x => x.Country);
                        break;
                    case "created":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Created)
                            : query.OrderByDescending(x => x.Created);
                        break;
                    case "modified":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Modified)
                            : query.OrderByDescending(x => x.Modified);
                        break;
                    case "employees":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Employees)
                            : query.OrderByDescending(x => x.Employees);
                        break;
                    case "totalassets":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.TotalAssets)
                            : query.OrderByDescending(x => x.TotalAssets);
                        break;
                    default:
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<ClientViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public string[] GetClientStatuses()
        {
            return this.SafeExecute(() => _dbContext.I_ClientStatuses.Select(x => x.Name).ToArray());
        }

        public string[] GetClientProgressStatuses()
        {
            return this.SafeExecute(() => _dbContext.I_ClientProgressStatuses.Select(x => x.Name).ToArray());
        }

        public string[] GetIndustries()
        {
            return this.SafeExecute(() => _dbContext.G_Industries.Select(x => x.Name).ToArray());
        }
        
        public IEnumerable<G_Industries> GetIndustriesObject()
        {
            var result = _dbContext.G_Industries.FromCache();

            return result;
        }

        public string[] GetCountries()
        {
            return this.SafeExecute(() => _dbContext.Countries.Where(d => d.Name != null).OrderBy(d => d.Name).Select(x => x.Name).ToArray());
        }

        public long[] GetServicesAgreementByOrganizationId(long organizationId)
        {
            return this.SafeExecute(() => _dbContext.G_OrganizationsG_Services.Where(d => d.G_Organizations_Id == organizationId)
                    .Select(d => d.G_Services_Id)
                    .ToArray());
        }

        public List<ClientViewModel> GetAllClients()
        {
            var query = this._organizationRepository.GetAllClients().AsQueryable();

            return query.FromCache().OrderBy(d=>d.Name).ToList();

        }

    }
}
