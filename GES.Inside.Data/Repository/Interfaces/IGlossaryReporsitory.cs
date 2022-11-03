using System;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGlossaryReporsitory : IGenericRepository<Glossary>
    {
        Glossary FindById(Guid glossaryId);

        IEnumerable<Glossary> GetByCategoryId(Guid CategoryId);

        void DeleteRange(Guid[] glossaryIds);

        bool CheckSlugExist(string slug, Guid id);

        Glossary GetBySlug(string slug);
    }
}