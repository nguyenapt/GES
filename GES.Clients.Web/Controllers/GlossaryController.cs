using System;
using System.Linq;
using System.Web.Mvc;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Exceptions;
using GES.Common.Logging;

namespace GES.Clients.Web.Controllers
{
    public class GlossaryController : GesControllerBase
    {
        private readonly IGlossaryService _glossaryService;

        public GlossaryController(IGesLogger logger, IGlossaryService glossaryService)
            : base(logger)
        {
            _glossaryService = glossaryService;
        }

        public ActionResult Index()
        {
            try
            {
                var glossaries = _glossaryService.GetGlossariesByCategory(new Guid());
                var glossaryViewModels = glossaries.Select(category => _glossaryService.ToViewModel(category, true))
                                                   .ToList();

                return View(glossaryViewModels);
            }
            catch(GesServiceException  ex)
            {
                Logger.Error(ex, "Error when load glossaries by category");

                throw;
            }
        }
    }
}