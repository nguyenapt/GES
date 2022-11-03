using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GesUserRolesRepository : GenericRepository<GesUserRole>, IGesUserRolesRepository
    {
        public GesUserRolesRepository(GesRefreshDbContext context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public List<GesUserRole> GetByUserId(string userid)
        {
            return this.SafeExecute<List<GesUserRole>>(() => _entities.Set<GesUserRole>().Where(d => d.UserId == userid).ToList());
        }

        public List<GesUserRole> GetByRoleId(string roleId)
        {
            return this.SafeExecute<List<GesUserRole>>(()=> _entities.Set<GesUserRole>().Where(d => d.RoleId == roleId).ToList());
        }

        public GesUserRole GetByUserAndRoleId(string userId, string roleId)
        {
            return this.SafeExecute<GesUserRole>(() => _entities.Set<GesUserRole>().FirstOrDefault(d => d.RoleId == roleId && d.UserId == userId));
        }

    }
}
