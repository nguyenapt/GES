using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Exceptions;

namespace GES.Inside.Data.Services
{
    public class I_PortfolioCompaniesImportService : EntityService<GesEntities, I_PortfolioCompaniesImport>, II_PortfolioCompaniesImportService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfolioCompaniesImportRepository _portfolioCompaniesImportRepository;
        private IUnitOfWork<GesEntities> _unitOfWork;

        public I_PortfolioCompaniesImportService(IUnitOfWork<GesEntities> unitOfWork,
            II_PortfolioCompaniesImportRepository portfolioCompaniesImportRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioCompaniesImportRepository)
        {
            _dbContext = (GesEntities)unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _portfolioCompaniesImportRepository = portfolioCompaniesImportRepository;
        }

        public IEnumerable<I_PortfolioCompaniesImport> GetPortfolioCompaniesImportByPortfolioId(long portfolioId)
        {
            return this.SafeExecute(() => _portfolioCompaniesImportRepository.FindBy(d => d.I_Portfolios_Id == portfolioId));
        }

        public void AddBatch(List<I_PortfolioCompaniesImport> entities)
        {
            Guard.AgainstNullArgument(nameof(entities), entities);
            
            this.SafeExecute(() => _portfolioCompaniesImportRepository.AddBatch(entities));
        }

        /// <summary>
        /// Remove PortfolioCompaniesImport by list Id 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int RemovePortfolioCompaniesImportByListId(List<long> ids)
        {
            this.SafeExecute(() =>
            {
                foreach (var deleteItem in _portfolioCompaniesImportRepository.FindBy(d => ids.Contains(d.I_PortfolioCompaniesImport_Id)).ToList())
                {
                    Delete(deleteItem);
                }
            });

            return this.UnitOfWork.Commit();
        }

        /// <summary>
        /// Clear PortfolioCompaniesImport by portfolioId
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        public void RemovePortfolioCompaniesImportByPortfolioId(long portfolioId)
        {
            this.SafeExecute(() => this._portfolioCompaniesImportRepository.BatchDelete(d => d.I_Portfolios_Id == portfolioId));
        }
        public List<I_PortfolioCompaniesImport> GetPortfolioCompaniesImportByListId(List<long> ids)
        {
            return this.SafeExecute(() => _portfolioCompaniesImportRepository.FindBy(d => ids.Contains(d.I_PortfolioCompaniesImport_Id)).ToList());
        }
    }
}
