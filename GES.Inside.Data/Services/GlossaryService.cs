using System;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class GlossaryService : EntityService<GesRefreshDbContext, Glossary>, IGlossaryService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IGlossaryReporsitory _glossaryReporsitory;

        public GlossaryService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IGlossaryReporsitory glossaryReporsitory, IGesLogger logger) : base(unitOfWork, logger, glossaryReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _glossaryReporsitory = glossaryReporsitory;
        }

        public CreateUpdateGlossaryResult CreateUpdateGlossary(Glossary glossary)
        {            
            var isSlugAlreadyExist = this.SafeExecute<bool>(() => _glossaryReporsitory.CheckSlugExist(glossary.Slug, glossary.Id));

            if (isSlugAlreadyExist)
            {
                return CreateUpdateGlossaryResult.SlugAlreadyExist;
            }

            var oldglossary = this.SafeExecute<Glossary>(() => _glossaryReporsitory.FindById(glossary.Id));
            if (oldglossary == null)
            {
                _glossaryReporsitory.Add(glossary);                
            }
            else
            {
                oldglossary.Id = glossary.Id;
                oldglossary.Slug = glossary.Slug;
                oldglossary.Title = glossary.Title;
                oldglossary.Description= glossary.Description;
                oldglossary.Order = glossary.Order;
                oldglossary.CategoryId = glossary.CategoryId;

                _glossaryReporsitory.Edit(oldglossary);
            }            
            
            this.SafeExecute(() => _glossaryReporsitory.Save());
            return CreateUpdateGlossaryResult.Success;
        }

        public List<Glossary> GetGlossariesByCategory(Guid categoryId)
        {
            return this.SafeExecute<List<Glossary>>(() => _dbContext.Glossaries
                             .Where(i => i.CategoryId == categoryId)
                             .OrderBy(i => i.Order)
                             .ToList());
        }

        public IEnumerable<Glossary> GetGlossariesByIds(Guid[] glossaryIds)
        {
            return this.SafeExecute<IEnumerable<Glossary>>(() => glossaryIds.Select(_glossaryReporsitory.FindById));
        }

        public void UpdateRange(IEnumerable<Glossary> glossaries)
        {
            foreach (var glossary in glossaries)
            {
                _glossaryReporsitory.Edit(glossary);
            }
            
            this.SafeExecute(this.UnitOfWork.Commit);
        }

        public bool CheckCanDelete(Guid[] glossaryIds)
        {
            return glossaryIds.All(i=> CheckCanDelete(i, glossaryIds));
        }

        public void DeleteRange(Guid[] glossaryIds)
        {
            this.SafeExecute(() =>
            {
                _glossaryReporsitory.DeleteRange(glossaryIds);

                this.UnitOfWork.Commit();
            });
        }

        public GlossaryViewModel ToViewModel(Glossary glossary, bool isIncludeRef)
        {
            var glossaryViewModel = new GlossaryViewModel()
                                    {
                                        Id = glossary.Id,
                                        Slug = glossary.Slug,
                                        Title = glossary.Title,
                                        Order = glossary.Order,
                                        CategoryId = glossary.CategoryId,
                                        Description = glossary.Description,
                                    };
            if (isIncludeRef)
            {
                glossaryViewModel.Childs = GetGlossariesByCategory(glossary.Id).Select(i => ToViewModel(i, true)).ToList();
                glossaryViewModel.ChildNums = glossaryViewModel.Childs.Count();
            }

            return glossaryViewModel;
        }

        public Glossary GetBySlug(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return null;
            }
            return this.SafeExecute<Glossary>(() => _glossaryReporsitory.GetBySlug(slug));
        }

        private bool CheckCanDelete(Guid glossaryId, Guid[] glossaryIds)
        {
            var childs = this.SafeExecute<IEnumerable<Glossary>>(() => _glossaryReporsitory.GetByCategoryId(glossaryId));
            return !childs.Any() || childs.All(i=> glossaryIds.Contains(i.Id));
        }
    }
}