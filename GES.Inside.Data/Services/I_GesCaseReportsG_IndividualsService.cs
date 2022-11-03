using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_GesCaseReportsG_IndividualsService : EntityService<GesEntities, I_GesCaseReportsG_Individuals>, II_GesCaseReportsG_IndividualsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCaseReportsG_IndividualsRepository _gesCaseReportsG_IndividualsReporsitory;
        private readonly IUnitOfWork<GesEntities> _unitOfWork;

        public I_GesCaseReportsG_IndividualsService(IUnitOfWork<GesEntities> unitOfWork,
            II_GesCaseReportsG_IndividualsRepository gesCaseReportsG_IndividualsReporsitory, IGesLogger logger) : base(unitOfWork, logger, gesCaseReportsG_IndividualsReporsitory)
        {
            _dbContext = (GesEntities)unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _gesCaseReportsG_IndividualsReporsitory = gesCaseReportsG_IndividualsReporsitory;
        }

        public bool RemoveGesCaseReportsG_IndividualsService(long gesCaseReportId, long individualId)
        {
            this.SafeExecute(() => this._gesCaseReportsG_IndividualsReporsitory.BatchDelete(d => d.I_GesCaseReports_Id == gesCaseReportId && d.G_Individuals_Id == individualId));

            return true;
        }

        public bool AddListGescaseReportToFocusList(List<long> gesCaseReportIds, long individualId)
        {
            RemoveListGesCaseReportsG_IndividualsService(gesCaseReportIds, individualId);

            var listAdd = gesCaseReportIds.Select(d => new I_GesCaseReportsG_Individuals { I_GesCaseReports_Id = d, G_Individuals_Id = individualId }).ToList();

            this.SafeExecute(() => _gesCaseReportsG_IndividualsReporsitory.AddBatch(listAdd));

            return true;
        }

        public bool RemoveListGesCaseReportsG_IndividualsService(List<long> gesCaseReportIds, long individualId)
        {
            this.SafeExecute(() => this._gesCaseReportsG_IndividualsReporsitory.BatchDelete(d => gesCaseReportIds.Contains(d.I_GesCaseReports_Id) && d.G_Individuals_Id == individualId));
            
            return true;
        }

    }
}