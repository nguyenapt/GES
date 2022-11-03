using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Services
{
    public class GesCaseReports_Audit_DetailService : EntityService<GesEntities, GesCaseReports_Audit_Details>, IGesCaseReports_Audit_DetailService
    {
        private readonly GesEntities _dbContext;
        private readonly IGesCaseReports_Audit_DetailRepository _gesCompanyDialogueRepository;

        public GesCaseReports_Audit_DetailService(IUnitOfWork<GesEntities> unitOfWork, IGesCaseReports_Audit_DetailRepository gesCompanyDialogueRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesCompanyDialogueRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCompanyDialogueRepository = gesCompanyDialogueRepository;
        }

        public GesCaseReports_Audit_Details GetById(Guid id)
        {
            return SafeExecute(() => _gesCompanyDialogueRepository.FindBy(x=>x.GesCaseReports_Audit_Details1==id).FirstOrDefault());
        }

        public List<GesCaseReports_Audit_Details> GetGesCaseProfileAuditByCaseReportId(long caseReportId, string columnName)
        {
            var result = (from a in _dbContext.GesCaseReports_Audit
                join d in _dbContext.GesCaseReports_Audit_Details on a.GesCaseReports_Audit_Id equals d
                    .GesCaseReports_Audit_GesCaseReports_Audit_Id
                where a.I_GesCaseReports_Id == caseReportId && d.TableName == "I_GesCaseReports" &&
                      d.ColumnName == columnName
                select d).ToList();

            return result;
        }
    }
}
