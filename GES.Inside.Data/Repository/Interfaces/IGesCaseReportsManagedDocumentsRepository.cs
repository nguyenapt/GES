using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCaseReportsManagedDocumentsRepository : IGenericRepository<I_GesCaseReportsG_ManagedDocuments>
    {
        I_GesCaseReportsG_ManagedDocuments GetGesCaseReportsManagedDocuments(long documentIds);
    }
}
