using GES.Inside.Data.DataContexts;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCompanyProfileRepository : IGenericRepository<GesCompanyProfiles>
    {
        IEnumerable<GesCompanyProfiles> GetCompaniesByIsins(IEnumerable<string> isins);

        IEnumerable<GesCompanyProfiles> GetCompaniesProfileNotInIsins(IEnumerable<string> isins);
    }
}
