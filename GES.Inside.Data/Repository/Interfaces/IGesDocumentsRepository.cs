using System;
using GES.Inside.Data.DataContexts;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesDocumentsRepository : IGenericRepository<GesDocument>
    {
        GesDocument GetById(Guid id);

        void DeleteRange(Guid[] documentIds);

        IEnumerable<GesDocumentViewModel> GetDocumentViewModel();
    }
}
