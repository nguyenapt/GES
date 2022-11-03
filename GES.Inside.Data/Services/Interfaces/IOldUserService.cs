using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IOldUserService: IEntityService<G_Users>
    {
        G_Users GetUserById(long userId);
        List<G_Users> GetListOldUsers();
        List<PrimaryAndForeignKeyModel> GetIndividualIdsAndOrgIdsLastLogin(List<long> oldUserIds);
    }
}
