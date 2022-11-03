using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using GES.Common.Enumeration;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class G_OrganizationsG_ServicesRepository : GenericRepository<G_OrganizationsG_Services>, IG_OrganizationsG_ServicesRepository
    {
        private readonly GesEntities _dbContext;

        public G_OrganizationsG_ServicesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public G_OrganizationsG_Services GetById(long id)
        {
            return this.SafeExecute<G_OrganizationsG_Services>(()=> _entities.Set<G_OrganizationsG_Services>().FirstOrDefault(d => d.G_OrganizationsG_Services_Id == id));
        }

        public IEnumerable<G_OrganizationsG_Services> GetOrganizationServices(long organizationId)
        {
            return from os in _dbContext.G_OrganizationsG_Services
                   where os.G_Organizations_Id == organizationId
                   select os;
        }

        public IQueryable<OrganizationsServicesViewModel> GetOrganizationServicesViewModel(long orgId)
        {
            var organizationsServices = from os in _dbContext.G_OrganizationsG_Services
                join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
                where os.G_Organizations_Id == orgId
                select new OrganizationsServicesViewModel
                {
                    ServicesId = s.G_Services_Id,
                    ServicesName = s.Name,
                    Price = os.Price,
                    Reporting = os.Reporting,
                    Comment = os.Comment,
                    Modified = os.Modified,
                    Created = os.Created,
                    ModifiedByUsersId = os.ModifiedByG_Users_Id,
                    OrganizationsServicesId = os.G_OrganizationsG_Services_Id,
                    OrganizationsId = os.G_Organizations_Id,
                    ManagedDocumentsId = os.G_ManagedDocuments_Id,
                    DemoEnd = os.DemoEnd,
                    ServiceStatesId = os.G_ServiceStates_Id,
                    TermsAccepted = os.TermsAccepted,
                    TermsAcceptedByIp = os.TermsAcceptedByIp,
                    SuperFilter = os.W_SuperFilter
                };

            return organizationsServices;
        }

        public IEnumerable<long> GetSubscribedServicesOfIndividual(long individualId)
        {
            return
            from i in _dbContext.G_Individuals
            join o in _dbContext.G_Organizations on i.G_Organizations_Id equals o.G_Organizations_Id
            join os in _dbContext.G_OrganizationsG_Services on o.G_Organizations_Id equals os.G_Organizations_Id
            join s in _dbContext.G_Services on os.G_Services_Id equals s.G_Services_Id
            join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into get from et in get.DefaultIfEmpty()

            where i.G_Individuals_Id == individualId && (et.I_EngagementTypeCategories_Id == (long)EngagementTypeCategoryEnum.BusinessConduct || et.I_EngagementTypeCategories_Id == (long)EngagementTypeCategoryEnum.StewardshipAndRisk || et.I_EngagementTypeCategories_Id == (long)EngagementTypeCategoryEnum.Bespoke || s.G_Services_Id == (long)GesService.GesGlobalEthicalStandard) 
            select os.G_Services_Id;
        }
    }
}
