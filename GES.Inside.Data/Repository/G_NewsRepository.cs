using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class G_NewsRepository : GenericRepository<G_News>, IG_NewsRepository
    {
        public G_NewsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public G_News GetById(long id)
        {
            return this.SafeExecute<G_News>(() => _entities.Set<G_News>().FirstOrDefault(d => d.G_News_Id == id));
        }
    }
}
