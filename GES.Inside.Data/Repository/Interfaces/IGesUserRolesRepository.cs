using System.Collections.Generic;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUserRolesRepository : IGenericRepository<GesUserRole>
    {
        List<GesUserRole> GetByUserId(string userid);
        List<GesUserRole> GetByRoleId(string roleId);

        GesUserRole GetByUserAndRoleId(string userId, string roleId);

    }
}
