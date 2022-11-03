using System.Collections.Generic;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUserClaimsRepository : IGenericRepository<GesUserClaim>
    {
        List<GesUserClaim> GetByUserId(string userId);
    }
}
