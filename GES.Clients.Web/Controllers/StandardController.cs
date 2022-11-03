using System.Web.Mvc;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Clients.Web.Extensions;

namespace GES.Clients.Web.Controllers
{
    public class StandardController : GesControllerBase
    {
        #region Declaration
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_GesCaseProfilesService _gesCaseProfilesService;
        #endregion

        #region Constructor
        public StandardController(IGesLogger logger, IG_IndividualsService gIndividualsService, II_GesCaseProfilesService gesCaseProfilesService)
            : base(logger)
        {
            _gIndividualsService = gIndividualsService;
            _gesCaseProfilesService = gesCaseProfilesService;
        }

        #endregion

        // GET: Standard
        public ActionResult Details(string id)
        {
            var caseReport = new CaseReportViewModel();
            long caseReportId;

            if (long.TryParse(id, out caseReportId))
            {
                long orgId = this.GetOrganizationId(_gIndividualsService);

                caseReport = this.SafeExecute(() => _gesCaseProfilesService.GetCaseReportViewModel(caseReportId, orgId) ?? new CaseReportViewModel(), $"Exception when getting the case report view model with criteria caseReport{caseReport}.");
            }

            return View(caseReport);
        }
    }
}