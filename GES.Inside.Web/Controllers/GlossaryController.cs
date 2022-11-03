using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.Ajax.Utilities;

namespace GES.Inside.Web.Controllers
{
    public class GlossaryController : Controller
    {

        private readonly IGlossaryService _glossaryService;

        public GlossaryController(IGlossaryService glossaryService)
        {
            _glossaryService = glossaryService;
        }

        [CustomAuthorize(FormKey = "ConfigGlossary", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult CreateForm_Glossary(string glossaryId)
        {
            var glossaries = _glossaryService.GetGlossariesByCategory(new Guid());
            var categories = new List<SelectListItem>
                             {
                                 new SelectListItem { Text = "None", Value = new Guid().ToString()}
                             };
            categories.AddRange(glossaries.Select(i => new SelectListItem {Text = i.Slug, Value = i.Id.ToString()}));
            ViewBag.glossaryCategory = categories;

            if (string.IsNullOrEmpty(glossaryId))
            {
                return PartialView("_CreateGlossary", new GlossaryViewModel());
            }           

            var glossary = _glossaryService.GetGlossariesByIds(new[] {new Guid(glossaryId)})
                                           .FirstOrDefault();

            if (glossary.CategoryId == new Guid())
            {
                var currentGlossarySelectItem = categories.First(i => i.Value == glossaryId);
                categories.Remove(currentGlossarySelectItem);
                ViewBag.glossaryCategory = categories;
            }            

            var glossaryViewModel = _glossaryService.ToViewModel(glossary, false);
            return PartialView("_CreateGlossary", glossaryViewModel);
        }

        [HttpPost]
        public JsonResult CreateUpdateGlossary(GlossaryViewModel glossaryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "Something wrong. Invalid model! Kindly check again."
                });
            }

            bool isEditing = true;
            if (glossaryViewModel.Id == new Guid())
            {
                glossaryViewModel.Id = Guid.NewGuid();
                isEditing = false;
            }

            var result = _glossaryService.CreateUpdateGlossary(new Glossary
                                                                 {
                                                                     Id = glossaryViewModel.Id,
                                                                     Slug = glossaryViewModel.Slug,
                                                                     CategoryId = glossaryViewModel.CategoryId,
                                                                     Description = glossaryViewModel.Description,
                                                                     Title = glossaryViewModel.Title,
                                                                     Order = glossaryViewModel.Order,
                                                                 });

            if (result == CreateUpdateGlossaryResult.SlugAlreadyExist)
            {
                return Json(new
                {
                    success = false,
                    error = "Slug already exists. Kindly choose another slug."
                });

            }

            return Json(new
            {
                success = true,
                editing = isEditing
            });
        }

        [HttpPost]
        public JsonResult GetGlossariesByCategoryId(Guid categoryId)
        {
            var glossaries = _glossaryService.GetGlossariesByCategory(categoryId);
            var glossaryViewModels = glossaries.Select(i => _glossaryService.ToViewModel(i, true));
            
            return Json(glossaryViewModels);
        }

        [HttpPost]
        public JsonResult SortGlossaries(Guid[] glossaryIds)
        {
            var glossaries = _glossaryService.GetGlossariesByIds(glossaryIds);
            var order = 0;
            glossaries.ForEach(i =>
                               {
                                   i.Order = order;
                                   order++;
                               });
            _glossaryService.UpdateRange(glossaries);
            return Json(new
                        {
                            success = true
                        });
        }

        [HttpPost]
        public JsonResult DeleteGlossaries(Guid[] glossaryIds)
        {
            var isCanDelete = _glossaryService.CheckCanDelete(glossaryIds);
            if (isCanDelete)
            {
                _glossaryService.DeleteRange(glossaryIds);
                return Json(new {success = true});
            }
            return Json(new {success = false, message = "Selected category contains terms. Kindy remove all terms first."});
        }
    }
}
