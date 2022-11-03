using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Common.Exceptions;

namespace GES.Inside.Data.Repository
{
    public class G_IndividualsRepository : GenericRepository<G_Individuals>, IG_IndividualsRepository
    {
        private GesEntities _dbContext;

        public G_IndividualsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public G_Individuals GetById(long id)
        {
            return this.SafeExecute<G_Individuals>(() => _entities.Set<G_Individuals>().FirstOrDefault(d => d.G_Individuals_Id == id));
        }

        public G_Individuals GetIndividualByUser(long userId)
        {
            return (from i in _dbContext.G_Individuals
                    join u in _dbContext.G_Users on i.G_Individuals_Id equals u.G_Individuals_Id
                    where u.G_Users_Id == userId
                    select i
            ).FirstOrDefault();
        }
    }
}
