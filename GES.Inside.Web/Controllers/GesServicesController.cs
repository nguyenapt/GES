using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Extensions;

namespace GES.Inside.Web.Controllers
{
    public class GesServicesController : GesControllerBase
    {

        #region Declaration

        private readonly II_ServicesService _servicesService;
        private readonly IG_ServicesRepository _gServicesRepository;
        private readonly II_EngagementTypesService _engagementTypesService;
        private readonly IGesUserService _gesUserService;
        private readonly IG_IndividualsService _gIndividualsService;

        #endregion

        #region Constructor

        public GesServicesController(IGesLogger logger, II_ServicesService servicesService, IGesUserService gesUserService, II_EngagementTypesService engagementTypesService, IG_IndividualsService gIndividualsService, IG_ServicesRepository gServicesRepository)
            : base(logger)
        {
            _gesUserService = gesUserService;
            _servicesService = servicesService;
            _engagementTypesService = engagementTypesService;
            _gIndividualsService = gIndividualsService;
            _gServicesRepository = gServicesRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigService", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            ViewBag.Title = "GES Services";
            return View();
        }

        [CustomAuthorize(FormKey = "ConfigService", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            long serviceId;
            var servicesModel = new ServicesModel();

            if (long.TryParse(id, out serviceId))
            {
                var gesServiceDetails = _servicesService.GetById(serviceId);

                if (gesServiceDetails != null)
                {
                    servicesModel.GServicesId = gesServiceDetails.G_Services_Id;
                    servicesModel.Name = gesServiceDetails.Name;
                    servicesModel.Sort = gesServiceDetails.Sort;
                    servicesModel.EngagementTypesId = gesServiceDetails.I_EngagementTypes_Id;
                    servicesModel.ShowInClient = gesServiceDetails.ShowInClient;
                    servicesModel.ShowInNavigation = gesServiceDetails.ShowInNavigation;
                    servicesModel.Url = gesServiceDetails.Url;
                    servicesModel.ReportLetter = gesServiceDetails.ReportLetter;
                }
            }

            ViewBag.NgController = "GesServicesController";

            return View(servicesModel);
        }

        #endregion

        #region JsonResult
        
        [HttpPost]
        public JsonResult GetDataForGesServicesJqGrid(JqGridViewModel jqGridParams)
        {
            var listGesServices = this.SafeExecute(() => _servicesService.GetGesServices(jqGridParams), "Error when getting the Ges services {@JqGridViewModel}", jqGridParams);

            return Json(listGesServices);
        }
        
        [HttpGet]
        public JsonResult GetDataForGesServices()
        {
            var listGesServices = this.SafeExecute(() => _servicesService.GetGesServices(), "Error when getting the Ges services");

            return Json(listGesServices, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetAllActiveEngagementType()
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var engagementTypes = _engagementTypesService.GetAllActiveEngagementType(orgId);

            return Json(engagementTypes, JsonRequestBehavior.AllowGet);
        }
        
        
        [HttpGet]
        public JsonResult GesServiceDetails(long id)
        {
            var servicesModel = new ServicesModel();

            if (id != 0)
            {
                var gesServiceDetails = _servicesService.GetById(id);

                if (gesServiceDetails != null)
                {
                    servicesModel.GServicesId = gesServiceDetails.G_Services_Id;
                    servicesModel.Name = gesServiceDetails.Name;
                    servicesModel.Sort = gesServiceDetails.Sort;
                    servicesModel.EngagementTypesId = gesServiceDetails.I_EngagementTypes_Id;
                    servicesModel.ShowInClient = gesServiceDetails.ShowInClient;
                    servicesModel.ShowInNavigation = gesServiceDetails.ShowInNavigation;
                    servicesModel.Url = gesServiceDetails.Url;
                    servicesModel.ReportLetter = gesServiceDetails.ReportLetter;
                }
            }

            ViewBag.NgController = "GesServicesController";

            return Json(servicesModel, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult UpdateGesServicesService(ServicesModel servicesModel)
        {
            G_Services updatingGServices;
            if (servicesModel.GServicesId == 0)
            {
                updatingGServices = new G_Services();
            }
            else
            {
                updatingGServices = _gServicesRepository.GetById(servicesModel.GServicesId);
                if (updatingGServices == null)
                    return null;
            }

            updatingGServices.Sort = servicesModel.Sort;
            updatingGServices.Name = servicesModel.Name;
            updatingGServices.Url = servicesModel.Url;
            updatingGServices.ShowInClient = servicesModel.ShowInClient;
            updatingGServices.ShowInNavigation = servicesModel.ShowInNavigation;
            updatingGServices.ReportLetter = servicesModel.ReportLetter;
            updatingGServices.I_EngagementTypes_Id = servicesModel.EngagementTypesId;


            if (servicesModel.GServicesId == 0)
            {
                _gServicesRepository.Add(updatingGServices);
            }
            else
            {
                _gServicesRepository.Edit(updatingGServices);
            }

            _gServicesRepository.Save();

            return Json(new { Status = "Success" });
        }
        
        #endregion
    }
}