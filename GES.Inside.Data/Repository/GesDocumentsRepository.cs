using System;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models;
using System.Collections.Generic;
using GES.Inside.Data.Configs;

namespace GES.Inside.Data.Repository
{
    public class GesDocumentsRepository : GenericRepository<GesDocument>, IGesDocumentsRepository
    {
        private readonly GesEntities _dbContext;

        public GesDocumentsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            this._dbContext = context;
        }

        public GesDocument GetById(Guid id)
        {
            return this.SafeExecute<GesDocument>(() => _entities.Set<GesDocument>().FirstOrDefault(d => d.DocumentId == id));
        }

        public void DeleteRange(Guid[] documentIds)
        {
            var documents = _entities.Set<GesDocument>().Where(i => documentIds.Contains(i.DocumentId));
            _entities.Set<GesDocument>().RemoveRange(documents);
        }

        public IEnumerable<GesDocumentViewModel> GetDocumentViewModel()
        {
            return from d in _dbset
                   join ds in _dbContext.GesDocumentService on d.GesDocumentServiceId equals
                       ds.GesDocumentServiceId into gds
                   from ds in gds.DefaultIfEmpty()
                   where !Settings.HiddenDocumentServices.Contains(ds.Name) && ds.GesDocumentServiceId == 3
                   select new GesDocumentViewModel
                   {
                       Id = d.DocumentId,
                       Name = d.Name,
                       FileName = d.FileName,
                       ServiceName = ds.Name,
                       Comment = d.Comment,
                       Created = d.Created
                   };
        }
    }
}
