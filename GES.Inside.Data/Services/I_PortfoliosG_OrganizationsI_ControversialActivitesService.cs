using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_PortfoliosG_OrganizationsI_ControversialActivitesService : EntityService<GesEntities, I_PortfoliosG_OrganizationsI_ControversialActivites>, II_PortfoliosG_OrganizationsI_ControversialActivitesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfoliosG_OrganizationsI_ControversialActivitesRepository _portfolioOrganizationControversialRepository;

        public I_PortfoliosG_OrganizationsI_ControversialActivitesService(IUnitOfWork<GesEntities> unitOfWork,
            II_PortfoliosG_OrganizationsI_ControversialActivitesRepository portfolioOrganizationControversialRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioOrganizationControversialRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _portfolioOrganizationControversialRepository = portfolioOrganizationControversialRepository;
        }

        public IEnumerable<I_PortfoliosG_OrganizationsI_ControversialActivites> GetByPortfolioOrgId(long portfolioOrgId)
        {
            return this.SafeExecute(() => _portfolioOrganizationControversialRepository.FindBy(x => x.I_PortfoliosG_Organizations_Id == portfolioOrgId));
        }

        /// <summary>
        /// Clear PortfolioCompaniesImport by portfolioId
        /// </summary>
        /// <param name="portfolioOrgId"></param>
        /// <returns></returns>
        public bool RemovePortfolioOrgControversialByPortfolioOrgId(long portfolioOrgId)
        {
            var deletePendingCompanies = this.SafeExecute(() => _portfolioOrganizationControversialRepository.FindBy(d => d.I_PortfoliosG_Organizations_Id == portfolioOrgId).ToList());

            foreach (var deleteItem in deletePendingCompanies)
            {
                this.Delete(deleteItem);
            }

            return this.UnitOfWork.Commit() > 0 || deletePendingCompanies.Count == 0;
        }

    }
}
