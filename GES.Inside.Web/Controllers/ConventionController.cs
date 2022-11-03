using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Web.Controllers
{
    public class ConventionController : GesControllerBase
    {
        #region Declaration

        private readonly II_ConventionsService _conventionsService;
        private readonly II_ConventionsRepository _conventionsRepository;

        #endregion

        #region Constructor

        public ConventionController(IGesLogger logger, II_ConventionsService conventionsService, II_ConventionsRepository conventionsRepository 
        )
            : base(logger)
        {
            _conventionsService = conventionsService;
            _conventionsRepository = conventionsRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigConvention", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            ViewBag.Title = "All Conventions";

            return View();
        }

        [CustomAuthorize(FormKey = "ConfigConvention", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            long conventionId;
            var result = Int64.TryParse(id, out conventionId);
            ViewBag.Id = -1;
            if (result)
                ViewBag.Id = conventionId;

            var conventionName = "Which convention?";
            if (result)
            {
                conventionName = this.SafeExecute(() => _conventionsService.GetById(conventionId).Name,
                    $"Error when getting the convention by Id ({conventionId})");
            }

            ViewBag.Title = $"Convention: {conventionName}";
            ViewBag.NgController = "ConventionController";

            return View();
        }

        public ActionResult Add()
        {
            ViewBag.NgController = "ConventionController";
            return View("Details");
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForConventionsJqGrid(JqGridViewModel jqGridParams)
        {
            var listConventions = this.SafeExecute(() => _conventionsService.GetAllConventionsForGrid(jqGridParams),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listConventions);
        }

        [HttpGet]
        public JsonResult GetConventionDetails(long id)
        {
            ConventionModel conventionModel = null;
            if (id != 0)
            {
                var conventionDetails = _conventionsService.GetById(id);
                conventionModel = Mapper.Map<ConventionModel>(conventionDetails);

            }

            ViewBag.NgController = "ConventionController";

            return Json(conventionModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCatalogues()
        {
            var catalogues = _conventionsService.AllConventionCategories().Select(x => new
            {
                id = x.I_ConventionCategories_Id,
                Name = x.Name ?? string.Empty
            });
            return Json(catalogues, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateConvention(ConventionModel conventionModel)
        {
            try
            {
                I_Conventions updatingConventions;
                if (conventionModel.I_Conventions_Id == 0)
                {
                    updatingConventions = new I_Conventions();

                }
                else
                {
                    updatingConventions = _conventionsRepository.GetById(conventionModel.I_Conventions_Id);
                    if (updatingConventions == null)
                        return null;
                }

                updatingConventions.I_ConventionCategories_Id = conventionModel.I_ConventionCategories_Id;
                updatingConventions.Name = conventionModel.Name;
                updatingConventions.ShortName = conventionModel.ShortName;
                updatingConventions.Text = conventionModel.Text;
                updatingConventions.Type = conventionModel.Type;
                updatingConventions.Background = conventionModel.Background;
                updatingConventions.Guidelines = conventionModel.Guidelines;
                updatingConventions.Purpose = conventionModel.Purpose;
                updatingConventions.Administration = conventionModel.Administration;
                updatingConventions.GesCriteria = conventionModel.GesCriteria;
                updatingConventions.GesScope = conventionModel.GesScope;
                updatingConventions.GesRiskIndustry = conventionModel.GesRiskIndustry;
                updatingConventions.ManagementSystems = conventionModel.ManagementSystems;
                updatingConventions.Links = conventionModel.Links;

                if (conventionModel.I_Conventions_Id == 0)
                {
                    _conventionsRepository.Add(updatingConventions);
                }
                else
                {
                    _conventionsRepository.Edit(updatingConventions);
                }
                
                _conventionsRepository.Save();

                return Json(new { Status = "Success" });
            }
            catch (Exception e)
            {
                return Json(new { Status = "Save Failed, Caused: " + e.Message});
            }

        }
        
        [HttpPost]
        public JsonResult DeleteConvention(long conventionId)
        {
            try
            {
                var convention = _conventionsRepository.GetById(conventionId);

                if (convention != null)
                {
                    _conventionsRepository.Delete(convention);

                    _conventionsRepository.Save();
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Error, Caused:" + e.Message });
            }
            return Json(new { Status = "Success" });
        }


        #endregion

        #region Private methods
       
        #endregion

    }
}