using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_UsersRepository : IGenericRepository<G_Users>
    {
        G_Users GetById(long id);
    }
}
