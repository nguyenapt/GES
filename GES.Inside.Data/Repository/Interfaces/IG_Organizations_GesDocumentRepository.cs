using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_Organizations_GesDocumentRepository : IGenericRepository<G_Organizations_GesDocument>
    {
        void AddBatch(List<G_Organizations_GesDocument> entities);
    }
}
