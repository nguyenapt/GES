using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Logging;
using GES.Inside.Data.Configs;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Inside.Web.Controllers
{
    public class SdgController : GesControllerBase
    {
        private readonly ISdgRepository _sdgRepository;
        private readonly IGesDocumentService _gesdocumentService;

        public SdgController(IGesLogger logger, ISdgRepository sdgRepository, IGesDocumentService gesdocumentService) : base(logger)
        {
            _sdgRepository = sdgRepository;
            _gesdocumentService = gesdocumentService;
        }

        [CustomAuthorize(FormKey = "ConfigSDG", Action = ActionEnum.Read)]
        public ActionResult Index()
        {
            ViewBag.NgController = "SdgController";
            return View();
        }

        [HttpGet]
        public JsonResult GetSdgs()
        {
            var sdgs = _sdgRepository.GetSdgs().Select(x => new SdgViewModel { Sdg_Id = x.Sdg_Id, Sdg_Link = x.Sdg_Link, Sdg_Name = x.Sdg_Name, DocumentId = x.DocumentId??new Guid()});
            return Json(sdgs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSdgById(long sdgId)
        {
            var sdg = _sdgRepository.GetById(sdgId);
            return Json(new SdgViewModel { Sdg_Id = sdg.Sdg_Id, Sdg_Link = sdg.Sdg_Link, Sdg_Name = sdg.Sdg_Name, DocumentId = sdg.DocumentId ?? new Guid() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSdg(SdgViewModel sdg, HttpPostedFileBase file)
        {
            var newDocumentId = SaveSdgIcon(file);
            var updatingSdg = new Sdg
            {
                Sdg_Id = sdg.Sdg_Id,
                Sdg_Name = sdg.Sdg_Name,
                Sdg_Link = sdg.Sdg_Link,
                DocumentId = newDocumentId == new Guid() ? sdg.DocumentId : newDocumentId
            };
            
            if (updatingSdg.Sdg_Id == 0)
            {
                updatingSdg.Created = DateTime.UtcNow;
                _sdgRepository.Add(updatingSdg);
            }
            else
            {
                updatingSdg.Modified = DateTime.UtcNow;
                _sdgRepository.Edit(updatingSdg);
            }
            _sdgRepository.Save();

            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult DeleteSdg(long sdgId)
        {
            if(_sdgRepository.TryDeleteSdg(sdgId))
                return Json(new { Status = "Success" });
            return Json(new { Status = "Failed" });
        }

        private Guid SaveSdgIcon(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) return new Guid();

            var gesDocument = new GesDocumentViewModel {Id = Guid.NewGuid(), HashCodeDocument = Settings.SdgUploadPath, ServiceId = 5};
            var fileExtension = Path.GetExtension(file.FileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

            var fileName = UtilHelper.CreateSafeFileName(fileNameWithoutExtension, fileExtension);
            var fileStream = file.InputStream;

            gesDocument.Name = fileName;
            _gesdocumentService.SaveGesDocument(gesDocument, fileStream, fileName);
            return gesDocument.Id;
        }
    }
}