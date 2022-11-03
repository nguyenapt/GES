using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
using GES.Inside.Web.Configs;
using GES.Inside.Web.Extensions;
using GES.Inside.Web.Helpers;
using GES.Inside.Web.Models;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Web.Controllers
{
    public class ExportController : GesControllerBase
    {
        #region Declaration

        private readonly IOrganizationService _organizationService;
        private readonly IPortfolioService _portfolioService;
        private readonly IExportExcelService _exportExcelService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_CompaniesService _companiesService;
        
        
        private readonly IG_OrganizationsRepository _gOrganizationsRepository;
        private readonly II_CompaniesService _companyService;
        private readonly II_ServicesService _servicesService;
        private readonly IG_IndividualsRepository _gIndividualsRepository;
        private readonly IG_OrganizationsG_ServicesService _gOrganizationsGServicesService;
        private readonly IG_OrganizationsG_ServicesRepository _gOrganizationsGServicesRepository;
        private readonly IGesUserService _gesUserService;
        
        

        #endregion

        #region Constructor

        public ExportController(IGesLogger logger, IOrganizationService organizationService,
            IG_OrganizationsRepository gOrganizationsRepository,
            II_CompaniesService companyService, II_ServicesService servicesService,
            IG_IndividualsRepository gIndividualsRepository,
            IG_OrganizationsG_ServicesService gOrganizationsGServicesService,
            IG_OrganizationsG_ServicesRepository gOrganizationsGServicesRepository, 
            IGesUserService gesUserService, IPortfolioService portfolioService,
            IExportExcelService exportExcelService,
            IG_IndividualsService gIndividualsService,
            II_CompaniesService companiesService)
            : base(logger)
        {
            _organizationService = organizationService;
            _portfolioService = portfolioService;
            _gIndividualsService = gIndividualsService;
            _companiesService = companiesService;
            
            _gOrganizationsRepository = gOrganizationsRepository;
            _companyService = companyService;
            _servicesService = servicesService;
            _gIndividualsRepository = gIndividualsRepository;
            _gOrganizationsGServicesService = gOrganizationsGServicesService;
            _gesUserService = gesUserService;
            _gOrganizationsGServicesRepository = gOrganizationsGServicesRepository;
            _exportExcelService = exportExcelService;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ExportScreeningReport", Action = ActionEnum.Read)]
        public ActionResult ScreeningReport()
        {

            ViewBag.Title = "Screening Report";
            ViewBag.NgController = "ScreeningReportsController";

            return View();
        }


        #endregion

        #region JsonResult

        [HttpGet]
        public JsonResult GetClients()
        {
            var listClient = this.SafeExecute(() => _organizationService.GetAllClients(),
                "Error when getting the clients");

            return Json(listClient, JsonRequestBehavior.AllowGet);
        }        
        
        [HttpGet]
        public JsonResult GetPortfolioIndexByClientId(long clientId)
        {
            var portfolios = this.SafeExecute(() => _portfolioService.GetGesPortfoliosByClientId(clientId),
                "Error when getting portfolios with the client");

            return Json(portfolios, JsonRequestBehavior.AllowGet);
        }

        [HandleJsonException]
        public ActionResult ScreeningReportExport(long clientId, string portfolioIdsString, DateTime? fromDate,
            DateTime? toDate, string normThemeIdsString)
        {

            var portfolioIds = new List<long>(portfolioIdsString.Split(',').Select(long.Parse).ToArray());
            var normThemeIds = new List<long>(normThemeIdsString.Split(',').Select(long.Parse).ToArray());

            var tempalteFilePath = ExcelTemplates.ScreeningReport;
            var screeningReportPrefix = ExcelTemplates.ScreeningReportPrefix;

            if (normThemeIds.Count == 1 && normThemeIds[0] == (long) ScreeningNormTheme.GlobalEthicalStandard)
            {
                tempalteFilePath = ExcelTemplates.GlobalEthicalStandardScreeningReport;
                screeningReportPrefix = ExcelTemplates.GlobalEthicalStandardScreeningReportPrefix;
            }

            var result = new ScreeningReportInputData();

            foreach (var portfolioId in portfolioIds)
            {

                var portfolioResult = _exportExcelService
                    .ExportScreeningReportToExcel(clientId, portfolioId, fromDate, toDate)
                    .ToList();

                if (portfolioResult.Count <= 0) continue;
                var portfolios = new ScreeningReportInfoViewModel
                {
                    name = RemoveSpecialCharacters(portfolioResult[0].PortfolioName),
                    cs = portfolioResult
                };
                result.portfolios.Add(portfolios);
            }

            var duplicatedName = from p in result.portfolios group p by p.name into g where g.Count() > 1 select g.Key;
            
            var nameValues = duplicatedName as string[] ?? duplicatedName.ToArray();

            foreach (var nameValue in nameValues)
            {
                var indexSheet = 1;
                foreach (var p in result.portfolios)
                {
                    if (!nameValue.Equals(p.name)) continue;
                    p.name = p.sheetName() + "-" + indexSheet;
                    indexSheet++;
                }
            }

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var companies = _exportExcelService.GetCompaniesByPortfolioIdsListView(orgId, portfolioIds, fromDate).OrderBy(x => x.CompanyIssueName);
            
            var templatePath = Server.MapPath(tempalteFilePath);
            var template = System.IO.File.ReadAllBytes(templatePath);
            var filename = string.Format(screeningReportPrefix + "{0}.xlsx",
                DateTime.Now.ToString("yyyyMMddHHmmss"));

            var changeSinceValue = fromDate?.ToString(Configurations.DateFormat);
            try
            {
                using (var ms = new MemoryStream(template))
                {
                    ms.Position = 0;
                    HttpContext.Response.ClearContent();
                    HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    HttpContext.Response.ContentType = "application/vnd.ms-excel";
          
                    using (var document =
                        NGS.Templater.Configuration.Factory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                    {
                        
                        document.Process(new {CP = result, Companies = companies, ChangedSice = changeSinceValue});
                    }

                    HttpContext.Response.End();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }


        [HandleJsonException]
        [CustomAuthorize(FormKey = "ExportEFStatusReport", Action = ActionEnum.Read)]
        public ActionResult ExportEFStatusReport()
        {
            var data = _exportExcelService.ExportEFStatusReport().OrderBy(d=>d.CompanyName);
            var tempalteFilePath = ExcelTemplates.EFStatusReport;

            var templatePath = Server.MapPath(tempalteFilePath);
            var screeningReportPrefix = ExcelTemplates.EFStatusReportPrefix;

            var template = System.IO.File.ReadAllBytes(templatePath);
            var filename = string.Format(screeningReportPrefix + "{0}.xlsx",
                DateTime.Now.ToString("yyyyMMddHHmmss"));

            try
            {
                using (var ms = new MemoryStream(template))
                {
                    ms.Position = 0;
                    HttpContext.Response.ClearContent();
                    HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    HttpContext.Response.ContentType = "application/vnd.ms-excel";

                    using (var document =
                        NGS.Templater.Configuration.Factory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                    {

                        document.Process(new { Data = data});
                    }

                    HttpContext.Response.End();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }

        #endregion

        #region Private methods

        private string RemoveSpecialCharacters(string value)
        {
            return Regex.Replace(value, @"(\s+|@|&|'|\(|\)|<|>|]|\[|#)", "");
        } 

        #endregion

    }
}