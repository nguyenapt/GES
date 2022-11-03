using GES.Inside.Data.Models.Auth;
using System.Collections;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUserRepository : IGenericRepository<GesUser>
    {
        IEnumerable<GesAccountViewModel> GetAllAccounts();

        IEnumerable<GesAccountViewModel> GetAllAccountsWithLockedInformation();
    }
}
