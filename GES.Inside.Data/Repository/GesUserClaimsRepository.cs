using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GesUserClaimsRepository : GenericRepository<GesUserClaim>, IGesUserClaimsRepository
    {
        public GesUserClaimsRepository(GesRefreshDbContext context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public List<GesUserClaim> GetByUserId(string userId)
        {
            return this.SafeExecute<List<GesUserClaim>>(() => _entities.Set<GesUserClaim>().Where(d => d.UserId == userId).ToList());
        }
    }
}
