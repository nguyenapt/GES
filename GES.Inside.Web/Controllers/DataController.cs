using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FileHelpers;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Helpers;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Web.Controllers
{
    public class DataController : GesControllerBase
    {
        private II_CompaniesService _companiesService;

        public DataController(IGesLogger logger, II_CompaniesService companiesService)
            : base(logger)
        {
            _companiesService = companiesService;
        }

        [CustomAuthorize(FormKey = "DataUtility", Action = ActionEnum.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EdiSync()
        {
            var file = Request.Files["txtfileinput"];

            if (file != null && file.ContentLength > 0)
            {
                var allCompanies = _companiesService.GetAllMasterCompanies().ToList();

                var allCountries = _companiesService.AllCountries();

                var engine = new FileHelperAsyncEngine<Company>();

                using (engine.BeginReadStream(new StreamReader(file.InputStream)))
                {
                    try
                    {
                        foreach (Company company in engine)
                        {
                            if (string.IsNullOrEmpty(company.ISIN)) continue;


                            var countryIncId =
                                allCountries.FirstOrDefault(
                                    d =>
                                        d.Alpha3Code != null && company.Country_Of_Inc != null &&
                                        d.Alpha3Code.Equals(company.Country_Of_Inc));

                            var existCompany =
                                allCompanies.FirstOrDefault(
                                    c =>
                                        c.MasterI_Companies_Id == null && c.Isin != null &&
                                        c.Isin.Equals(company.ISIN, StringComparison.OrdinalIgnoreCase));

                            if (existCompany != null &&
                                ((countryIncId != null &&
                                  countryIncId.Id != existCompany.CountryOfIncorporationId) ||
                                  existCompany.MsciName != company.Issuer_Name
                                  ))
                            {
                                existCompany.CountryOfIncorporationId = countryIncId?.Id;
                                existCompany.MsciName = company.Issuer_Name;
                                existCompany.IssuerName = company.Issuer_Name;

                                _companiesService.Update(existCompany);
                            }
                        }

                        _companiesService.Save();
                        return Json(new
                        {
                            success = true,
                            message = "Sync completed."
                        });
                    }
                    catch (Exception ex)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Exception occurred: " + ex.Message,
                            error = ex.Message
                        });
                    }
                }
            }

            return Json(new
            {
                success = false,
                message = "No File Selected."
            });
        }

        [HttpPost]
        public JsonResult MarketCap()
        {
            var file = Request.Files["marketCapFileInput"];
            var allCompanies = _companiesService.GetAll().ToList();

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var excelHelper = new ExcelReaderHelpers();

                var columnConfigs =
                    excelHelper.GetImportExcelConfigs(Server.MapPath("~/Configs/ExcelImport/ExcelMappingColumn.xml"), "ImportMarketCap");

                var rows = excelHelper.GetExcelReaderData(string.Empty, file.InputStream, fileExtension);

                var listData = new List<ImportMarketCap>();

                foreach (var rowItem in rows)
                {
                    var companyMarketCap = new ImportMarketCap();
                    foreach (var config in columnConfigs)
                    {
                        companyMarketCap.SetValueByPropertyName(excelHelper.GetRowValue(rowItem, config.ExcelField, config.DataType), config.DatabaseField);
                    }
                    if (!string.IsNullOrWhiteSpace(companyMarketCap.Isin)
                        && listData.All(d => !d.Isin.Equals(companyMarketCap.Isin, StringComparison.OrdinalIgnoreCase)))
                    {
                        listData.Add(companyMarketCap);
                    }
                }

                const int blockSize = 500;
                var start = 0;
                var end = start + blockSize > listData.Count - 1 ? listData.Count - 1 : start + blockSize; 
                while (start <= end)
                {
                    var importMarketCap = listData[start];
                    var company = allCompanies.FirstOrDefault(
                            d => d.Isin != null && d.Isin.Equals(importMarketCap.Isin, StringComparison.OrdinalIgnoreCase));

                    if (company != null)
                    {
                        company.MarketCap = importMarketCap.MarketCap;
                        _companiesService.Update(company, false);
                    }

                    start++;
                    if (start > end)
                    {
                        _companiesService.Save();
                        end = start + blockSize > listData.Count - 1 ? listData.Count - 1 : start + blockSize;
                    }
                }
                
                return Json(new
                {
                    success = true,
                    message = "Sync completed."
                });
            }


            return Json(new
            {
                success = false,
                message = "No File Selected."
            });
        }
    }

    [DelimitedRecord("\t")]
    public class Company
    {
        public string SEDOL { get; set; }
        public string Description { get; set; }
        public string Issuer_Name { get; set; }
        public string Cntry_Of_Inc { get; set; }
        public string ISIN { get; set; }
        public string OPOL { get; set; }
        public string Status { get; set; }
        public string TIDM { get; set; }
        public string PrimaryListing { get; set; }
        public string IssuerID { get; set; }
        public string SecurityID { get; set; }
        public string Country_Of_Inc { get; set; }
        public string Issuer_Status { get; set; }
        public string Security_Type { get; set; }
        public string Country_of_Register { get; set; }
        public string Underlying_Issuer { get; set; }
        public string Security_CAType { get; set; }

    }

    public class ImportMarketCap
    {
        public string Isin { get; set; }

        public Decimal MarketCap { get; set; }

    }

}
