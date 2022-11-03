using System;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesUserClaimsService : IEntityService<GesUserClaim>
    {
        List<GesUserClaim> GetByUserId(string userId);
    }
}
