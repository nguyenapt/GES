using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.DataContexts;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GlossaryRepository : GenericRepository<Glossary>, IGlossaryReporsitory
    {
        public GlossaryRepository(GesRefreshDbContext dbContext, IGesLogger logger)
           : base(dbContext, logger)
        {
        }
        
        public Glossary FindById(Guid glossaryId)
        {
            return this.SafeExecute<Glossary>(() => _entities.Set<Glossary>().Find(glossaryId));
        }

        public IEnumerable<Glossary> GetByCategoryId(Guid categoryId)
        {
            return this.SafeExecute<IEnumerable<Glossary>>(() => _entities.Set<Glossary>().Where(i => i.CategoryId == categoryId));
        }

        public void DeleteRange(Guid[] glossaryIds)
        {
            var glossaries = _entities.Set<Glossary>().Where(i => glossaryIds.Contains(i.Id));
            this.SafeExecute(() => _entities.Set<Glossary>().RemoveRange(glossaries));
        }

        public bool CheckSlugExist(string slug, Guid id)
        {
            return this.SafeExecute<bool>(() => _entities.Set<Glossary>().Any(i => i.Slug.ToLower() == slug.ToLower() && i.Id != id));
        }

        public Glossary GetBySlug(string slug)
        {
            return this.SafeExecute<Glossary>(() => _entities.Set<Glossary>().FirstOrDefault(i => i.Slug.ToLower() == slug.ToLower()));
        }
    }
}