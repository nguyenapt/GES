using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Web.Extensions;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Web.Controllers
{
    public class ClientController : GesControllerBase
    {
        #region Declaration

        private readonly IOrganizationService _organizationService;
        private readonly IG_OrganizationsRepository _gOrganizationsRepository;
        private readonly II_CompaniesService _companyService;
        private readonly II_ServicesService _servicesService;
        private readonly IG_IndividualsRepository _gIndividualsRepository;
        private readonly IG_OrganizationsG_ServicesService _gOrganizationsGServicesService;
        private readonly IG_OrganizationsG_ServicesRepository _gOrganizationsGServicesRepository;
        private readonly IGesUserService _gesUserService;

        #endregion

        #region Constructor

        public ClientController(IGesLogger logger, IOrganizationService organizationService,
            IG_OrganizationsRepository gOrganizationsRepository,
            II_CompaniesService companyService, II_ServicesService servicesService,
            IG_IndividualsRepository gIndividualsRepository,
            IG_OrganizationsG_ServicesService gOrganizationsGServicesService, IG_OrganizationsG_ServicesRepository gOrganizationsGServicesRepository, IGesUserService gesUserService)
            : base(logger)
        {
            _organizationService = organizationService;
            _gOrganizationsRepository = gOrganizationsRepository;
            _companyService = companyService;
            _servicesService = servicesService;
            _gIndividualsRepository = gIndividualsRepository;
            _gOrganizationsGServicesService = gOrganizationsGServicesService;
            _gesUserService = gesUserService;
            _gOrganizationsGServicesRepository = gOrganizationsGServicesRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "Client", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            this.SafeExecute(() =>
            {
                ViewBag.ClientsStatuses = _organizationService.GetClientStatuses();
                ViewBag.ClientProgressStatuses = _organizationService.GetClientProgressStatuses();
                ViewBag.Industries = _organizationService.GetIndustries();
                ViewBag.Countries = _organizationService.GetCountries();
            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            ViewBag.Title = "All Clients";

            return View();
        }

        [CustomAuthorize(FormKey = "Client", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            long clientId;
            var result = Int64.TryParse(id, out clientId);
            ViewBag.Id = -1;
            if (result)
                ViewBag.Id = clientId;

            var orgName = "Which client?";
            if (result)
            {
                orgName = this.SafeExecute(() => _organizationService.GetById(clientId).Name,
                    $"Error when getting the organization by Id ({clientId})");
            }

            ViewBag.Title = String.Format("Client: {0}", orgName);
            ViewBag.NgController = "ClientController";

            return View();
        }

        public ActionResult Add()
        {
            ViewBag.NgController = "ClientController";
            return View("Details");
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForClientsJqGrid(JqGridViewModel jqGridParams)
        {
            var listClient = this.SafeExecute(() => _organizationService.GetClients(jqGridParams),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listClient);
        }

        [HttpPost]
        public JsonResult GetAllDataForClientsJqGrid(JqGridViewModel jqGridParams)
        {
            var listClient = this.SafeExecute(() => _organizationService.GetClients(jqGridParams, true),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listClient);
        }

        [HttpGet]
        public JsonResult GetClientDetails(long id)
        {
            OrganizationViewModel organizationViewModel = null;
            if (id != 0)
            {
                var organizationDetails = _organizationService.GetById(id);
                organizationViewModel = Mapper.Map<OrganizationViewModel>(organizationDetails);
                organizationViewModel.Created = organizationDetails.Created.ToString(Configurations.DateFormat);

                if (organizationDetails.ModifiedByG_Users_Id != null && organizationDetails.Modified != null)
                {
                    var individual = _gIndividualsRepository.GetIndividualByUser((long)organizationDetails
                        .ModifiedByG_Users_Id);
                    if (individual != null)
                    {
                        organizationViewModel.ModifyString =
                            $"{individual.FirstName} {individual.LastName} at {organizationDetails.Modified.ToString()}";
                    }
                }

                var organizationsGServices = _gOrganizationsGServicesService.GetOrganizationsServices(id).ToList();

                organizationViewModel.OrganizationsServicesViewModels = organizationsGServices;


            }

            ViewBag.NgController = "GesServicesController";

            return Json(organizationViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            var countries = _companyService.AllCountries().Select(x => new
            {
                x.Id,
                FullName = x.Name ?? string.Empty,
                CountryCode = x.Alpha3Code?.ToLower()?.Substring(0,2)
            });
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIndustries()
        {
            var industries = _organizationService.GetIndustriesObject()
                .Select(x => new { x.G_Industries_Id, FullName = x.Name });
            return Json(industries, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetServices()
        {
            var services = _servicesService.GetAll().Select(x => new { x.G_Services_Id, FullName = x.Name });
            return Json(services, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetCompaniesForAutocomplete(string term, int limit)
        {
            var clients = this.SafeExecute(() => _companyService.GetCompaniesWithTerm(term, limit),
                $"Error when getting the company with term ('{term}')");

            return Json(new
            {
                total = clients.Count,
                rows = clients
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrganizationServices(long id)
        {

            var organizationsGServices = _gOrganizationsGServicesService.GetOrganizationsServices(id);

            return Json(organizationsGServices, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCompaniesWSubCompaniesForAutocomplete(string term, int limit)
        {
            var clients = this.SafeExecute(() => _companyService.GetCompaniesAndSubCompaniesWithTerm(term, limit),
                $"Error when getting the company with term ('{term}')");

            return Json(new
            {
                total = clients.Count,
                rows = clients
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateClient(OrganizationViewModel client)
        {
            G_Organizations updatingOrganizations;
            if (client.G_Organizations_Id == 0)
            {
                updatingOrganizations = new G_Organizations { Created = DateTime.UtcNow };
            }
            else
            {
                updatingOrganizations = _gOrganizationsRepository.GetById(client.G_Organizations_Id);
                if (updatingOrganizations == null)
                    return null;

                updatingOrganizations.Modified = DateTime.UtcNow;
            }

            updatingOrganizations.Name = client.Name;
            updatingOrganizations.OrgNr = client.OrgNr;
            updatingOrganizations.Address1 = client.Address1;
            updatingOrganizations.Address2 = client.Address2;
            updatingOrganizations.Address3 = client.Address3;
            updatingOrganizations.PostalCode = client.PostalCode;
            updatingOrganizations.City = client.City;
            updatingOrganizations.CountryId = client.CountryId;
            updatingOrganizations.BillingAddress1 = client.BillingAddress1;
            updatingOrganizations.BillingAddress2 = client.BillingAddress2;
            updatingOrganizations.BillingAddress3 = client.BillingAddress3;
            updatingOrganizations.BillingPostalCode = client.BillingPostalCode;
            updatingOrganizations.BillingCity = client.BillingCity;
            updatingOrganizations.BillingG_Countries_Id = client.BillingG_Countries_Id;
            updatingOrganizations.Phone = client.Phone;
            updatingOrganizations.Fax = client.Fax;
            updatingOrganizations.WebsiteUrl = client.WebsiteUrl;
            updatingOrganizations.G_Industries_Id = client.G_Industries_Id;
            updatingOrganizations.Employees = client.Employees;
            updatingOrganizations.Members = client.Members;
            updatingOrganizations.TotalAssets = client.TotalAssets;
            updatingOrganizations.Equity = client.Equity;
            updatingOrganizations.Property = client.Property;
            updatingOrganizations.Fi = client.Fi;
            updatingOrganizations.OurApproach = client.OurApproach;
            updatingOrganizations.Comment = client.Comment;
            updatingOrganizations.Customer = client.Customer;
            updatingOrganizations.ModifiedByG_Users_Id = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId;

            //TODO: Will be added later
            //            updatingOrganizations.OwnerG_Organizations_Id = client.OwnerG_Organizations_Id;
            //            updatingOrganizations.LicenseKey = client.LicenseKey;
            //            updatingOrganizations.Domain = client.Domain;
            //            updatingOrganizations.WebsiteExists = client.WebsiteExists;
            //            updatingOrganizations.G_Languages_Id = client.G_Languages_Id;
            //            updatingOrganizations.G_PaymentMethods_Id = client.G_PaymentMethods_Id;
            //            updatingOrganizations.PostgiroNumber = client.PostgiroNumber;
            //            updatingOrganizations.BankgiroNumber = client.BankgiroNumber;
            //            updatingOrganizations.LogoUrl = client.LogoUrl;
            //            updatingOrganizations.G_SaleStates_Id = client.G_SaleStates_Id;
            //            updatingOrganizations.CreditPaymentTerms = client.CreditPaymentTerms;
            //            updatingOrganizations.G_SalesRatings_Id = client.G_SalesRatings_Id;
            //            updatingOrganizations.SalesInterest = client.SalesInterest;
            //            updatingOrganizations.SalesFirstContact = client.SalesFirstContact;
            //            updatingOrganizations.SalesLastContact = client.SalesLastContact;
            //            updatingOrganizations.SalesNextContact = client.SalesNextContact;
            //            updatingOrganizations.SalesMeetingReport = client.SalesMeetingReport;
            //            updatingOrganizations.SalesComment = client.SalesComment;
            //            updatingOrganizations.ReferredBy = client.ReferredBy;
            //            updatingOrganizations.DeliveryDate = client.DeliveryDate;
            //            updatingOrganizations.Customer = client.Customer;
            //            updatingOrganizations.G_OrganizationsI_ClientStatuses_Id = client.G_OrganizationsI_ClientStatuses_Id;
            //            updatingOrganizations.I_ClientProgressStatuses_Id = client.I_ClientProgressStatuses_Id;
            //            updatingOrganizations.FreeHours = client.FreeHours;
            //            updatingOrganizations.StandardPrice = client.StandardPrice;
            //            updatingOrganizations.I_Companies_Id = client.I_Companies_Id;

            try
            {
                if (updatingOrganizations.G_Organizations_Id == 0)
                {
                    _gOrganizationsRepository.Add(updatingOrganizations);
                }
                else
                {
                    _gOrganizationsRepository.Edit(updatingOrganizations);
                }

                _gOrganizationsRepository.Save();

                var updateAgreementResult = UpdateAgreements(
                    (List<OrganizationsServicesViewModel>)client.OrganizationsServicesViewModels,
                    updatingOrganizations.G_Organizations_Id);

                if (!updateAgreementResult)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed updating Agreements."
                    });
                }

                return Json(new { Status = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Status = "Failed" });
            }

        }


        #endregion

        #region Private methods

        private bool UpdateAgreements(IReadOnlyCollection<OrganizationsServicesViewModel> newAgreements,
            long orgId)
        {
            var oldAgreements = _gOrganizationsGServicesRepository.GetOrganizationServices(orgId).ToList();

            if (newAgreements == null)
            {
                //Remove all news
                foreach (var deleteItem in oldAgreements)
                {
                    _gOrganizationsGServicesRepository.Delete(deleteItem);
                }

                return true;
            }

            var listAgreementIds = newAgreements.ToList().Select(d => d.OrganizationsServicesId);

            try
            {
                //Delete Item
                var listDelete = oldAgreements.Where(d => !listAgreementIds.Contains(d.G_OrganizationsG_Services_Id));

                foreach (var deleteItem in listDelete)
                {
                    _gOrganizationsGServicesRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newAgreementItem in newAgreements)
                {
                    if (newAgreementItem.OrganizationsServicesId == 0)
                    {
                        _gOrganizationsGServicesRepository.Add(new G_OrganizationsG_Services()
                        {

                            G_Organizations_Id = orgId,
                            G_Services_Id = newAgreementItem.ServicesId,
                            Created = DateTime.UtcNow,
                            ModifiedByG_Users_Id = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId,
                            Price = newAgreementItem.Price,
                            Reporting = newAgreementItem.Reporting,
                            Comment = newAgreementItem.Comment,
                            G_ServiceStates_Id = 1, //Will be implement later
                            W_SuperFilter = false, //Will be implement later
                            TermsAccepted = false //Will be implement later
                            //G_ManagedDocuments_Id = newAgreementItem.ManagedDocumentsId,
                            //DemoEnd = newAgreementItem.DemoEnd,
                            //G_ServiceStates_Id = newAgreementItem.ServiceStatesId,
                            //TermsAccepted = newAgreementItem.TermsAccepted,
                            //TermsAcceptedByIp = newAgreementItem.TermsAcceptedByIp,
                            //W_SuperFilter = newAgreementItem.SuperFilter
                        });
                    }
                    else
                    {
                        var updateAgreement = _gOrganizationsGServicesRepository.GetById(newAgreementItem.OrganizationsServicesId);
                        updateAgreement.G_Services_Id = newAgreementItem.ServicesId;
                        updateAgreement.Price = newAgreementItem.Price;
                        updateAgreement.Reporting = newAgreementItem.Reporting;
                        updateAgreement.Comment = newAgreementItem.Comment;

                        updateAgreement.Modified = newAgreementItem.Modified;
                        updateAgreement.ModifiedByG_Users_Id =
                            _gesUserService.GetById(User.Identity.GetUserId()).OldUserId;
                        _gOrganizationsGServicesRepository.Edit(updateAgreement);
                    }
                }

                _gOrganizationsGServicesRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public JsonResult SaveGesOrganization(ClientViewModel organization)
        {
            G_Organizations updatingOrganization;
            if (organization.Id == 0)
            {
                updatingOrganization = new G_Organizations();
            }
            else
            {
                updatingOrganization = _gOrganizationsRepository.GetById(organization.Id);
                if (updatingOrganization == null)
                    return null;
            }

            updatingOrganization.Name = organization.Name;
            updatingOrganization.Address1 = organization.Address;
            updatingOrganization.PostalCode = organization.PostalCode;
            updatingOrganization.City = organization.City;
            updatingOrganization.CountryId = organization.CountryId;
            updatingOrganization.Phone = organization.Phone;
            updatingOrganization.WebsiteUrl = organization.Website;
            updatingOrganization.Comment = organization.Comment;            

            if (organization.Id == 0)
            {
                updatingOrganization.Created = DateTime.Now;
                updatingOrganization.Customer = false;
                _gOrganizationsRepository.Add(updatingOrganization);
            }
            else
            {
                _gOrganizationsRepository.Edit(updatingOrganization);
            }

            _gOrganizationsRepository.Save();

            return Json(new { Status = "Success" });
        }

        #endregion

    }
}