using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Web.Controllers
{
    public class DashboardController : GesControllerBase
    {
        private IDashboardService _dashboardService;
        private readonly IGesUserService _gesUserService;

        public DashboardController(IGesLogger logger, IDashboardService dashboardService, IGesUserService gesUserService) : base(logger)
        {
            _dashboardService = dashboardService;
            _gesUserService = gesUserService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetUserCasesJqGrid(JqGridViewModel jqGridParams)
        {

            var userId = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId??0;

            var listCases = this.SafeExecute(() => _dashboardService.GetUserCasesForGrid(jqGridParams, userId),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listCases);
        }

    }
}