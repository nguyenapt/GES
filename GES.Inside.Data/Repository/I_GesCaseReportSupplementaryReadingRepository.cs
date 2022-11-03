using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportSupplementaryReadingRepository : GenericRepository<I_GesCaseReportSupplementaryReading>, II_GesCaseReportSupplementaryReadingRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCaseReportSupplementaryReadingRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public IList<CaseReportSupplementaryViewModel> GetSupplementaryReadingsByCaseReportId(long caseReportId)
        {
            return this.SafeExecute(() => (from s in _dbContext.I_GesCaseReportSupplementaryReading
                                           join dc in _dbContext.G_ManagedDocuments
                                           on s.G_ManagedDocuments_Id equals dc.G_ManagedDocuments_Id into sg
                                           from dc in sg.DefaultIfEmpty()
                                           where s.I_GesCaseReports_Id == caseReportId
                                           select new CaseReportSupplementaryViewModel
                                           {
                                               Id = s.I_GesCaseReportSupplementaryReading_Id,
                                               ManagedDocumentId = s.G_ManagedDocuments_Id,
                                               Accessed = s.Accessed,
                                               AvailableFrom = s.AvailableFrom,
                                               PublicationYear = s.PublicationYear,
                                               Source = s.Source,
                                               Name = dc.Name,
                                               ShowInReport = s.ShowInReport,
                                               CaseReportId = s.I_GesCaseReports_Id,
                                               Status = s.I_GesCaseReportAvailabilityStatuses_Id
                                           }).ToList());
        }

        public I_GesCaseReportSupplementaryReading GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportSupplementaryReading>(() => _entities.Set<I_GesCaseReportSupplementaryReading>().Find(id));
        }
    }
}
