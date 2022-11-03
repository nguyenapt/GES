using System;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesUserService : IEntityService<GesUser>
    {
        GesUser GetById(string id);
        GesUser GetByOldUserId(long id);

        PaginatedResults<GesAccountViewModel> GetGesUsers(JqGridViewModel jqGridParams);

        List<GesAccountViewModel> GetAllGesUsers();
        List<long> GetOldUserIdLastLogin(double hours);
        GesUser GetByUserName(string userName);
    }
}
