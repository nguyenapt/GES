using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGlossaryService : IEntityService<Glossary>
    {
        CreateUpdateGlossaryResult CreateUpdateGlossary(Glossary glossary);

        List<Glossary> GetGlossariesByCategory(Guid categoryId);        

        IEnumerable<Glossary> GetGlossariesByIds(Guid[] glossaryIds);

        void UpdateRange(IEnumerable<Glossary> glossaries);

        bool CheckCanDelete(Guid[] glossaryIds);

        void DeleteRange(Guid[] glossaryIds);

        GlossaryViewModel ToViewModel(Glossary glossary, bool isIncludeRef);

        Glossary GetBySlug(string slug);
    }
}