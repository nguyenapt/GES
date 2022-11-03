using System;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Exceptions;

namespace GES.Inside.Data.Services
{
    public class I_PortfoliosG_OrganizationG_ServicesService : EntityService<GesEntities, I_PortfoliosG_OrganizationsG_Services>, II_PortfoliosG_OrganizationG_ServicesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfoliosG_OrganizationG_ServicesRepository _portfoliosOrganizationServicesRepository;
        private IUnitOfWork<GesEntities> _unitOfWork;

        public I_PortfoliosG_OrganizationG_ServicesService(IUnitOfWork<GesEntities> unitOfWork,
            II_PortfoliosG_OrganizationG_ServicesRepository portfoliosOrganizationServicesRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfoliosOrganizationServicesRepository)
        {
            _dbContext = (GesEntities)unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _portfoliosOrganizationServicesRepository = portfoliosOrganizationServicesRepository;
        }

        public I_PortfoliosG_OrganizationsG_Services GetById(long id)
        {
            return this.SafeExecute(() => _portfoliosOrganizationServicesRepository.GetById(id));
        }

        public bool UpdatePortfolioOrganizationServices(long portfolioOrganizationId, long[] serviceIds)
        {
            //Guard.AgainstNullArgument(nameof(serviceIds), serviceIds);

            this.SafeExecute(() =>
            {
                var existingRecords = _portfoliosOrganizationServicesRepository.GetByPortfolioOrganizationId(portfolioOrganizationId);

                if (serviceIds == null)
                {
                    if(existingRecords!=null && existingRecords.Any())
                    {
                        foreach (var item in existingRecords)
                        {
                            Delete(item);
                        }
                    }
                }
                else
                {
                    var unchangedIds = existingRecords.Where(i => serviceIds.Contains(i.G_Services_Id)).Select(i => i.G_Services_Id);

                    // delete
                    var toDeleteRecords = serviceIds == null ? existingRecords : existingRecords.Where(i => !serviceIds.Contains(i.G_Services_Id));
                    if (toDeleteRecords.Any())
                    {
                        foreach (var item in toDeleteRecords)
                        {
                            Delete(item);
                        }
                    }

                    // add
                    // only if serviceIds != null
                    if (serviceIds != null)
                    {
                        foreach (var id in serviceIds.Where(id => !unchangedIds.Contains(id)))
                        {
                            Add(new I_PortfoliosG_OrganizationsG_Services()
                            {
                                I_PortfoliosG_Organizations_Id = portfolioOrganizationId,
                                G_Services_Id = id,
                                Created = DateTime.UtcNow
                            });
                        }
                    }
                }
            });

            return this.UnitOfWork.Commit() > 0;
        }
    }
}
