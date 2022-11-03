using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesRolesService : IEntityService<GesRole>
    {
        GesRole GetById(string id);

        PaginatedResults<GesRole> GetGesRoles(JqGridViewModel jqGridParams);

        IEnumerable<GesRole> GetGesRoles();

        IList<KeyValueObject<string, string>> GetRolesInString();

        string[] RolesToStringArray(ICollection<GesUserRole> roles);

        PaginatedResults<RoleModel> GetAllRolesForGrid(JqGridViewModel jqGridParams);
        List<RolePermissionModel> GetRolePermissions(string roleId);

        List<GesAccountRoleViewModel> GetUsersInRole(string roleId);

        List<GesAccountRoleViewModel> GetUsersNotInRole(string roleId);
    }
}
