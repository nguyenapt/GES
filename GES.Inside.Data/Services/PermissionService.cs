using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class PermissionService : EntityService<GesEntities, G_OrganizationsG_Services>, IPermissionService
    {
        private readonly IG_OrganizationsG_ServicesRepository _organizationServicesRepository;
        private GesEntities _dbContext;

        public PermissionService(IUnitOfWork<GesEntities> unitOfWork, IG_OrganizationsG_ServicesRepository organizationServicesRepository, IGesLogger logger)
            : base(unitOfWork, logger, organizationServicesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _organizationServicesRepository = organizationServicesRepository;
        }

        public List<G_OrganizationsG_Services> GetOrganizationsG_ServicesByOrgId(long orgId)
        {
            var query = this._organizationServicesRepository.GetOrganizationServices(orgId);

            return this.SafeExecute(query.ToList);
        }

    }
}
