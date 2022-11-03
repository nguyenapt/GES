using System.Collections.Generic;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Inside.Data.Services
{
    public class I_PortfoliosG_OrganizationService : EntityService<GesEntities, I_PortfoliosG_Organizations>, II_PortfoliosG_OrganizationService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfoliosG_OrganizationRepository _portfoliosGOrganizationRepository;
        private IUnitOfWork<GesEntities> _unitOfWork;

        public I_PortfoliosG_OrganizationService(IUnitOfWork<GesEntities> unitOfWork,
            II_PortfoliosG_OrganizationRepository portfoliosGOrganizationRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfoliosGOrganizationRepository)
        {
            _dbContext = (GesEntities)unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _portfoliosGOrganizationRepository = portfoliosGOrganizationRepository;
        }

        public I_PortfoliosG_Organizations GetById(long id)
        {
            return this.SafeExecute(() => _portfoliosGOrganizationRepository.GetById(id));
        }

        public List<I_PortfoliosG_Organizations> GetByPortfolioAndOrganizationIds(long portfolioId, long organizationId)
        {
            return this.SafeExecute(() => _portfoliosGOrganizationRepository.GetByPortfolioIdAndOrganizationId(portfolioId, organizationId));
        }

        public long? GetOrganizationIdFromPortfolioOrganizationId(long poId)
        {
            if (poId <= 0)
            {
                return null;
            }

            var result = this.SafeExecute(() => _portfoliosGOrganizationRepository.GetById(poId));
            return result.G_Organizations_Id;
        }


        public bool DeletePortfolioOrganization(long portfolioId, long organizationId)
        {
            var deleteItems = GetByPortfolioAndOrganizationIds(portfolioId, organizationId);

            if (deleteItems.Count > 0)
            {
                foreach (var item in deleteItems)
                {
                    Delete(item);
                }
            }

            return this.UnitOfWork.Commit() > 0;
        }

        public bool CheckExistPortfolioInPortfolioOrganization(long portfolioId)
        {
            return this.SafeExecute(() => _portfoliosGOrganizationRepository.FindBy(d => d.I_Portfolios_Id == portfolioId).Any());
        }

    }
}
