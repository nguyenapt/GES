using System.Data.Entity;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.DataContexts;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GesCompanyProfileRepository : GenericRepository<GesCompanyProfiles>, IGesCompanyProfileRepository
    {
        public GesCompanyProfileRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
        }

        public IEnumerable<GesCompanyProfiles> GetCompaniesByIsins(IEnumerable<string> isins)
        {
            return _dbset.Where(c => isins.Contains(c.Isin));
        }

        public IEnumerable<GesCompanyProfiles> GetCompaniesProfileNotInIsins(IEnumerable<string> isins)
        {
            return _dbset.Where(c => !isins.Contains(c.Isin));
        }
    }
}