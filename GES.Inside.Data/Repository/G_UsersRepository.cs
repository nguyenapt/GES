using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class G_UsersRepository : GenericRepository<G_Users>, IG_UsersRepository
    {
        public G_UsersRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public G_Users GetById(long id)
        {
            return this.SafeExecute<G_Users>(() => _entities.Set<G_Users>().FirstOrDefault(d => d.G_Users_Id == id));
        }

    }
}
