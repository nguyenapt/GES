using System.Collections.Generic;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesRolePermissionRepository : IGenericRepository<GesRolePermission>
    {
        GesRolePermission GetById(long id);
        List<GesRolePermission> GetPermissionsByRoleId(string roleId);
    }
}
