using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_NaArticlesRepository : IGenericRepository<I_NaArticles>
    {
        I_NaArticles GetById(long id);
        IEnumerable<I_NaArticles> GetPublishableArticles(long companyId);
    }
}
