using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_GesCaseReportsExtraService : EntityService<GesEntities, I_GesCaseReportsExtra>, II_GesCaseReportsExtraService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCaseReportsExtraRepository _gesCaseReportsExtraRepository;

        public I_GesCaseReportsExtraService(IUnitOfWork<GesEntities> unitOfWork,
            II_GesCaseReportsExtraRepository gesCaseReportsExtraRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesCaseReportsExtraRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCaseReportsExtraRepository = gesCaseReportsExtraRepository;
        }

        public int BatchUpdateCaseReportKeywords(IEnumerable<I_GesCaseReportsExtra> list)
        {
            var allExisting = GetAll().ToList();
            var toAddList = new List<I_GesCaseReportsExtra>();
            foreach (var item in list)
            {
                var existing = allExisting.Find(i => i.I_GesCaseReports_Id == item.I_GesCaseReports_Id);
                if (existing != null)
                {
                    existing.Keywords = item.Keywords;
                    
                    //toUpdateList.Add(existing);
                    Update(existing);
                }
                else
                {
                    toAddList.Add(item);
                }
            }

            // saving updates
            this.SafeExecute(this.UnitOfWork.Commit);

            // batch insert
            this.SafeExecute(() => _gesCaseReportsExtraRepository.AddBatch(toAddList));

            return toAddList.Count;
        }
    }
}
