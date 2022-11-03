using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Services.Interface;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Web.Extensions;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Web.Controllers
{
    //[ResourceAuthorize(InsideResources.PortfolioActions.View, InsideResources.Portfolio)]
    public class CompanyController : GesControllerBase
    {
        #region Declaration
        private II_CompaniesService _companyService;
        private readonly II_CompaniesRepository _companyRepository;
        private IOrganizationService _organizationService;

        private readonly IG_IndividualsService _gIndividualsService;
        private readonly IGesUserService _gesUserService;

        private II_CompaniesI_ManagementSystemsService _companiesI_ManagementSystemsService;
        private readonly II_GesCaseReportsRepository _gesCaseProfilesRepository;
        private readonly II_CalendarEventsRepository _calendarEventsRepository;
        private readonly ICalendarService _calendarService;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IGesEventCalendarAttendeesRepository _attendeesRepository;
        
        #endregion

        #region Constructor

        public CompanyController(IGesLogger logger, II_CompaniesService companyService,
            II_CompaniesRepository companyRepository, IOrganizationService organizationService,
            IGesFileStorageService gesFileStorageService, IG_IndividualsService gIndividualsService,
            IGesUserService gesUserService, II_CompaniesI_ManagementSystemsService companiesI_ManagementSystemsService,
            II_GesCaseReportsRepository gesCaseProfilesRepository, ICalendarService calendarService,
            II_CalendarEventsRepository calendarEventsRepository,
            IGesEventCalendarAttendeesRepository attendeesRepository,
            IApplicationSettingsService applicationSettingsService)
            : base(logger)
        {
            _companyService = companyService;
            _organizationService = organizationService;
            _gIndividualsService = gIndividualsService;
            _companyRepository = companyRepository;
            _gesUserService = gesUserService;
            _companiesI_ManagementSystemsService = companiesI_ManagementSystemsService;
            _gesCaseProfilesRepository = gesCaseProfilesRepository;
            _calendarService = calendarService;
            _calendarEventsRepository = calendarEventsRepository;
            _applicationSettingsService = applicationSettingsService;
            _attendeesRepository = attendeesRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "Company", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            ViewBag.Countries = _organizationService.GetCountries();
            ViewBag.Title = "Companies";

            return View();
        }
        [CustomAuthorize(FormKey = "Company", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            
            long companyId;
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var companyDetail = new CompanyDetailViewModel();

            if (long.TryParse(id, out companyId))
            {
                companyDetail = _companyService.GetCompanyDetailViewModel(companyId, orgId, individualId) ?? new CompanyDetailViewModel();
            }

            companyDetail.OrganizationId = orgId;
            companyDetail.IndividualId = individualId;

            ViewBag.PageClass = "page-company page-company-profile";
            ViewBag.NgController = "CompanyController";
            ViewBag.Id = companyId;
            ViewBag.Countries = _organizationService.GetCountries();
            return View(companyDetail);
        }
        #endregion
        
        #region JsonResult
        [HttpGet]
        public JsonResult GetCompanyById(long id)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var companyDetail = new CompanyDetailViewModel();
            if (id != 0)
            {
                companyDetail = _companyService.GetCompanyDetailViewModel(id, orgId, individualId) ?? new CompanyDetailViewModel();
                
                foreach (var doc in companyDetail.Documents)
                {
                    doc.DownloadUrl = "/documentmgmt/CompanyDocDownload/?documentId=" + doc.G_ManagedDocuments_Id;
                }
            }

            return Json(companyDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllManagementSystems()
        {
            var managementSystems = _companyService.GetAllManagementSystems();
            return Json(managementSystems, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllSubPeerGroups()
        {
            var subPeerGroups = _companyService.GetAllSubPeerGroups();
            return Json(subPeerGroups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllMscis()
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var mscis = _companyRepository.GetMsciViewModels(orgId,individualId);

            return Json(mscis, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllFtses()
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var mscis = _companyRepository.GetFtseViewModels(orgId, individualId);

            return Json(mscis, JsonRequestBehavior.AllowGet);
        }
        

        [HttpGet]
        public JsonResult GetAllCaseProfiles(long companyId)
        {
            var caseProfiles = _gesCaseProfilesRepository.GetCaseProfiles(companyId).OrderByDescending(x=>x.EntryDate).ToList();

            return Json(caseProfiles, JsonRequestBehavior.AllowGet);
        }        
        
        [HttpGet]
        public JsonResult GetCompanyEvents(long companyId)
        {
            var companyEvents = _calendarService.GetCalendarEventsByCompanyId(companyId).OrderByDescending(x=>x.EventDate).ToList();

            return Json(companyEvents, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataForCompaniesJqGrid(JqGridViewModel jqGridParams)
        {
            var listCompany = this.SafeExecute(() => _companyService.GetCompanies(jqGridParams), "Error when getting the companies {@JqGridViewModel}", jqGridParams);

            return Json(listCompany);
        }
                
        [HttpPost]
        public JsonResult GetDataForCompaniesAllowCreateCaseProfileJqGrid(JqGridViewModel jqGridParams)
        {
            var listCompany = this.SafeExecute(() => _companyService.GetGesCompanies(jqGridParams), "Error when getting the companies {@JqGridViewModel}", jqGridParams);

            return Json(listCompany);
        }
        
        [HttpPost]
        public JsonResult GetDataForMasterCompaniesJqGrid(JqGridViewModel jqGridParams)
        {
            var listCompany = this.SafeExecute(() => _companyService.GetMasterCompanies(jqGridParams), "Error when getting the companies {@JqGridViewModel}", jqGridParams);

            return Json(listCompany);
        }

        public JsonResult RepairIsinCodes()
        {
            return Json(new
            {
                fakeIsinCodes = FakeIsinCodes(),
                normalizedIsinCodesWithWhiteSpaces = NormalizeWhiteSpaceIsinCodes(),
                error = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCompany(CompanyDetailViewModel companyDetail)
        {
            I_Companies updatingCompany;
            if (companyDetail.CompanyId == 0)
            {
                updatingCompany = new I_Companies();
            }
            else
            {
                updatingCompany = _companyRepository.GetCompanyById(companyDetail.CompanyId);
                if (updatingCompany == null)
                    return null;
            }

            //updatingCompany.BbgID = companyDetail.BbgID;
            //updatingCompany.UnGlobalCompact = companyDetail.UnGlobalCompact;
            //updatingCompany.GriAlignedDisclosure = companyDetail.GriAlignedDisclosure;
            updatingCompany.Modified = DateTime.UtcNow;
            updatingCompany.ModifiedByG_Users_Id = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId;

            ///TODO: Need add more assigned fields here to complete Company info..
            //updatingCompany.Name = companyDetail.CompanyName;
            //updatingCompany.FtseName = companyDetail.OtherName1;
            //updatingCompany.SixName = companyDetail.OtherName2;
            //updatingCompany.OtherName = companyDetail.OtherName3;
            //updatingCompany.OldName = companyDetail.OldName;
            //updatingCompany.MediaName= companyDetail.MediaName;
            //updatingCompany.MasterI_Companies_Id = companyDetail.MasterCompanyId;
            //updatingCompany.Description = companyDetail.Overview;
            //updatingCompany.TransparencyDisclosure = companyDetail.TransparencyDisclosure;

            //updatingCompany.Sedol = companyDetail.Sedol;
            //if (companyDetail.FTSE!=null){
            //    updatingCompany.I_Ftse_Id = companyDetail.FTSE.Id;
            //}
            //if (companyDetail.MSCI != null){
            //    updatingCompany.I_Msci_Id = companyDetail.MSCI.Id;
            //}
            //updatingCompany.InformationSource = companyDetail.InformationSource;
            //updatingCompany.ListSource = companyDetail.ListSource;
            //updatingCompany.SecurityDescription = companyDetail.SecurityDescription;
            //updatingCompany.Isin = companyDetail.Isin;
            //updatingCompany.CountryOfIncorporationId = companyDetail.CountryId;
            //updatingCompany.SubPeerGroupId = companyDetail.SubPeerGroupId;

            //updatingCompany.CountryRegG_Countries_Id = companyDetail.CountryRegId;
            updatingCompany.Website = companyDetail.Website;
            //updatingCompany.CountryOfIncorporationId = companyDetail.CountryId;
            //updatingCompany.SubPeerGroupId = companyDetail.SubPeerGroupId;

            //updatingCompany.CountryRegG_Countries_Id = companyDetail.CountryRegId;
            //updatingCompany.Website = companyDetail.Website;

            //if (updatingCompany.I_Companies_Id == 0)
            //{
            //    updatingCompany.Created = DateTime.UtcNow;
            //    _companyRepository.Add(updatingCompany);
            //}
            //else
            //{
            //    _companyRepository.Edit(updatingCompany);
            //}

            //_companyRepository.Save();

            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveCompanyManagementSystem(CompanyManagementSystemModel companyManagementSystem)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            I_CompaniesI_ManagementSystems m_CompaniesI_ManagementSystems = null;
            try
            {
                if (companyManagementSystem.I_CompaniesI_ManagementSystems_Id == 0)
                {
                    m_CompaniesI_ManagementSystems = new I_CompaniesI_ManagementSystems()
                    {
                        I_Companies_Id = companyManagementSystem.I_Companies_Id,
                        I_ManagementSystems_Id = companyManagementSystem.I_ManagementSystems_Id,
                        Certification = companyManagementSystem.Certification,
                        Coverage = companyManagementSystem.Coverage,
                        ModifiedByG_Users_Id = individualId,
                        Created = DateTime.Now
                    };
                    _companiesI_ManagementSystemsService.Add(m_CompaniesI_ManagementSystems);

                }
                else
                {
                    m_CompaniesI_ManagementSystems = _companiesI_ManagementSystemsService.GetById(companyManagementSystem.I_CompaniesI_ManagementSystems_Id);

                    if (m_CompaniesI_ManagementSystems != null)
                    {
                        m_CompaniesI_ManagementSystems.I_ManagementSystems_Id = companyManagementSystem.I_ManagementSystems_Id;
                        m_CompaniesI_ManagementSystems.Certification = companyManagementSystem.Certification;
                        m_CompaniesI_ManagementSystems.Coverage = companyManagementSystem.Coverage;
                        m_CompaniesI_ManagementSystems.ModifiedByG_Users_Id = individualId;
                        m_CompaniesI_ManagementSystems.Modified = DateTime.Now;

                        _companiesI_ManagementSystemsService.Update(m_CompaniesI_ManagementSystems);
                    }
                }
                _companiesI_ManagementSystemsService.Save();
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_CompaniesI_ManagementSystems.I_CompaniesI_ManagementSystems_Id });
        }

        [HttpPost]
        public JsonResult DeleteCompanyManagementSystem(CompanyManagementSystemModel companyManagementSystem)
        {
            try
            {
                var m_CompaniesI_ManagementSystems = _companiesI_ManagementSystemsService.GetById(companyManagementSystem.I_CompaniesI_ManagementSystems_Id);

                if (m_CompaniesI_ManagementSystems != null)
                {
                    _companiesI_ManagementSystemsService.Delete(m_CompaniesI_ManagementSystems);

                    _companiesI_ManagementSystemsService.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }        
        
        [HttpPost]
        public JsonResult DeleteCompanyEvent(EventListViewModel companyEvent)
        {
            try
            {
                var calenderEvent = _calendarEventsRepository.GetById(companyEvent.Id);
                
                if (calenderEvent != null)
                {
                    
                    var attendees = calenderEvent.GesEventCalendarUserAccept;

                    if (attendees.Count() != 0)
                    {
                        var eventDetails = _calendarService.GetCalendarEventById(companyEvent.Id);
                        var recipients = eventDetails.Attendees.Select(attendee => new MailAddress(attendee.Email, attendee.FullName)).ToList();
                        EventExportSendMail(eventDetails,recipients, "CANCELLED");
                        
                        _attendeesRepository.DeleteRange(companyEvent.Id);
                        _attendeesRepository.Save();
                    }

                    _calendarEventsRepository.Delete(calenderEvent);
                    _calendarEventsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }
        
        [HttpPost]
        public JsonResult SaveCompanyEvent(EventListViewModel companyEvent)
        {
            var saveCompanyEventResult = false;
            try
            {
                if (companyEvent != null)
                {
                    saveCompanyEventResult = UpdateCompanyEvent(companyEvent);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failed Save Company Event Caused:" + e.Message });
            }

            return Json(new { Status = "Success", saveCompanyEventResult});
        }
        
//        [HttpPost]
//        public JsonResult UploadFile(HttpPostedFileBase file)
//        {
//            var oldSystemFilePath = UtilHelper.GetFilePath(_applicationSettingsService.GesReportFileUploadPathForOldSystem,
//                file.FileName, null);
//
//            var newSystemFilePath =
//                UtilHelper.GetFilePath(_applicationSettingsService.BlobPath, file.FileName, UploadHashcode);
//            
//            SaveFile(file);
//
//            CopyFileToOldSystem(newSystemFilePath, oldSystemFilePath);
//            
//
//            return Json(new {Status = "Success"});
//        }

        #endregion

        #region Private methods

        private int FakeIsinCodes()
        {
            var companies = _companyService.GetCompaniesWithErrorIsinCode().ToList();
            var lastestCustomIsin = _companyService.GetMaximumCustomIsinCode();

            foreach (var company in companies)
            {
                lastestCustomIsin = IsinHelper.GenerateIsinCode(lastestCustomIsin);
                company.Isin = lastestCustomIsin;
                _companyService.Update(company);
            }

            return _companyService.Save();
        }

        private int NormalizeWhiteSpaceIsinCodes()
        {
            foreach (var company in _companyService.GetCompaniesWithWhiteSpaceIsinCode().ToList())
            {
                company.Isin = company.Isin.Trim();
                _companyService.Update(company);
            }

            return _companyService.Save();
        }

        private bool UpdateCompanyEvent(EventListViewModel companyEvent)
        {
            if (companyEvent == null) return false;

            var companyId = companyEvent.CompanyId;

            I_CalenderEvents calenderEvent;
            if (companyEvent.Id != 0)
            {
                calenderEvent = _calendarEventsRepository.GetById(companyEvent.Id);
            }
            else
            {
                calenderEvent = new I_CalenderEvents()
                {
                    I_Companies_Id = companyId,
                    Created = DateTime.UtcNow
                };
            }
                
            calenderEvent.EventTitle = companyEvent.EventTitle;
            calenderEvent.EventDate = DateTimeHelper.ConvertCetOrCestDateTimeToUtc(companyEvent.EventDate, companyEvent.StartTime);
            calenderEvent.StartTime = companyEvent.StartTime;
            calenderEvent.EventEndDate = DateTimeHelper.ConvertCetOrCestDateTimeToUtc(companyEvent.EventEndDate, companyEvent.EndTime);
            calenderEvent.EndTime = companyEvent.EndTime;
            calenderEvent.AllDayEvent = companyEvent.AllDayEvent;
            calenderEvent.GesEvent = companyEvent.IsGesEvent;
            calenderEvent.EventLocation = companyEvent.EventLocation;
            calenderEvent.Description = companyEvent.Description;

            try
            {

                if (companyEvent.Id != 0)
                {
                    _calendarEventsRepository.Edit(calenderEvent);
                }
                else
                {
                    _calendarEventsRepository.Add(calenderEvent);
                }
                
                _calendarEventsRepository.Save();

                if (companyEvent.Id != 0)
                {
                    var eventDetails = _calendarService.GetCalendarEventById(calenderEvent.I_CalenderEvents_Id);
                    var recipients = eventDetails.Attendees.Select(attendee => new MailAddress(attendee.Email, attendee.FullName)).ToList();
                    EventExportSendMail(eventDetails, recipients, "CONFIRMED");
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void EventExportSendMail(EventListViewModel eventDetails, List<MailAddress> recipients, string status)
        {
            var gesCalendarEventAttachments = new List<GesCalendarEventAttachment>(); //TODO: Add attach doc if require, it will implement soon
            
            try
            {
                if (eventDetails == null) return;
                
                var gesCalendarEventOrganizer =
                    new GesCalendarEventOrganizer
                    {
                        CommonName = _applicationSettingsService.EventSenderEmailName,
                        Value = new Uri("mailto:" + _applicationSettingsService.EventSenderEmailAddress)
                    };
                
                var attendees = eventDetails?.Attendees;
                var calendarEventAttendees = new List<GesCalendarEventAttendee>();
                if (attendees != null && _applicationSettingsService.AllowViewAttendees)
                    calendarEventAttendees.AddRange(attendees.Select(attendee => new GesCalendarEventAttendee()
                    {
                        CommonName = attendee.FullName,
                        Email = attendee.Email,
                        Value = new Uri("mailto:" + attendee.Email)
                    }));
                
                var calendarEventAlarms = new List<GesCalendarEventAlarm>();
                var calendarEventAlarm = new GesCalendarEventAlarm()
                {
                    Summary = "Inquiry due in 1 day"
                };
                calendarEventAlarms.Add(calendarEventAlarm);

                var calendarEventRecurrencePatterns = new List<GesCalendarEventRecurrencePattern>();
                var rrule = new GesCalendarEventRecurrencePattern() {Count = 5};
                calendarEventRecurrencePatterns.Add(rrule);
                var method = status == "CANCELLED" ? "CANCEL" : "REQUEST";

                var gesCalendarEvent = new GesCalendarEvent(gesCalendarEventOrganizer,calendarEventAttendees, calendarEventAlarms, gesCalendarEventAttachments, calendarEventRecurrencePatterns )
                {
                    Start = eventDetails.EventDate,
                    End = eventDetails.EventEndDate ?? eventDetails.EventDate,
                    Description = eventDetails.Description,
                    Summary = eventDetails.Heading,
                    Location = eventDetails.EventLocation,
                    Uid = eventDetails.Id + "@sustainalytics.com",
                    IsAllDay = eventDetails.AllDayEvent != null && (bool) eventDetails.AllDayEvent,
                    Sequence = 0,
                    Priority = 5,
                    Status = status,
                    Class = "PUBLIC",
                    Method = method
                };
                
                var calendar = new GesCalendar();
                
                var emailSetting =
                    new GesEmailSetting
                    {
                        SenderEmailAddress = _applicationSettingsService.EventSenderEmailAddress,
                        SenderName = _applicationSettingsService.EventSenderEmailName,
                        Subject = eventDetails.Heading,
                        EmailBody = eventDetails.Description,
                        Method = method,
                        Recipients = recipients
                    };

                var gesSmtpSetting =
                    new GesSmtpSetting
                    {
                        SmtpUserName = _applicationSettingsService.SmtpUserName,
                        SmtpUserPassword = _applicationSettingsService.SmtpUserPassword,
                        SmtpHost = _applicationSettingsService.SmtpHost,
                        SmtpPort = _applicationSettingsService.SmtpPort,
                        SmtpEnableSsl = _applicationSettingsService.SmtpEnableSsl
                    };

                calendar.GesSendEmailCalendar(emailSetting, gesSmtpSetting, gesCalendarEvent);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
        #endregion

    }
}