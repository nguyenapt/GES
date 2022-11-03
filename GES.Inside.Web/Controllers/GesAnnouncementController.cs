using System;
using System.Linq;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Inside.Web.Controllers
{
    public class GesAnnouncementController : GesControllerBase
    {

        #region Declaration

        private readonly I_GesAnnouncementService _gesAnnouncementService;
        private readonly I_GesAnnouncementRepository _gesAnnouncementRepository;

        #endregion

        #region Constructor

        public GesAnnouncementController(IGesLogger logger, I_GesAnnouncementService gesAnnouncementService, I_GesAnnouncementRepository gesAnnouncementRepository)
            : base(logger)
        {
            _gesAnnouncementService = gesAnnouncementService;
            _gesAnnouncementRepository = gesAnnouncementRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigAnnouncement", Action = ActionEnum.Read)]
        public ActionResult Index()
        {
            ViewBag.Title = "GES Announcement";
            ViewBag.NgController = "GesAnnouncementController";
            return View();
        }

        [CustomAuthorize(FormKey = "ConfigAnnouncement", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            Guid announcementId;
            var announcementModel = new GesAnnouncementModel();

            if (Guid.TryParse(id, out announcementId))
            {
                var gesAnnouncementDetails = _gesAnnouncementService.GetById(announcementId);

                if (gesAnnouncementDetails != null)
                {
                    announcementModel.GesAnnouncementId = gesAnnouncementDetails.GesAnnouncementId;
                    announcementModel.AnnouncementDate = gesAnnouncementDetails.AnnouncementDate;
                    announcementModel.Content = gesAnnouncementDetails.Content;
                    announcementModel.LinkTitle = gesAnnouncementDetails.LinkTitle;
                    announcementModel.Title = gesAnnouncementDetails.Title;
                }
            }

            ViewBag.NgController = "GesAnnouncementController";

            return View(announcementModel);
        }

        #endregion

        #region JsonResult

        [HttpGet]
        public JsonResult GetAnnouncements()
        {
            var announcementModels = _gesAnnouncementService.GetAll().OrderByDescending(x => x.AnnouncementDate).ToList().Select(x => new GesAnnouncementModel { GesAnnouncementId = x.GesAnnouncementId, AnnouncementDate = x.AnnouncementDate, Content = x.Content, LinkTitle = x.LinkTitle, Title = x.Title });
            return Json(announcementModels, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveAnnouncement(GesAnnouncementModel announcement)
        {
            GesAnnouncement updatingGesAnnouncement;
            if (announcement.GesAnnouncementId == Guid.Empty)
            {
                updatingGesAnnouncement = new GesAnnouncement();
            }
            else
            {
                updatingGesAnnouncement = _gesAnnouncementRepository.GetById(announcement.GesAnnouncementId);
                if (updatingGesAnnouncement == null)
                    return null;
            }

            updatingGesAnnouncement.AnnouncementDate = announcement.AnnouncementDate != null ? announcement.AnnouncementDate.Value.Date : (DateTime?)null;
            updatingGesAnnouncement.Content = announcement.Content;
            updatingGesAnnouncement.LinkTitle = announcement.LinkTitle;
            updatingGesAnnouncement.Title = announcement.Title;


            if (announcement.GesAnnouncementId == Guid.Empty)
            {
                updatingGesAnnouncement.GesAnnouncementId = Guid.NewGuid();
                _gesAnnouncementRepository.Add(updatingGesAnnouncement);
            }
            else
            {
                _gesAnnouncementRepository.Edit(updatingGesAnnouncement);
            }

            _gesAnnouncementRepository.Save();

            return Json(new { Status = "Success" });
        }

        public JsonResult DeleteAnnouncement(Guid announcementId)
        {

            var gesAnnouncementDetails = _gesAnnouncementService.GetById(announcementId);

            if (gesAnnouncementDetails != null)
            {
                _gesAnnouncementRepository.Delete(gesAnnouncementDetails);
            }

            _gesAnnouncementRepository.Save();

            return Json(new { Status = "Success" });
        }

        #endregion
    }
}