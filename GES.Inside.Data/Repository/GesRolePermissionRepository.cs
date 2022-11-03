using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class GesRolePermissionRepository : GenericRepository<GesRolePermission>, IGesRolePermissionRepository
    { 
        public GesRolePermissionRepository(GesRefreshDbContext context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public GesRolePermission GetById(long id)
        {
            return this.SafeExecute<GesRolePermission>(() => _entities.Set<GesRolePermission>().FirstOrDefault(d => d.Id == id));
        }

        public List<GesRolePermission> GetPermissionsByRoleId(string roleId)
        {
            return this.SafeExecute<List<GesRolePermission>>(() => _entities.Set<GesRolePermission>().Where(d => d.GesRoleId == roleId).ToList());
        }

    }
}
