using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_GesCompanyWatcherService : EntityService<GesEntities, I_GesCompanyWatcher>, II_GesCompanyWatcherService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCompanyWatcherRepository _gesCompanyWatcherReporsitory;

        public I_GesCompanyWatcherService(IUnitOfWork<GesEntities> unitOfWork, II_GesCompanyWatcherRepository gesCompanyWatcherReporsitory, IGesLogger logger)
            : base(unitOfWork, logger, gesCompanyWatcherReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCompanyWatcherReporsitory = gesCompanyWatcherReporsitory;
        }

        public bool RemoveGesCompanyWatcher(long gesCompanyId, long individualId)
        {
            this.SafeExecute(() => this._gesCompanyWatcherReporsitory.BatchDelete(d => d.I_GesCompanies_Id == gesCompanyId && d.G_Individuals_Id == individualId));

            return true;
        }

        public bool AddListCompanyToFocusList(List<long> gesCompanyIds, long individualId)
        {
            RemoveListGesCompanyWatcher(gesCompanyIds, individualId);

            var listAdd = gesCompanyIds.Select(d => new I_GesCompanyWatcher { I_GesCompanies_Id = d, G_Individuals_Id = individualId }).ToList();
            this.SafeExecute(() => _gesCompanyWatcherReporsitory.AddBatch(listAdd));

            return true;
        }

        public bool RemoveListGesCompanyWatcher(List<long> gesCompanyIds, long individualId)
        {
            this.SafeExecute(() => this._gesCompanyWatcherReporsitory.BatchDelete(d => gesCompanyIds.Contains(d.I_GesCompanies_Id) && d.G_Individuals_Id == individualId));

            return true;
        }
    }
}