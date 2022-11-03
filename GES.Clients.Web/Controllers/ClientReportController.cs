using System;
using System.Linq;
using System.Web.Mvc;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Exceptions;
using GES.Common.Logging;

namespace GES.Clients.Web.Controllers
{
    public class ClientReportController : GesControllerBase
    {
        private readonly IGlossaryService _glossaryService;

        public ClientReportController(IGesLogger logger, IGlossaryService glossaryService)
            : base(logger)
        {
            _glossaryService = glossaryService;
        }

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch(GesServiceException  ex)
            {
                Logger.Error(ex, "Error when load glossaries by category");

                throw;
            }
        }
    }
}