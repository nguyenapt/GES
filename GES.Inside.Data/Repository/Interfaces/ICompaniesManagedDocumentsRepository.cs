using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface ICompaniesManagedDocumentsRepository : IGenericRepository<I_CompaniesG_ManagedDocuments>
    {
        long GetMaxId();
        I_CompaniesG_ManagedDocuments GetCompaniesGManagedDocuments(long documentIds);
    }
}
