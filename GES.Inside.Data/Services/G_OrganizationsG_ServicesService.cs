using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using GES.Common.Configurations;
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
    public class G_OrganizationsG_ServicesService : EntityService<GesEntities, G_OrganizationsG_Services>, IG_OrganizationsG_ServicesService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_OrganizationsG_ServicesRepository _gOrganizationsGServicesRepository;

        public G_OrganizationsG_ServicesService(IUnitOfWork<GesEntities> unitOfWork, IG_OrganizationsG_ServicesRepository gOrganizationsGServicesRepository, IGesLogger logger)
            : base(unitOfWork, logger, gOrganizationsGServicesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gOrganizationsGServicesRepository = gOrganizationsGServicesRepository;
        }


        public G_OrganizationsG_Services GetById(long id)
        {
            return _gOrganizationsGServicesRepository.GetById(id);
        }

        public IEnumerable<OrganizationsServicesViewModel> GetOrganizationsServices(long orgId)
        {

            var organizationsServices = _gOrganizationsGServicesRepository.GetOrganizationServicesViewModel(orgId);

            return organizationsServices;

        }
    }
}
