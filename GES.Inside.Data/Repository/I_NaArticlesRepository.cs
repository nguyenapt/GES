using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class I_NaArticlesRepository : GenericRepository<I_NaArticles>, II_NaArticlesRepository
    {
        public I_NaArticlesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_NaArticles GetById(long id)
        {
            return this.SafeExecute<I_NaArticles>(() => _entities.Set<I_NaArticles>().FirstOrDefault(d => d.I_NaArticles_Id == id));
        }

        public IEnumerable<I_NaArticles> GetPublishableArticles(long companyId)
        {
            return _dbset.Where(d => d.I_Companies_Id == companyId && d.Publishable);
        }
    }
}
