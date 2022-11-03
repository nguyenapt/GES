using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class GesContactController : GesControllerBase
    {
        private readonly IG_IndividualsRepository _gIndividualsRepository;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_GesCaseReportsRepository _gesCaseProfilesRepository;
        private readonly IOrganizationService _organizationService;
        private readonly IG_OrganizationsRepository _gOrganizationsRepository;

        public GesContactController(IGesLogger logger, IG_IndividualsRepository gIndividualsRepository, IG_IndividualsService gIndividualsService, II_GesCaseReportsRepository gesCaseProfilesRepository, IOrganizationService organizationService, IG_OrganizationsRepository gOrganizationsRepository) : base(logger)
        {
            this._gIndividualsRepository = gIndividualsRepository;
            this._gIndividualsService = gIndividualsService;
            this._gesCaseProfilesRepository = gesCaseProfilesRepository;
            this._organizationService = organizationService;
            this._gOrganizationsRepository = gOrganizationsRepository;
        }

        [CustomAuthorize(FormKey = "Contact", Action = ActionEnum.Read)]
        public ActionResult Index()
        {
            this.SafeExecute(() =>
            {
                ViewBag.ClientsStatuses = _organizationService.GetClientStatuses();
                ViewBag.ClientProgressStatuses = _organizationService.GetClientProgressStatuses();
                ViewBag.Industries = _organizationService.GetIndustries();
                ViewBag.Countries = _organizationService.GetCountries();
            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            ViewBag.Title = "Contacts";
            ViewBag.NgController = "GesContactController";
            return View();
        }

        [HttpPost]
        public JsonResult GetContactsJqGrid(JqGridViewModel jqGridParams)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var contacts = SafeExecute(() => _gesCaseProfilesRepository.GetContacts(jqGridParams, orgId), "Error when getting the Contacts {@JqGridViewModel}", jqGridParams);

            return Json(contacts);
        }        

        [HttpPost]
        public JsonResult SaveGesContact(GesContact contact)
        {
            G_Individuals updatingGesContact;
            if (contact.UserId == 0)
            {
                updatingGesContact = new G_Individuals();                
            }
            else
            {
                updatingGesContact = _gIndividualsRepository.GetById(contact.UserId);
                if (updatingGesContact == null)
                    return null;
            }

            updatingGesContact.FirstName = contact.FirstName;
            updatingGesContact.LastName= contact.LastName;
            updatingGesContact.Email= contact.Email;
            updatingGesContact.JobTitle= contact.JobTitle;
            updatingGesContact.G_Organizations_Id = contact.OrganizationId;
            updatingGesContact.Phone = contact.Phone;
            updatingGesContact.Comment = contact.Comment;            

            if (contact.UserId == 0)
            {
                updatingGesContact.G_TimeZones_Id = 27;//Default value
                _gIndividualsRepository.Add(updatingGesContact);
            }
            else
            {
                _gIndividualsRepository.Edit(updatingGesContact);
            }

            _gIndividualsRepository.Save();

            return Json(new { Status = "Success" });
        }

        [HttpGet]
        public JsonResult GetDialogueByIndividual(long individualId, string type)
        {
            IList<DialogueCaseModel> dialogues = new List<DialogueCaseModel>();
            if (type.ToUpper() == "COMPANY"){
                dialogues = _gIndividualsService.GetCompanyDialogueLogsByIndividual(individualId);
            }
            else if (type.ToUpper() == "SOURCE"){
                dialogues = _gIndividualsService.GetSourceDialogueLogsByIndividual(individualId);
            }
            return Json(dialogues, JsonRequestBehavior.AllowGet);
        }        
    }
}