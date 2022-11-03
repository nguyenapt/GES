using System;
using System.Linq;
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
    public class CaseProfileUIConfigController : GesControllerBase
    {

        #region Declaration
        private readonly II_EngagementTypesService _engagementTypesService;
        private readonly IGesUserService _gesUserService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly IGesCaseProfileTemplatesRepository _caseProfileTemplatesRepository;
        private readonly II_GesCaseReportsRepository _gesCaseProfilesRepository;
        private readonly IGesCaseProfileInvisibleEntitiesRepository _gesCaseProfileInvisibleEntitiesRepository;

        #endregion

        #region Constructor

        public CaseProfileUIConfigController(IGesLogger logger,
            IGesUserService gesUserService, II_EngagementTypesService engagementTypesService,
            IG_IndividualsService gIndividualsService,
            IGesCaseProfileTemplatesRepository caseProfileTemplatesRepository,
            II_GesCaseReportsRepository gesCaseProfilesRepository,
            IGesCaseProfileInvisibleEntitiesRepository gesCaseProfileInvisibleEntitiesRepository)
            : base(logger)
        {
            _gesUserService = gesUserService;
            _engagementTypesService = engagementTypesService;
            _gIndividualsService = gIndividualsService;
            _caseProfileTemplatesRepository = caseProfileTemplatesRepository;
            _gesCaseProfilesRepository = gesCaseProfilesRepository;
            _gesCaseProfileInvisibleEntitiesRepository = gesCaseProfileInvisibleEntitiesRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigCaseProfileTemplate", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            ViewBag.Title = "Case Profile templates";
            return View();
        }

        [CustomAuthorize(FormKey = "ConfigCaseProfileTemplate", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            var sesCaseProfileInvisibleEntitiesViewModel = id != "Add" ? _caseProfileTemplatesRepository.GetViewModelById(new Guid(id)) : new GesCaseProfileTemplatesViewModel();

            ViewBag.NgController = "CaseProfileUITemplateController";

            return View(sesCaseProfileInvisibleEntitiesViewModel);
        }

        #endregion

        #region JsonResult
        
        [HttpPost]
        public JsonResult GetDataForCaseProfileUIsJqGrid(JqGridViewModel jqGridParams)
        {
            var listCaseProfileAttributes = this.SafeExecute(() => _caseProfileTemplatesRepository.GetCaseProfilesUITemplate(jqGridParams), "Error when getting the Ges services {@JqGridViewModel}", jqGridParams);

            return Json(listCaseProfileAttributes);
        }
        
        [HttpGet]
        public JsonResult CheckExistedTemplate(long? engagementTypesId, long? recomendationId)
        {
            var isExisted = _caseProfileTemplatesRepository.CheckExistedTemplate(engagementTypesId, recomendationId);
            
            if (isExisted)
            {
                return Json(new
                {
                    existed = true,
                    message = "The template with selected Engagement type and Engagement status already exists"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                existed = false,
                message = ""
            }, JsonRequestBehavior.AllowGet);

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
        public JsonResult GetAllRecommendations()
        {
            var recoumendations = _gesCaseProfilesRepository.GetRecommendations().Select(x => new { x.I_GesCaseReportStatuses_Id, x.Name });
            return Json(recoumendations, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetAllGesCaseProfileEntities()
        {
            var gesCaseProfileEntities = _caseProfileTemplatesRepository.I_GesCaseProfileEntitiesGroup();
            return Json(gesCaseProfileEntities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllGesCaseProfileEntitiesClient()
        {
            var gesCaseProfileEntities = _caseProfileTemplatesRepository.I_GesCaseProfileEntitiesGroupClient();
            return Json(gesCaseProfileEntities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGesCaseProfileUiTemaplateDetails(string id)
        {
            var sesCaseProfileInvisibleEntitiesViewModel = id != "Add" ? _caseProfileTemplatesRepository.GetViewModelById(new Guid(id)) : new GesCaseProfileTemplatesViewModel();

            ViewBag.NgController = "CaseProfileUITemplateController";

            return Json(sesCaseProfileInvisibleEntitiesViewModel, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult UpdateCaseProfileUiTemplate(GesCaseProfileTemplatesViewModel gesCaseProfileTemplatesViewModel)
        {
            GesCaseProfileTemplates updatingGesCaseProfileTemplate;
            if (gesCaseProfileTemplatesViewModel.GesCaseProfileTemplatesId == Guid.Empty)
            {
                updatingGesCaseProfileTemplate =
                    new GesCaseProfileTemplates
                    {
                        GesCaseProfileTemplates_Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow
                    };
            }
            else
            {
                updatingGesCaseProfileTemplate = _caseProfileTemplatesRepository.GetById(gesCaseProfileTemplatesViewModel.GesCaseProfileTemplatesId);
                if (updatingGesCaseProfileTemplate == null)
                    return null;
            }

            updatingGesCaseProfileTemplate.Name = gesCaseProfileTemplatesViewModel.TemplateName;
            updatingGesCaseProfileTemplate.Description = gesCaseProfileTemplatesViewModel.TemplateDescription;
            updatingGesCaseProfileTemplate.I_EngagementTypes_Id = gesCaseProfileTemplatesViewModel.EngagementTypeId;
            updatingGesCaseProfileTemplate.I_GesCaseReportStatuses_Id = gesCaseProfileTemplatesViewModel.RecomendationId;


            if (gesCaseProfileTemplatesViewModel.GesCaseProfileTemplatesId == Guid.Empty)
            {
                _caseProfileTemplatesRepository.Add(updatingGesCaseProfileTemplate);
            }
            else
            {
                _caseProfileTemplatesRepository.Edit(updatingGesCaseProfileTemplate);
            }

            _caseProfileTemplatesRepository.Save();

            UpdateInvisiableEntities(updatingGesCaseProfileTemplate, gesCaseProfileTemplatesViewModel);

            return Json(new { Status = "Success" });
        }
        #endregion
        
        #region private

        private void UpdateInvisiableEntities(GesCaseProfileTemplates updatingGesCaseProfileTemplate,GesCaseProfileTemplatesViewModel gesCaseProfileTemplatesViewModel)
        {
            var entities = _gesCaseProfileInvisibleEntitiesRepository.GetAllEntitiesByTemplateId(updatingGesCaseProfileTemplate.GesCaseProfileTemplates_Id);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    _gesCaseProfileInvisibleEntitiesRepository.Delete(entity);
                }

                _gesCaseProfileInvisibleEntitiesRepository.Save();

            }

            var invisibleEntities = gesCaseProfileTemplatesViewModel.ProfileInvisibleEntitiesViewModels;

            if (invisibleEntities == null) return;
            foreach (var entity in invisibleEntities)
            {
                var invisibleEntity =
                    new GesCaseProfileInvisibleEntities
                    {
                        GesCaseProfileInvisibleEntity_Id = Guid.NewGuid(),
                        GesCaseProfileTemplates_Id = updatingGesCaseProfileTemplate.GesCaseProfileTemplates_Id,
                        GesCaseProfileEntity_Id = entity.GesCaseProfileEntity_Id,
                        InVisibleType = entity.InVisibleType
                    };

                _gesCaseProfileInvisibleEntitiesRepository.Add(invisibleEntity);
            }

            _gesCaseProfileInvisibleEntitiesRepository.Save();

        }

        #endregion
    }
}