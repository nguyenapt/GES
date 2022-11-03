using System;
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
    public class GuiderlinesController : GesControllerBase
    {
        #region Declaration

        private readonly II_NormsService _normsService;
        private readonly II_NormsRepository _normsRepository;

        #endregion

        #region Constructor

        public GuiderlinesController(IGesLogger logger, II_NormsService normsService, II_NormsRepository normsRepository 
        )
            : base(logger)
        {
            _normsService = normsService;
            _normsRepository = normsRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigGuideline", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            ViewBag.Title = "All Guidelines";

            return View();
        }

        [CustomAuthorize(FormKey = "ConfigGuideline", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            long normId;
            var result = Int64.TryParse(id, out normId);
            ViewBag.Id = -1;
            if (result)
                ViewBag.Id = normId;

            var guidelineName = "Which Guideline?";
            if (result)
            {
                guidelineName = this.SafeExecute(() => _normsService.GetById(normId).FriendlyName,
                    $"Error when getting the guiderline by Id ({normId})");
            }

            ViewBag.Title = $"Guiderline: {guidelineName}";
            ViewBag.NgController = "GuiderlineController";

            return View();
        }

        public ActionResult Add()
        {
            ViewBag.NgController = "GuiderlineController";
            return View("Details");
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForGuiderlinesJqGrid(JqGridViewModel jqGridParams)
        {
            var listGuiderlines = this.SafeExecute(() => _normsService.GetAllConventionsForGrid(jqGridParams),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listGuiderlines);
        }

        [HttpGet]
        public JsonResult GetConventionDetails(long id)
        {
            NormModel normModel = null;
            if (id != 0)
            {
                var guiderlineDetails = _normsService.GetById(id);
                normModel = Mapper.Map<NormModel>(guiderlineDetails);

            }

            ViewBag.NgController = "GuiderlineController";

            return Json(normModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateGuiderline(NormModel normModel)
        {
            try
            {
                I_Norms updatingGuiderline;
                if (normModel.I_Norms_Id == 0)
                {
                    updatingGuiderline = new I_Norms();

                }
                else
                {
                    updatingGuiderline = _normsRepository.GetById(normModel.I_Norms_Id);
                    if (updatingGuiderline == null)
                        return null;
                }

                updatingGuiderline.FriendlyName = normModel.FriendlyName;
                updatingGuiderline.SectionTitle = normModel.SectionTitle;


                if (normModel.I_Norms_Id == 0)
                {
                    _normsRepository.Add(updatingGuiderline);
                }
                else
                {
                    _normsRepository.Edit(updatingGuiderline);
                }
                
                _normsRepository.Save();

                return Json(new { Status = "Success" });
            }
            catch (Exception e)
            {
                return Json(new { Status = "Save Failed, Caused: " + e.Message});
            }

        }
        
        [HttpPost]
        public JsonResult DeleteGuiderline(long guiderlineId)
        {
            try
            {
                var guiderline = _normsRepository.GetById(guiderlineId);

                if (guiderline != null)
                {
                    _normsRepository.Delete(guiderline);

                    _normsRepository.Save();
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