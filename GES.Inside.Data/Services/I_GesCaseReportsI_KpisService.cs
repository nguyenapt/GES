using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class  I_GesCaseReportsI_KpisService : EntityService<GesEntities, I_GesCaseReportsI_Kpis>,   II_GesCaseReportsI_KpisService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCaseReportsKpisRepository _gesCaseReportsKpisRepository;

        public I_GesCaseReportsI_KpisService(IUnitOfWork<GesEntities> unitOfWork, II_GesCaseReportsKpisRepository gesCaseReportsKpisRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesCaseReportsKpisRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCaseReportsKpisRepository = gesCaseReportsKpisRepository;
        }


        public I_GesCaseReportsI_Kpis GetById(long id)
        {
            return _gesCaseReportsKpisRepository.GetById(id);
        }

        public IEnumerable<I_GesCaseReportsI_Kpis> GetGesCaseReportsKpisByCaseReportID(long gesCaseReportsId)
        {
            var gesCaseReportsKpis = from t in _dbContext.I_GesCaseReportsI_Kpis
                       where t.I_GesCaseReports_Id == gesCaseReportsId
                select t;

            return gesCaseReportsKpis;
        }
    }
}
