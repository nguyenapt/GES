using System.Linq;
using System.Web.Mvc;
using GES.Clients.Web.Extensions;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Common.Resources;
using GES.Inside.Data.Models;

namespace GES.Clients.Web.Controllers
{
    public class AlertController : GesControllerBase
    {
        #region Declaration
        private readonly IAlertService _alertService;
        private readonly IG_IndividualsService _gIndividualsService;
        #endregion

        #region Constructor
        public AlertController(IGesLogger logger, IAlertService alertService, IG_IndividualsService gIndividualsService) : base(logger)
        {
            _alertService = alertService;
            _gIndividualsService = gIndividualsService;

        }
        #endregion

        #region JsonResult

        public ActionResult AlertDetails(string id)
        {
            long alertId;

            if (!long.TryParse(id, out alertId))
            {
                return View(new AlertListViewModel());
            }

            long orgId = 0, individualId = 0;

            SafeExecute(() => this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId),
                "Exception when getting the individual/organization id");
            return SafeExecute(() =>
            {
                var alertDetails = _alertService.GetAlertById(alertId);

                if (alertDetails != null)
                {
                    if (alertDetails.IsExtended || (alertDetails.NaTypeId != null &&
                                                    alertDetails.NaTypeId.Value == (long) NaType.AlertIncidentWatch))
                    {
                        alertDetails.Notices = Resources.AlertWatchNotices;
                        alertDetails.AlertType = "Alert incident watch";
                    }
                    else
                    {
                        alertDetails.AlertType = "Alert";
                    }
                    ViewBag.Title = alertDetails.AlertType  + " - " + alertDetails.Heading ;
                }
                
                return View(alertDetails);

            }, $"Exception when getting the case report data of case ({alertId}).");

        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public JsonResult GetAlerts(JqGridViewModel jqGridParams, long companyId)
        {
            var alerts = this.SafeExecute(() => _alertService.GetAlertsByCompanyId(companyId), $"Error when get alerts of companyid {companyId}.");

            foreach (var alert in alerts)
            {
                if (alert.IsExtended || (alert.NaTypeId != null &&
                                         alert.NaTypeId.Value == (long) NaType.AlertIncidentWatch))
                {
                    alert.Notices = Resources.AlertWatchNotices;
                    alert.AlertType = "Alert incident watch";
                }
                else
                {
                    alert.AlertType = "Alert";
                }
            }

            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "heading":
                        alerts = sortDir == "asc"
                            ? alerts.OrderBy(x => x.Heading).ToList()
                            : alerts.OrderByDescending(x => x.Heading).ToList();
                        break;
                    case "date":
                        alerts = sortDir == "asc"
                            ? alerts.OrderBy(x => x.Date).ThenBy(d => d.Heading).ToList()
                            : alerts.OrderByDescending(x => x.Date).ThenByDescending(d => d.Heading).ToList();
                        break;
                    case "location":
                        alerts = sortDir == "asc"
                            ? alerts.OrderBy(x => x.Location).ThenBy(d => d.Date).ToList()
                            : alerts.OrderByDescending(x => x.Location).ThenByDescending(d => d.Date).ToList();
                        break;
                    case "norm":
                        alerts = sortDir == "asc"
                            ? alerts.OrderBy(x => x.Norm).ThenBy(d => d.Date).ToList()
                            : alerts.OrderByDescending(x => x.Norm).ThenByDescending(d => d.Date).ToList();
                        break;
                    case "summary":
                        alerts = sortDir == "asc"
                            ? alerts.OrderBy(x => x.Summary).ToList()
                            : alerts.OrderByDescending(x => x.Summary).ToList();
                        break;
                }
            }

            return Json(alerts.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}