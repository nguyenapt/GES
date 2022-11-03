using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportSourcesRepository : GenericRepository<I_GesCaseReportSources>, II_GesCaseReportSourcesRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCaseReportSourcesRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }
        public IList<CaseReportSourceReferenceViewModel> GetSourcesByCaseReportId(long caseReportId)
        {
            return this.SafeExecute(() => (from s in _dbContext.I_GesCaseReportSources
                                           join dc in _dbContext.G_ManagedDocuments
                                           on s.G_ManagedDocuments_Id equals dc.G_ManagedDocuments_Id into sg
                                           from dc in sg.DefaultIfEmpty()
                                           where s.I_GesCaseReports_Id == caseReportId
                                           select new CaseReportSourceReferenceViewModel
                                           {
                                               Id = s.I_GesCaseReportSources_Id,
                                               ManagedDocumentId = s.G_ManagedDocuments_Id,
                                               Accessed = s.Accessed,
                                               AvailableFrom = s.AvailableFrom,
                                               MainSource = s.MainSource,
                                               PublicationYear = s.PublicationYear,
                                               Source = s.Source,
                                               Name = dc.Name,
                                               ShowInReport = s.ShowInReport,
                                               CaseReportId = s.I_GesCaseReports_Id,
                                               Status = s.I_GesCaseReportAvailabilityStatuses_Id ?? 0
                                           }).ToList());
        }

        public I_GesCaseReportSources GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportSources>(() => _entities.Set<I_GesCaseReportSources>().Find(id));
        }
    }
}
