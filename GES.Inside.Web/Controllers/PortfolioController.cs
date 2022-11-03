using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Portfolio;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Configs;
using GES.Inside.Web.Helpers;
using GES.Inside.Web.Models;
using jqGridWeb;
using GES.Common.Logging;
using GES.Common.Exceptions;
using Microsoft.Ajax.Utilities;

namespace GES.Inside.Web.Controllers
{
    public class PortfolioController : GesControllerBase
    {
        #region Declaration
        private readonly IPortfolioService _portfolioService;
        private readonly II_PortfoliosG_OrganizationService _portfoliosGOrganizationService;
        private readonly II_CompaniesService _companiesService;
        private readonly II_PortfoliosI_CompaniesService _portfolioCompaniesService;
        private readonly II_PortfolioCompaniesImportService _portfolioCompaniesImportService;
        private readonly IOrganizationService _organizationService;
        private readonly IPortfolioTypesService _portfolioTypesService;
        private readonly II_PortfoliosG_OrganizationG_ServicesService _portfolioOrganizationServiceService;
        private readonly II_PortfoliosG_OrganizationsI_ControversialActivitesService _portfolioOrganizationControversialService;
        private readonly II_ControversialActivitesPresetsService _controActivPresetService;
        private readonly II_ControversialActivitesPresetsItemsService _controActivPresetItemsService;
       
        #endregion

        #region Constructor
        public PortfolioController(IGesLogger logger, IPortfolioService portfolioService,
            II_PortfoliosG_OrganizationService portfoliosGOrganizationService,
            II_CompaniesService companiesService,
            II_PortfoliosI_CompaniesService portfolioCompaniesService,
            II_PortfolioCompaniesImportService portfolioCompaniesImportService,
            IOrganizationService organizationService,
            IPortfolioTypesService portfolioTypesService,
            II_PortfoliosG_OrganizationG_ServicesService portfolioOrganizationServiceService,
            II_PortfoliosG_OrganizationsI_ControversialActivitesService portfolioOrganizationControversialService,
            II_ControversialActivitesPresetsService controActivPresetService,
            II_ControversialActivitesPresetsItemsService controActivPresetItemsService
            ) : base(logger)
        {
            _portfolioService = portfolioService;
            _portfoliosGOrganizationService = portfoliosGOrganizationService;
            _companiesService = companiesService;
            _portfolioCompaniesService = portfolioCompaniesService;
            _portfolioCompaniesImportService = portfolioCompaniesImportService;
            _organizationService = organizationService;
            _portfolioTypesService = portfolioTypesService;
            _portfolioOrganizationServiceService = portfolioOrganizationServiceService;
            _portfolioOrganizationControversialService = portfolioOrganizationControversialService;
            _controActivPresetService = controActivPresetService;
            _controActivPresetItemsService = controActivPresetItemsService;
        }

        #endregion

        #region ActionResult
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(FormKey = "Portfolio", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            var orgId = Request.QueryString["orgid"];
            long orgIdInt;
            var result = Int64.TryParse(orgId, out orgIdInt);
            ViewBag.OrgId = -1;
            if (result)
                ViewBag.OrgId = orgIdInt;

            var orgName = "Which Organization?";

            this.SafeExecute(() =>
            {
                if (result)
                {
                    orgName = _organizationService.GetById(orgIdInt).Name;
                }
                ViewBag.OrgName = orgName;

                ViewBag.Title = result ? String.Format("Portfolios of: {0}", orgName) : "Portfolio List";

                ViewBag.PortfolioTypes = _portfolioService.GetPortfolioTypes();
                ViewBag.AgreementServices = _organizationService.GetServicesAgreementByOrganizationId(orgIdInt);
            }, $"Exception when getting the portfolio information.");

            return View();
        }

        [CustomAuthorize(FormKey = "ControActivePreset", Action = ActionEnum.Read)]
        public ActionResult ControActivPresets()
        {
            ViewBag.Title = "Controversial Activities Presets";

            return View();
        }

        [CustomAuthorize(FormKey = "Portfolio", Action = ActionEnum.Read)]
        public ActionResult Details()
        {
            var portfolioIdStr = Request.QueryString["id"];
            var invalidSustainalyticsIDsString = Request.QueryString["invalidId"];
            ViewBag.InvalidIds = invalidSustainalyticsIDsString??"";
            
            long poId;
            long portfolioId;
            bool result = Int64.TryParse(portfolioIdStr, out portfolioId);
            ViewBag.PortfolioId = -1;
            if (Int64.TryParse(Request.QueryString["po_id"], out poId))
            {
                ViewBag.PortfolioOrgId = poId;
                ViewBag.OrgId = this.SafeExecute(() => _portfoliosGOrganizationService.GetOrganizationIdFromPortfolioOrganizationId(poId), $"Error when getting the organizationId");
            }

            if (result)
                ViewBag.PortfolioId = portfolioId;

            var portfolioName = "Which portfolio?";
            if (result)
            {
                portfolioName = _portfolioService.GetById(portfolioId).Name;
            }

            ViewBag.Title = String.Format("Portfolio: {0}", portfolioName);

            return View();
        }

        [HttpPost]
        public ActionResult UpdatePortfolio_ShowInCsc(int portfolioId, bool showInCsc)
        {
            var errorMsg = "";
            try
            {
                var portfolio = _portfolioService.GetById(portfolioId);
                portfolio.ShowInCSC = showInCsc;
                _portfolioService.Update(portfolio, true);
            }
            catch (GesServiceException e)
            {
                errorMsg = e.Message;
            }

            return Json(new
            {
                meta = new
                {
                    success = String.IsNullOrEmpty(errorMsg),
                    error = errorMsg
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPendingCompanyRecords(long id, List<PendingCompanyViewModel> records)
        {
            int addedItems = 0;
            int editedItems = 0;
            int duplicatedItems = 0;
            int deletedItems = 0;

            this.SafeExecute(() => ProcessPendingCompanies(records, id, out addedItems, out editedItems, out duplicatedItems, out deletedItems)
            , $"Error when processing the companies.");
            return Json(new
            {
                meta = new
                {

                },
                data = new
                {
                    AddedItems = addedItems,
                    EditedItems = editedItems,
                    DuplicatedItems = duplicatedItems,
                    DeletedItems = deletedItems
                }
            });
        }

        [HttpPost]
        public ActionResult EditControActivRecords(long poId, List<PortfolioControActivityViewModel> records)
        {
            var addedItems = 0;
            var editedItems = 0;
            var deletedItems = 0;

            this.SafeExecute(() => ProcessControActivities(records, poId, false, false, out addedItems, out editedItems, out deletedItems)
                 , $"Error when processing the control activities.");

            return Json(new
            {
                meta = new
                {
                    success = true
                },
                data = new
                {
                    AddedItems = addedItems,
                    EditedItems = editedItems,
                    DeletedItems = deletedItems
                }
            });
        }

        [HttpPost]
        public ActionResult UpdatePortfolioServices(long pk, long[] value)
        {
            var result = this.SafeExecute(() => _portfolioOrganizationServiceService.UpdatePortfolioOrganizationServices(pk, value)
            , $"Error when updating the portfolio of organization service");

            return Json(new
            {
                meta = new
                {
                    success = result
                }
            });
        }

        /// <summary>
        /// Import portfolio companies
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadExcelFile(int id)
        {
            var file = Request.Files["FileUpload"];
            long portfolioIdInt = id;
            var invalidSustainalyticsIDsString = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var excelHelper = new ExcelReaderHelpers();

                var columnConfigs =
                    excelHelper.GetImportExcelConfigs(Server.MapPath("~/Configs/ExcelImport/ExcelMappingColumn.xml"), "ImportPortfolio");

                var rows = excelHelper.GetExcelReaderData(string.Empty, file.InputStream, fileExtension);

                var listData = new List<I_PortfolioCompaniesImport>();

                foreach (var rowItem in rows)
                {
                    var portfolioImport = new I_PortfolioCompaniesImport();
                    foreach (var config in columnConfigs)
                    {
                        portfolioImport.SetValueByPropertyName(excelHelper.GetRowValue(rowItem, config.ExcelField, config.DataType), config.DatabaseField);
                    }
                    portfolioImport.I_Portfolios_Id = portfolioIdInt;

                    if (portfolioImport.SustainalyticsID != 0)
                    {
                        listData.Add(portfolioImport);
                    }
                    
                }

               invalidSustainalyticsIDsString =  SaveImportFromExcelToDatabase(listData, portfolioIdInt);

            }

            TempData["portfolioId"] = portfolioIdInt;
            return invalidSustainalyticsIDsString != ""
                ? Redirect(Url.Action("Details", "Portfolio") + "?id=" + portfolioIdInt + "&invalidId=" +
                           invalidSustainalyticsIDsString)
                : Redirect(Url.Action("Details", "Portfolio") + "?id=" + portfolioIdInt);
        }

        [HttpPost]
        public ActionResult AddStandardPortfolioForm(long orgId, string orgName)
        {
            return PartialView("_AddStandard", new AddStandardPortfolioViewModel()
            {
                StandardPortfolios = GetStandardPortfolios(),
                OrganizationId = orgId,
                OrganizationName = orgName,
                ShowInCsc = false,
                Alerts = false,
                IsAddAgreementServices = true
            });
        }

        [HttpPost]
        public ActionResult ApplyControActivPresetForm(long poId)
        {
            TempData["PresetsData"] = _controActivPresetService.GetPresetsWithTerm("", 9999);
            return PartialView("_ApplyControActivPreset", new ApplyControActivPresetViewModel()
            {
                Presets = GetPresets(),
                PortfolioOrganizationId = poId,
                OverwriteExistingValues = false
            });
        }

        [HttpPost]
        public ActionResult AddExistingPortfolio(AddStandardPortfolioViewModel model)
        {
            if (!ModelState.IsValid || model.PortfolioId == null)
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Invalid model). Please try again.",
                    error = "Invalid model"
                });

            try
            {
                var existing = _portfoliosGOrganizationService.GetByPortfolioAndOrganizationIds((long)model.PortfolioId, model.OrganizationId);
                if (existing.Count > 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "That portfolio already exists in this organization"
                    });
                }

                var newPortfolioOrganization = new I_PortfoliosG_Organizations()
                {
                    I_Portfolios_Id = (long)model.PortfolioId,
                    G_Organizations_Id = model.OrganizationId,
                    ShowInCsc = model.ShowInCsc,
                    IncludeInAlerts = model.Alerts
                };
                _portfoliosGOrganizationService.Add(newPortfolioOrganization);

                if (_portfoliosGOrganizationService.Save() > 0 && model.IsAddAgreementServices)
                {
                    _portfolioOrganizationServiceService.UpdatePortfolioOrganizationServices(newPortfolioOrganization.I_PortfoliosG_Organizations_Id, _organizationService.GetServicesAgreementByOrganizationId(model.OrganizationId));
                }
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, "Error when adding the existing portfolio {@AddStandardPortfolioViewModel}", model);

                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public ActionResult ApplyControActivPreset(ApplyControActivPresetViewModel model)
        {
            if (!ModelState.IsValid || model.PresetId == null)
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Invalid model). Please try again.",
                    error = "Invalid model"
                });

            var addedItems = 0;
            var overwrittenItems = 0;
            var deletedItems = 0;
            try
            {
                var itemsInPreset = _controActivPresetItemsService.GetPresetItemsByPresetId((long)model.PresetId);
                var itemsInPreset_Converted = itemsInPreset.Select(i => new PortfolioControActivityViewModel
                {
                    ControActivId = i.I_ControversialActivites_Id,
                    ControActivName = "",
                    Threshold = i.MinimumInvolvment
                }).ToList();

                ProcessControActivities(itemsInPreset_Converted, model.PortfolioOrganizationId, true, model.OverwriteExistingValues, out addedItems, out overwrittenItems, out deletedItems);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, "Error when applying controActivityPreset {@ApplyControActivPresetViewModel}.", model);

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        message = "Exception occurred: " + e.Message
                    }
                });
            }

            return Json(new
            {
                meta = new
                {
                    success = true
                },
                data = new
                {
                    AddedItems = addedItems,
                    OverwrittenItems = overwrittenItems
                }
            });
        }

        [HttpPost]
        public ActionResult CreateForm(long orgId, string orgName,
            bool? IncludeInAlerts, string Name, bool StandardPortfolio, bool GESStandardUniverse, string type, long PortfolioId = 0, bool showInCSC = true)
        {

            if (PortfolioId <= 0) // Create
                return PartialView("_Create", new BasicPortfolioViewModel()
                {
                    OrganizationId = orgId,
                    OrganizationName = orgName,
                    PortfolioTypeId = 1, // default to "Customer Portfolio"
                    PortfolioTypes = GetPortfolioTypes(),
                    ShowInCsc = showInCSC,
                    GoToDetailsPage = true,
                    IsAddAgreementServices = true
                });

            var typeId = this.SafeExecute(() => _portfolioTypesService.GetByName(type), $"Error when getting the portfolio type ({type})").I_PortfolioTypes_Id;

            // Edit
            return PartialView("_Create", new BasicPortfolioViewModel()
            {
                OrganizationId = orgId,
                OrganizationName = orgName,
                PortfolioTypeId = typeId,
                PortfolioTypes = GetPortfolioTypes(),
                Alerts = IncludeInAlerts ?? false,
                PortfolioName = Name,
                PortfolioId = PortfolioId,
                StandardPortfolio = StandardPortfolio,
                GesStandardUniverse = GESStandardUniverse,
                ShowInCsc = showInCSC,
                EditingPortfolioOnly = IncludeInAlerts == null
            });
        }

        [HttpPost]
        public ActionResult CreateForm_ControActivPreset(long ControActivPresetId = 0)
        {
            if (ControActivPresetId == 0)
            {
                return PartialView("_Create_ControActivPreset", new CreateControActivPresetViewModel()
                {
                    PresetName = "",
                    PresetId = 0
                });
            }
            else
            {
                var preset = this.SafeExecute(() => _controActivPresetService.GetById(ControActivPresetId), $"Exception when getting the control preset");

                return PartialView("_Create_ControActivPreset", new CreateControActivPresetViewModel()
                {
                    PresetName = preset?.Name,
                    PresetId = ControActivPresetId
                });
            }
        }

        [HttpPost]
        public ActionResult CreatePortfolio(BasicPortfolioViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Invalid model). Please try again.",
                    error = "Invalid model"
                });

            var updating = false;

            long newPortfolioId = -1;
            long newPoId = -1;
            try
            {
                // do something here
                var newPortfolio = new I_Portfolios()
                {
                    Created = DateTime.UtcNow,
                    I_PortfolioTypes_Id = model.PortfolioTypeId,
                    StandardPortfolio = model.StandardPortfolio,
                    GESStandardUniverse = model.GesStandardUniverse,
                    ShowInCSC = model.ShowInCsc,
                    Name = model.PortfolioName,
                    G_Organizations_Id = model.OrganizationId,
                    LastUpdated = DateTime.UtcNow
                };

                // if existed > update
                if (model.PortfolioId > 0)
                {
                    var existed = _portfolioService.GetById(model.PortfolioId);
                    if (existed != null)
                    {
                        updating = true;
                        existed.I_PortfolioTypes_Id = model.PortfolioTypeId;
                        existed.StandardPortfolio = model.StandardPortfolio;
                        existed.GESStandardUniverse = model.GesStandardUniverse;
                        existed.Name = model.PortfolioName;
                        existed.ShowInCSC = model.ShowInCsc;
                        existed.LastUpdated = DateTime.UtcNow;
                        try
                        {
                            _portfolioService.Update(existed, true);

                            if (!model.EditingPortfolioOnly)
                            {
                                var needUpdating =
                                    _portfoliosGOrganizationService.GetByPortfolioAndOrganizationIds(model.PortfolioId,
                                        model.OrganizationId);
                                foreach (var item in needUpdating)
                                {
                                    item.IncludeInAlerts = model.Alerts;
                                    item.ShowInCsc = model.ShowInCsc;
                                    _portfoliosGOrganizationService.Update(item);
                                }
                                _portfoliosGOrganizationService.Save();
                            }
                        }
                        catch (Exception e)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Something went wrong while updating (Exception occurred). Please try again.",
                                error = e.Message
                            });
                        }
                    }
                    else // found nothing to update
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No record matched in Database."
                        });
                    }
                }

                if (!updating)
                {
                    _portfolioService.Add(newPortfolio, true);
                    newPortfolioId = newPortfolio.I_Portfolios_Id;

                    var newPorfolioOrganizationService = new I_PortfoliosG_Organizations()
                    {
                        I_Portfolios_Id = newPortfolio.I_Portfolios_Id,
                        G_Organizations_Id = model.OrganizationId,
                        IncludeInAlerts = model.Alerts,
                        ShowInCsc = model.ShowInCsc
                    };

                    _portfoliosGOrganizationService.Add(newPorfolioOrganizationService);

                    if (_portfoliosGOrganizationService.Save() > 0 && model.IsAddAgreementServices)
                    {
                        newPoId = newPorfolioOrganizationService.I_PortfoliosG_Organizations_Id;
                        _portfolioOrganizationServiceService.UpdatePortfolioOrganizationServices(newPorfolioOrganizationService.I_PortfoliosG_Organizations_Id, _organizationService.GetServicesAgreementByOrganizationId(model.OrganizationId));
                    }
                }
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, "Error when creating the portfolio {@BasicPortfolioViewModel}.", model);

                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Exception occurred). Please try again.",
                    error = e.Message
                });
            }

            return Json(new
            {
                success = true,
                editing = updating,
                redirectUrl = (updating || !model.GoToDetailsPage) ? "" : (Url.Action("Details", "Portfolio") + string.Format("?id={0}&po_id={1}", newPortfolioId, newPoId))
            });
        }

        [HttpPost]
        public ActionResult CreateControActivPreset(CreateControActivPresetViewModel model)
        {

            if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Invalid model). Please try again.",
                    error = "Invalid model"
                });
            var updating = false;
            try
            {

                var dicValue = new Dictionary<long, short>();

                if (!string.IsNullOrWhiteSpace(model.ControvSettings))
                {
                    var valueArray = model.ControvSettings.Split(';');
                    foreach (var item in valueArray)
                    {
                        var itemControversial = item.Split(':');
                        if (itemControversial.Length == 2)
                        {
                            dicValue.Add(long.Parse(itemControversial[0]), short.Parse(itemControversial[1]));
                        }
                    }
                }

                if (model.PresetId > 0)// update
                {
                    updating = true;
                    var preset = _controActivPresetService.GetById(model.PresetId);
                    if (preset != null)
                    {
                        preset.Name = model.PresetName;
                        _controActivPresetService.Update(preset, true);


                        var presetItems = _controActivPresetItemsService.GetPresetItemsByPresetId(model.PresetId).ToList();

                        foreach (var item in dicValue)
                        {
                            var existItem = presetItems.FirstOrDefault(d => d.I_ControversialActivites_Id == item.Key);
                            if (existItem != null)
                            {
                                existItem.MinimumInvolvment = item.Value;

                                _controActivPresetItemsService.Update(existItem);
                            }
                            else
                            {
                                var newPresetItem = new I_ControversialActivitesPresetsItems()
                                {
                                    I_ControversialActivites_Id = item.Key,
                                    I_ControversialActivitesPresets_Id = preset.I_ControversialActivitesPresets_Id,
                                    MinimumInvolvment = item.Value,
                                    Created = DateTime.UtcNow
                                };
                                _controActivPresetItemsService.Add(newPresetItem);
                            }
                        }

                        //delete
                        var deletedItems = presetItems.Where(d => !dicValue.Keys.Contains(d.I_ControversialActivites_Id)).ToList();

                        foreach (var deletedItem in deletedItems)
                        {
                            _controActivPresetItemsService.Delete(deletedItem);
                        }
                        _controActivPresetItemsService.Save();
                    }
                }
                else //add new preset
                {

                    var newPreset = new I_ControversialActivitesPresets()
                    {
                        Name = model.PresetName,
                        Created = DateTime.UtcNow
                    };

                    _controActivPresetService.Add(newPreset, true);

                    if (dicValue.Count > 0)
                    {
                        foreach (var item in dicValue)
                        {
                            var newPresetItem = new I_ControversialActivitesPresetsItems()
                            {
                                I_ControversialActivites_Id = item.Key,
                                I_ControversialActivitesPresets_Id = newPreset.I_ControversialActivitesPresets_Id,
                                MinimumInvolvment = item.Value,
                                Created = DateTime.UtcNow
                            };
                            _controActivPresetItemsService.Add(newPresetItem);
                        }
                        _controActivPresetItemsService.Save();
                    }
                }

            }
            catch (GesServiceException e)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Exception occurred). Please try again.",
                    error = e.Message
                });
            }

            return Json(new
            {
                success = true,
                editing = updating
            });
        }

        [HttpPost]
        public ActionResult UpdateCompanies(long portfolioId, bool newValue, long companyId)
        {
            bool result;
            try
            {
                if (newValue)
                {
                    _portfolioCompaniesService.Add(new I_PortfoliosI_Companies { I_Portfolios_Id = portfolioId, I_Companies_Id = companyId });
                    result = _portfolioCompaniesService.Save() > 0;
                }
                else
                {
                    result = _portfolioCompaniesService.RemovePortfolioCompaniesByPortfolioIdAndCompanyID(portfolioId, companyId);
                }
                UpdateLastUpdateForPortfolio(portfolioId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, "Exception when update the company.");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateShowInClient(bool newValue, long companyId)
        {
            bool result = false;
            try
            {
                var company = _companiesService.GetCompaniesByListIds(new List<long>() { companyId }).FirstOrDefault();

                if (company != null)
                {
                    company.ShowInClient = newValue;

                    _companiesService.Update(company);
                    result = _companiesService.Save() > 0;
                }
            }
            catch (GesServiceException e)
            {
                Logger.Error("Error when update show in client value.");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGESStandardUniverse(long portfolioId, bool newValue)
        {
            bool result;
            try
            {
                var portfolioItem = _portfolioService.GetById(portfolioId);
                if (portfolioItem != null)
                {
                    portfolioItem.GESStandardUniverse = newValue;
                    portfolioItem.LastUpdated = DateTime.UtcNow;

                    _portfolioService.Update(portfolioItem);
                    result = _portfolioService.Save() > 0;
                }
                else
                {
                    result = false;
                }
            }
            catch (GesServiceException e)
            {
                Logger.Error("Error when update show in client value.");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        //long portfolioId
        public ActionResult DeletePortfolios(List<long> ids)
        {
            var orgId = Request.QueryString["orgid"];
            long orgIdInt;
            long organizationId = -1;
            if (Int64.TryParse(orgId, out orgIdInt))
            {
                organizationId = orgIdInt;
            }

            var result = this.SafeExecute(() => DeletePortfolios(ids, organizationId), "Exception when delete the portfolio {@id}", ids);

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        //Delete controversial activities preset
        public ActionResult DeleteControActivPresets(List<long> Ids)
        {
            return this.SafeExecute(() =>
            {
                foreach (var presetId in Ids)
                {
                    var preset = _controActivPresetService.GetById(presetId);

                    if (preset != null)
                    {
                        _controActivPresetService.Delete(preset);
                    }
                }

                var result = _controActivPresetService.Save() > 0;

                return Json(new
                {
                    meta = new
                    {
                        success = result,
                        error = ""
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }, "Exception when delete presets");
        }

        #endregion

        #region JsonResult
        [HttpPost]
        public JsonResult GetDataForPortfoliosJqGrid(JqGridViewModel jqGridParams)
        {
            var orgId = Request.QueryString["orgid"];
            long orgIdInt;
            bool result = Int64.TryParse(orgId, out orgIdInt);

            var listPortfolio = this.SafeExecute(() =>
            {
                return result ? _portfolioService.GetGesPortfolios(jqGridParams, orgIdInt)
                                        : _portfolioService.GetGesPortfolios(jqGridParams, -1);
            }, "Exception when getting the portfolio {@JqGridViewModel}", jqGridParams);

            return Json(listPortfolio);
        }

        [HttpPost]
        public JsonResult GetDataForControActivPresets(JqGridViewModel jqGridParams)
        {
            var presets = this.SafeExecute(() => _portfolioService.GetControActivPresets(jqGridParams), "Exception when get preset {@JqGridViewModel}.", jqGridParams);

            return Json(presets);
        }

        [HttpPost]
        public JsonResult GetDataForPortfolioDetails(JqGridViewModel jqGridParams)
        {
            var portfolioIdStr = Request.QueryString["id"];
            long portfolioId;
            bool result = Int64.TryParse(portfolioIdStr, out portfolioId);

            var listCompany = this.SafeExecute(() =>
            {
                return result ? _portfolioService.GetCompanyListForPortfolioDetails(jqGridParams, portfolioId)
                                             : _portfolioService.GetCompanyListForPortfolioDetails(jqGridParams, -1);
            }, "Exception when get preset {@JqGridViewModel}.", jqGridParams);

            return Json(listCompany);
        }

        [HttpPost]
        public JsonResult GetPendingCompanies(JqGridViewModel jqGridParams)
        {
            var portfolioIdStr = Request.QueryString["id"];
            long portfolioId;
            var result = Int64.TryParse(portfolioIdStr, out portfolioId);

            var listCompany = this.SafeExecute(() =>
            {
                return result ? _portfolioService.GetPendingCompanies(jqGridParams, portfolioId)
                                         : _portfolioService.GetPendingCompanies(jqGridParams, -1);
            }, "Exception when get preset {@JqGridViewModel}.", jqGridParams);

            return Json(listCompany);
        }

        [HttpPost]
        public JsonResult ClearPendingCompanies(long portfolioId)
        {
            try
            {
                _portfolioCompaniesImportService.RemovePortfolioCompaniesImportByPortfolioId(portfolioId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Exception when remove the portfolio {portfolioId}");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = true,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearCompanies(long portfolioId)
        {
            try
            {
                _portfolioCompaniesService.RemovePortfolioCompaniesByPortfolioId(portfolioId);
                UpdateLastUpdateForPortfolio(portfolioId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Exception when remove the portfolio {portfolioId}");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = true,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearAll(long portfolioId)
        {
            try
            {
                _portfolioCompaniesService.RemovePortfolioCompaniesByPortfolioId(portfolioId);
                _portfolioCompaniesImportService.RemovePortfolioCompaniesImportByPortfolioId(portfolioId);
                UpdateLastUpdateForPortfolio(portfolioId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Exception when remove the portfolio {portfolioId}");


                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = true,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearControActivThresholds(long portfolioOrgId)
        {
            var result = false;
            try
            {
                result = _portfolioOrganizationControversialService.RemovePortfolioOrgControversialByPortfolioOrgId(portfolioOrgId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Exception when remove the portfolio {portfolioOrgId}");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = "Something went wrong (Invalid model). Please try again."
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetControversialActivitiesGridData(JqGridViewModel jqGridParams)
        {
            var portfolioOrgIdStr = Request.QueryString["po_id"];
            long portfolioOrgId;
            var result = Int64.TryParse(portfolioOrgIdStr, out portfolioOrgId);

            var list = result ? _portfolioService.GetControActivities(jqGridParams, portfolioOrgId)
                                         : _portfolioService.GetControActivities(jqGridParams, -1);

            return Json(list);
        }


        [HttpPost]
        public JsonResult GetControversialPresetGridData(JqGridViewModel jqGridParams)
        {
            var presetIdStr = Request.QueryString["presetId"];
            long presetId;
            var result = Int64.TryParse(presetIdStr, out presetId);

            var list = result ? _controActivPresetService.GetControActivitiesPresetItem(jqGridParams, presetId)
                                         : _controActivPresetService.GetControActivitiesPresetItem(jqGridParams, -1);

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetStandardPortfoliosForAutocomplete(string term, int limit)
        {
            var standardPortflios = _portfolioService.GetPortfoliosWithTerm(term, limit, true);

            return Json(new
            {
                total = standardPortflios.Count,
                rows = standardPortflios
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetControActivPresetsForAutocomplete(string term, int limit)
        {
            var presets = _controActivPresetService.GetPresetsWithTerm(term, limit);

            return Json(new
            {
                total = presets.Count,
                rows = presets
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ExcelResult
        private static readonly string[] HeadersPendingSecurities = {
                "Id", "Name", "ISIN", "Sedol", "MasterCompanyId", "Not Screened?"
            };

        private static readonly string[] HeadersGesStandardUniverse = {
                "CompanyId", "Name", "ISIN", "Sedol", "Country", "SectorCode", "Sector", "IndustryGroup", "Industry"
            };

        private static readonly DataForExcel.DataType[] ColunmTypesPendingSecurities = {
                DataForExcel.DataType.Integer,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.Integer,
                DataForExcel.DataType.String
            };
        private static readonly DataForExcel.DataType[] ColunmTypesGesStandardUniverse = {
                DataForExcel.DataType.Integer,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String,
                DataForExcel.DataType.String
            };

        [HandleJsonException]
        public ActionResult ExportPendingListToExcel()
        {
            var portfolioIdStr = Request.QueryString["id"];
            long portfolioId;
            var result = Int64.TryParse(portfolioIdStr, out portfolioId);

            if (!result)
                return new EmptyResult();

            var portfolio = _portfolioService.GetById(portfolioId);
            if (portfolio == null)
                return new EmptyResult();

            var filename = string.Format("[Pending-Securities].{0}", UtilHelper.CreateSafeNameNoSpace(portfolio.Name));

            var pendingList = _portfolioService.GetPendingCompanies(portfolioId).ToList();
            var data = pendingList.Select(item => new[]
            {
                item.Id.ToString(),
                item.Name,
                item.Isin,
                item.Sedol,
                item.MasterCompanyId.ToString(),
                item.Screened.ToString()
            }).ToList();

            return new ExcelResult(HeadersPendingSecurities, ColunmTypesPendingSecurities, data,
                                    string.Format("{0}.xlsx", filename), "Pending Securities");
        }

        [HandleJsonException]
        public ActionResult ExportGESStandardUniverseToExcel(bool fullList = false)
        {
            var filename = string.Format(ExcelTemplates.GesStandardUniversePrefix + "{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var templatePath = Server.MapPath(ExcelTemplates.GesStandardUniverse);

            if (fullList)
            {
                filename = "[FULL]." + filename;
                templatePath = Server.MapPath(ExcelTemplates.FullGesStandardUniverse);
            }

            var standardUniverses = _portfolioService.GetGesStandardUniverseCompanies(fullList);

            var pendingList = _portfolioService.GetGesStandardUniversePendingCompanies();

            var template = System.IO.File.ReadAllBytes(templatePath);
            using (var ms = new MemoryStream(template))
            {
                ms.Position = 0;
                HttpContext.Response.ClearContent();
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                HttpContext.Response.ContentType = "application/octet-stream";
                using (var document = MvcApplication.TemplaterFactory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                {
                    document.Process(new { Companies = standardUniverses, Pending = pendingList });
                }
                HttpContext.Response.End();
            }
            return null;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Save data from Excel to Database
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        private string SaveImportFromExcelToDatabase(List<I_PortfolioCompaniesImport> excelData, long portfolioId)
        {
            if (!excelData.Any()) return "";
            var listIds = excelData.Where(d=>d.SustainalyticsID != null).Select(d => d.SustainalyticsID.Value).ToList();

            var allComparies = _companiesService.GetCompaniesBySustainalyticsIDList(listIds).ToList();
            var portfolioCompanies = _portfolioCompaniesService.GetPortfolioCompaniesByPortfolioId(portfolioId).ToList();
            var newItems = new List<I_PortfoliosI_Companies>();
            var pendingCompanies = new List<I_PortfolioCompaniesImport>();
            var updateCompanies = new List<I_Companies>();

            foreach (var portfoliocompany in excelData)
            {
                var company = allComparies.FirstOrDefault(d => d.Id == portfoliocompany.SustainalyticsID);

                
                if (company != null)
                {
                    if (!CheckExistPortfolioCompany(portfolioCompanies, company.I_Companies_Id, portfolioId))
                    {
                        newItems.Add(new I_PortfoliosI_Companies
                        {
                            I_Portfolios_Id = portfolioId,
                            I_Companies_Id = company.I_Companies_Id
                        });
                    }
                }
                else
                {
                    pendingCompanies.Add(portfoliocompany);
                }
            }

            //Add to Portfolios_Companies table
            _portfolioCompaniesService.AddPortfolioCompaniesByList(newItems);

            //Add to Temp tablevar
            var invalidSustainalyticsIDsString ="";
            if (pendingCompanies.Any())
            {
                foreach (var newItem in pendingCompanies)
                {
                    if (newItem.SustainalyticsID != null)
                    {
                        invalidSustainalyticsIDsString += newItem.SustainalyticsID + "; ";
                    }
                    
                }

                UpdateLastUpdateForPortfolio(portfolioId);
            }

            return invalidSustainalyticsIDsString;
        }

        /// <summary>
        /// Process pending companies
        /// </summary>
        /// <param name="pendingCompanies"></param>
        /// <param name="portfolioId"></param>
        /// <param name="addedItems"></param>
        /// <param name="duplicatedItems"></param>
        /// <param name="deletedItems"></param>
        private void ProcessPendingCompanies(List<PendingCompanyViewModel> pendingCompanies, long portfolioId, out int addedItems, out int editedItems, out int duplicatedItems, out int deletedItems)
        {
            addedItems = 0;
            duplicatedItems = 0;
            deletedItems = 0;
            editedItems = 0;
            if (pendingCompanies == null || !pendingCompanies.Any()) return;

            var listIsinCode = pendingCompanies.Select(d => d.Isin).ToList();
            var existComparies = _companiesService.GetCompaniesByListIsin(listIsinCode).ToList();
            var portfolioCompanies = _portfolioCompaniesService.GetPortfolioCompaniesByPortfolioId(portfolioId).ToList();

            var listMasterCompanyId = pendingCompanies.Where(d => d.MasterCompanyId != null).Select(d => d.MasterCompanyId.Value).ToList();
            var listMasterCompanies = _companiesService.GetCompaniesByListIds(listMasterCompanyId).ToList();

            var addCompanies = new List<I_Companies>();
            var updateCompanies = new List<I_Companies>();
            var addPortfolioCompanies = new List<I_PortfoliosI_Companies>();

            var listToAdd = pendingCompanies.Where(d => d.SelectedToAdd).ToList();

            var lastGeneratedCustomIsin = _companiesService.GetMaximumCustomIsinCode();

            //process add to portfolio company
            foreach (var pendingCompany in listToAdd)
            {
                var existCompany = existComparies.FirstOrDefault(d => d.Isin.Equals(pendingCompany.Isin, StringComparison.OrdinalIgnoreCase));
                //if company is exist in database => get companyId and add to portfolioCompanies
                if (existCompany != null)
                {
                    if (!CheckExistPortfolioCompany(portfolioCompanies, existCompany.I_Companies_Id, portfolioId))
                    {
                        addPortfolioCompanies.Add(new I_PortfoliosI_Companies
                        {
                            I_Companies_Id = existCompany.I_Companies_Id,
                            I_Portfolios_Id = portfolioId
                        });
                    }
                    else
                    {
                        duplicatedItems++;
                    }

                    //update master company for exist company
                    if (existCompany.MasterI_Companies_Id == null && pendingCompany.MasterCompanyId != null)
                    {
                        if (pendingCompany.MasterCompanyId != null)//validate master companyId
                        {
                            var masterCompany = listMasterCompanies.FirstOrDefault(d => d.I_Companies_Id == pendingCompany.MasterCompanyId);
                            if (masterCompany != null && masterCompany.MasterI_Companies_Id == null && existCompany.I_Companies_Id != masterCompany.I_Companies_Id)
                            {
                                existCompany.MasterI_Companies_Id = masterCompany.I_Companies_Id;
                                updateCompanies.Add(existCompany);
                            }
                            else
                            {
                                pendingCompany.SelectedToAdd = false;
                            }
                        }
                    }

                    //update ShowInclient = true
                    if (!existCompany.ShowInClient && !updateCompanies.Contains(existCompany))
                    {
                        existCompany.ShowInClient = true;
                        updateCompanies.Add(existCompany);
                    }
                }
                else// if company is not exist in database => add new company
                {
                    long? masterCompanyId = null;
                    if (pendingCompany.MasterCompanyId != null)//validate master companyId
                    {
                        var masterCompany = listMasterCompanies.FirstOrDefault(d => d.I_Companies_Id == pendingCompany.MasterCompanyId);
                        if (masterCompany != null && masterCompany.MasterI_Companies_Id == null)
                        {
                            masterCompanyId = masterCompany.I_Companies_Id;
                        }
                        else
                        {
                            pendingCompany.SelectedToAdd = false;
                            continue;
                        }
                    }

                    // generate ISIN
                    var generatedIsin = string.IsNullOrEmpty(pendingCompany.Isin) ? "" : pendingCompany.Isin.ToUpper();
                    if (generatedIsin == "")
                    {
                        generatedIsin = IsinHelper.GenerateIsinCode(lastGeneratedCustomIsin);
                        lastGeneratedCustomIsin = generatedIsin;
                    }
                    addCompanies.Add(new I_Companies
                    {
                        Isin = generatedIsin,
                        OtherName = pendingCompany.Name == null ? "N/A" : pendingCompany.Name.Trim(),
                        Created = DateTime.UtcNow,
                        MasterI_Companies_Id = masterCompanyId,
                        ShowInClient = true,
                        Gri = false,
                        ApprovedGesCompany = true
                    });
                }
            }

            if (addCompanies.Any())
            {
                addCompanies = addCompanies.GroupBy(i => i.Isin).Select(g => g.First()).ToList();
                foreach (var company in addCompanies)
                {
                    _companiesService.Add(company);
                }
                if (_companiesService.Save() > 0)
                {
                    //after save company we have companyId
                    foreach (var company in addCompanies)
                    {
                        addPortfolioCompanies.Add(new I_PortfoliosI_Companies { I_Companies_Id = company.I_Companies_Id, I_Portfolios_Id = portfolioId });
                    }
                }
            }

            //update exist companies
            if (updateCompanies.Any())
            {
                foreach (var company in updateCompanies)
                {
                    _companiesService.Update(company);
                }
                _companiesService.Save();
            }

            //save to I_PortfoliosI_Companies table
            addedItems = _portfolioCompaniesService.AddPortfolioCompaniesByList(addPortfolioCompanies);

            //Delete in temp import table
            var listId = pendingCompanies.Where(d => d.SelectedToAdd || d.SelectedToDelete).Select(d => d.Id).ToList();
            _portfolioCompaniesImportService.RemovePortfolioCompaniesImportByListId(listId);

            deletedItems = pendingCompanies.Count(d => d.SelectedToDelete);

            //save data if can not add
            var listModified = pendingCompanies.Where(d => d.SelectedToAdd == false && d.SelectedToDelete == false).ToList();
            UpdateImportedCompanies(listModified);
            editedItems = listModified.Count;

            UpdateLastUpdateForPortfolio(portfolioId);
        }

        /// <summary>
        /// Process pending companies
        /// </summary>
        /// <param name="controActivities"></param>
        /// <param name="portfolioOrgId"></param>
        /// <param name="addedItems"></param>
        /// <param name="editedItems"></param>
        /// <param name="deletedItems"></param>
        private void ProcessControActivities(List<PortfolioControActivityViewModel> controActivities, long portfolioOrgId, bool isApplyingPreset, bool isApplyPreset_Overwrite, out int addedItems, out int editedItems, out int deletedItems)
        {
            addedItems = 0;
            editedItems = 0;
            deletedItems = 0;

            if (controActivities == null || !controActivities.Any()) return;

            var listExistingRecords = _portfolioOrganizationControversialService.GetByPortfolioOrgId(portfolioOrgId);
            var listExistingRecords_ControActivId = listExistingRecords.Select(i => i.I_ControversialActivites_Id);

            foreach (var item in controActivities)
            {
                try
                {
                    if (item.Threshold == null && !isApplyingPreset) // delete
                    {
                        var toDelete =
                            listExistingRecords.FirstOrDefault(i => i.I_ControversialActivites_Id == item.ControActivId);
                        if (toDelete != null)
                        {
                            _portfolioOrganizationControversialService.Delete(toDelete);
                            var result = _portfolioOrganizationControversialService.Save();
                            if (result > 0)
                            {
                                deletedItems++;
                            }
                        }
                    }
                    else // add or update
                    {
                        if (!listExistingRecords_ControActivId.Contains(item.ControActivId)) // add
                        {
                            var toAdd = new I_PortfoliosG_OrganizationsI_ControversialActivites()
                            {
                                I_PortfoliosG_Organizations_Id = portfolioOrgId,
                                I_ControversialActivites_Id = item.ControActivId,
                                MinimumInvolvment = (short)item.Threshold
                            };
                            _portfolioOrganizationControversialService.Add(toAdd);
                            var result = _portfolioOrganizationControversialService.Save();
                            if (result > 0)
                            {
                                addedItems++;
                            }
                        }
                        else // update
                        {
                            var toUpdate =
                            listExistingRecords.FirstOrDefault(i => i.I_ControversialActivites_Id == item.ControActivId);
                            if (toUpdate != null)
                            {
                                if ((isApplyingPreset && isApplyPreset_Overwrite) || !isApplyingPreset)
                                {
                                    var convertedThreshold = (short)item.Threshold;
                                    if (convertedThreshold == toUpdate.MinimumInvolvment)
                                    {
                                        // no need to update
                                    }
                                    else
                                    {
                                        toUpdate.MinimumInvolvment = convertedThreshold;
                                        _portfolioOrganizationControversialService.Update(toUpdate);
                                        var result = _portfolioOrganizationControversialService.Save();
                                        if (result > 0)
                                        {
                                            editedItems++;
                                        }
                                    }
                                }
                                else
                                {
                                    // do nothing
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void UpdateImportedCompanies(List<PendingCompanyViewModel> listCompanyImported)
        {
            if (listCompanyImported.Any())
            {
                var importUpdate = _portfolioCompaniesImportService.GetPortfolioCompaniesImportByListId(
                    listCompanyImported.Select(d => d.Id).ToList());

                foreach (var portfolioImport in importUpdate)
                {
                    var adjustedata = listCompanyImported.FirstOrDefault(d => d.Id == portfolioImport.I_PortfolioCompaniesImport_Id);
                    if (adjustedata != null)
                    {
                        portfolioImport.Isin = adjustedata.Isin;
                        portfolioImport.MasterI_Companies_Id = adjustedata.MasterCompanyId;
                        portfolioImport.OtherName = adjustedata.Name;
                        portfolioImport.Sedol = adjustedata.Sedol;
                        portfolioImport.Screened = adjustedata.Screened;
                        _portfolioCompaniesImportService.Update(portfolioImport);
                    }
                }
                _portfolioCompaniesImportService.Save();
            }
        }

        /// <summary>
        /// check portfolio exist in list
        /// </summary>
        /// <param name="existPortfoliosICompanieses"></param>
        /// <param name="companyId"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        private bool CheckExistPortfolioCompany(List<I_PortfoliosI_Companies> existPortfoliosICompanieses, long companyId, long portfolioId)
        {
            return existPortfoliosICompanieses.Any(d => d.I_Companies_Id == companyId && d.I_Portfolios_Id == portfolioId);
        }

        /// <summary>
        /// Delete multi portfolios
        /// </summary>
        /// <param name="portfolioIds"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        private bool DeletePortfolios(List<long> portfolioIds, long organizationId)
        {
            var result = false;
            foreach (var id in portfolioIds)
            {
                result = _portfoliosGOrganizationService.DeletePortfolioOrganization(id, organizationId);
                // if portfolio not use in other Organization => delete in Portfolio table
                if (result && !_portfoliosGOrganizationService.CheckExistPortfolioInPortfolioOrganization(id))
                {
                    var deletePortfolio = _portfolioService.GetById(id);
                    if (deletePortfolio != null && deletePortfolio.StandardPortfolio == false)
                    {
                        _portfolioService.Delete(deletePortfolio);

                        //delete portfolio companies
                        var listDeletedPortfolioCompanies = _portfolioCompaniesService.GetPortfolioCompaniesByPortfolioId(deletePortfolio.I_Portfolios_Id).ToList();

                        foreach (var portfolioCompanies in listDeletedPortfolioCompanies)
                        {
                            _portfolioCompaniesService.Delete(portfolioCompanies);
                        }
                    }
                }
            }
            _portfolioCompaniesService.Save();
            _portfolioService.Save();

            return result;
        }

        private IEnumerable<SelectListItem> GetPortfolioTypes()
        {
            var types = _portfolioService.GetPortfolioTypeItems()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });

            return new SelectList(types, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetPresets()
        {
            var types = _controActivPresetService.GetAll()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.I_ControversialActivitesPresets_Id.ToString(),
                                    Text = x.Name
                                });

            var list = new SelectList(types, "Value", "Text");
            return list;
        }

        private IEnumerable<SelectListItem> GetStandardPortfolios()
        {
            var types = _portfolioService.GetStandardPortfolios()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.I_Portfolios_Id.ToString(),
                                    Text = x.Name
                                });

            var list = new SelectList(types, "Value", "Text");
            return list;
        }

        private void UpdateLastUpdateForPortfolio(long portfolioId)
        {
            var portfolio = _portfolioService.GetById(portfolioId);
            if (portfolio != null)
            {
                portfolio.LastUpdated = DateTime.UtcNow;
                _portfolioService.Update(portfolio,true);
            }
        }

        #endregion

    }
}