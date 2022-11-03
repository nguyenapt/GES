using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Exceptions;

namespace GES.Inside.Data.Services
{
    public class I_PortfoliosI_CompaniesService : EntityService<GesEntities, I_PortfoliosI_Companies>, II_PortfoliosI_CompaniesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfoliosI_CompaniesRepository _portfolioCompaniesRepository;
        private IUnitOfWork<GesEntities> _unitOfWork;

        public I_PortfoliosI_CompaniesService(IUnitOfWork<GesEntities> unitOfWork, II_PortfoliosI_CompaniesRepository portfolioCompaniesRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioCompaniesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _portfolioCompaniesRepository = portfolioCompaniesRepository;
        }

        public IEnumerable<I_PortfoliosI_Companies> GetPortfolioCompaniesByPortfolioId(long portfolioId)
        {
            return this.SafeExecute(() => _portfolioCompaniesRepository.FindBy(d => d.I_Portfolios_Id == portfolioId));
        }

        /// <summary>
        /// Clear portfolio_Companies by portfolioId
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        public bool RemovePortfolioCompaniesByPortfolioId(long portfolioId)
        {
            this.SafeExecute(() => this._portfolioCompaniesRepository.BatchDelete(d => d.I_Portfolios_Id == portfolioId));

            return true;
        }

        public void AddBatch(List<I_PortfoliosI_Companies> entities)
        {
            Guard.AgainstNullArgument(nameof(entities), entities);

            this.SafeExecute(() => _portfolioCompaniesRepository.AddBatch(entities));
        }

        /// <summary>
        /// Add list portfolioCompanies to database
        /// </summary>
        /// <param name="listPortfoliosCompanieses"></param>
        /// <returns></returns>
        public int AddPortfolioCompaniesByList(List<I_PortfoliosI_Companies> listPortfoliosCompanieses)
        {
            if (listPortfoliosCompanieses.Any())
            {
                this.AddBatch(listPortfoliosCompanieses);

                this.UnitOfWork.Commit();

                return listPortfoliosCompanieses.Count;
            }
            return 0;
        }

        public bool RemovePortfolioCompaniesByPortfolioIdAndCompanyID(long portfolioId, long companyId)
        {
            this.SafeExecute(() => this._portfolioCompaniesRepository.BatchDelete(d => d.I_Portfolios_Id == portfolioId && d.I_Companies_Id == companyId));

            return true;
        }
    }
}
