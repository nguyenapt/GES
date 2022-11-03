using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_NewsRepository : IGenericRepository<G_News>
    {
        G_News GetById(long id);
    }
}
