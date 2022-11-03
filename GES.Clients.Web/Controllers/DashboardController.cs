using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Helpers;
using GES.Clients.Web.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using GES.Common.Logging;
using GES.Clients.Web.Extensions;
using GES.Common.Resources;
using GES.Inside.Data.Models.Anonymous;
using GES.Common.Exceptions;
using GES.Clients.Web.Configs;

namespace GES.Clients.Web.Controllers
{
    public class DashboardController : GesControllerBase
    {
        #region Declaration
        private readonly IDashboardService _dashboardService;
        private readonly II_CompaniesService _companiesService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly IPersonalSettingsService _personalSettingsService;
        private readonly I_GesAnnouncementService _gesAnnouncementService;

        private const long BussinessConductServiceId = 30;
        #endregion

        #region Constructor
        public DashboardController(IGesLogger logger, IDashboardService dashboardService, II_CompaniesService companiesService,
            IG_IndividualsService gIndividualsService, IPersonalSettingsService personalSettingsService, I_GesAnnouncementService gesAnnouncementService)
            : base(logger)
        {
            _companiesService = companiesService;
            _gIndividualsService = gIndividualsService;
            _dashboardService = dashboardService;
            _personalSettingsService = personalSettingsService;
            _gesAnnouncementService = gesAnnouncementService;
        }

        #endregion

        public ActionResult Index()
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);
            var model = new DashboardViewModel()
            {
                Portfolios = GetPortfolios(orgId, individualId),
                Indices = GetIndices(orgId, individualId),
                AnnouncementModels = _gesAnnouncementService.GetModels(),
                BlogModels = _dashboardService.GetBlogModels(SiteSettings.BlogFeedUrl)
            };
            
            //var rssAnnouncementItems = _gesAnnouncementService.GetRssModels(SiteSettings.AnnouncementFeedUrl);

            //if (rssAnnouncementItems != null && rssAnnouncementItems.Any())
            //{
            //    if (model.AnnouncementModels == null)
            //        model.AnnouncementModels = new List<GesAnnouncementModel>();

            //    foreach(var rssItem in rssAnnouncementItems)
            //    {
            //        model.AnnouncementModels.Add(rssItem);
            //    }

            //    model.AnnouncementModels = model.AnnouncementModels.OrderByDescending(x => x.AnnouncementDate).ToList();
            //}

            // Pre-load functions
            this.SafeExecute(() => PreloadAutocomplete(orgId), $"Exception when pre-loading the auto-complete value of organization({orgId})");

            ViewBag.IndividualId = individualId;
            ViewBag.OrgId = orgId;

            var searchName = string.Empty;

            var countries = string.Empty;
            var homeCountries = string.Empty;
            var recommendationIds = string.Empty;
            var portfolioIds = string.Empty;
            var normAreaIds = string.Empty;
            var serviceIds = string.Empty;
            var sectorIds = string.Empty;
            var industryGroupIds = string.Join(",", _companiesService.GetIndustryGroupIdsFromSectorIds(sectorIds));
            var continentIds = string.Empty;
            countries = string.IsNullOrEmpty(countries)
                ? string.Join(",", _companiesService.GetCountryCodesFromContinentIds(continentIds))
                : countries;

            var boxViewModel = new CompanySearchBoxViewModel
            {
                PortfolioIndexes = GetPortfolioIndexDropdown(orgId, portfolioIds),
                ShowClosedCases = true,
                OnlyCompaniesWithActiveCases = false,
                Recommendations = GetRecommendationDropdown(recommendationIds),
                Conclusions = _companiesService.GetConclusions().Select(ToSelectListItem),
                EngagementAreas = GetEngagementAreaDropdown(serviceIds, normAreaIds, orgId),
                Locations = GetLocationDropdown(countries),
                Responses = _companiesService.GetResponses().Select(ToSelectListItem),
                Progresses = _companiesService.GetProgresses().Select(ToSelectListItem),
                Industries = GetIndustryGroupDropdown(industryGroupIds),
                Name = searchName,
                //onlySearchCompany = !string.IsNullOrEmpty(searchName),
                onlySearchCompany = false,
                onlyShowFocusList = false,
                HomeCountries = GetHomeCountriesDropdown(homeCountries),
                ClientType = _companiesService.GetClientType(orgId)
            };

            model.CompanySearchBox = boxViewModel;

            return View(model);
        }

        private SelectListItem ToSelectListItem(IdNameModel item)
        {
            return new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            };
        }

        private IEnumerable<SelectListItem> GetPortfolioIndexDropdown(long orgId, string portfolioIds)
        {
            Guard.AgainstNullArgument(nameof(portfolioIds), portfolioIds);

            var ids = portfolioIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(long.Parse);

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetPortfolioIndexes(orgId).ToList();
                var items = all.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
                var selected = all.Where(i => ids.Contains(i.Id)).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Exception when getting the portfolio index of Organization: {orgId}.");
        }

        private IEnumerable<SelectListItem> GetRecommendationDropdown(string recommendationIds)
        {
            Guard.AgainstNullArgument(nameof(recommendationIds), recommendationIds);

            var ids = recommendationIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(long.Parse);

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetRecommendations().ToList();
                var items = all.Select(ToSelectListItem);
                var selected = all.Where(i => ids.Contains(i.Id)).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting the recommendations.");
        }

        private IEnumerable<SelectListItem> GetEngagementAreaDropdown(string serviceIds, string normAreaIds, long orgId)
        {
            Guard.AgainstNullArgument(nameof(serviceIds), serviceIds);
            Guard.AgainstNullArgument(nameof(normAreaIds), normAreaIds);

            // selected: services
            var sIds = serviceIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());
            var selectedServices = sIds.Select(nid => $"{nid}--").ToList();
            // selected: normAreas
            var nIds = normAreaIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());
            var selectedNormAreas = nIds.Select(nid => $"{BussinessConductServiceId}--{nid}").ToList();

            return this.SafeExecute(() =>
            {
                var items = _companiesService.GetEngagmentThemeNorm(orgId);
                var selected = selectedNormAreas;
                if (selectedServices.Count > 0)
                {
                    selected = selectedServices;
                }

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting the Engagement Areas with organization {orgId}.");
        }

        private IEnumerable<SelectListItem> GetLocationDropdown(string cts)
        {
            Guard.AgainstNullArgument(nameof(cts), cts);

            var countries = cts.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllCountries().ToList();
                var items = all.Where(d => d.Name != null); //all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => countries.Contains(i.Alpha3Code)).Select(i => i.Id);

                return new MultiSelectList(items, "Alpha3Code", "Name", selected);
            }, $"Error when getting all countries");
        }

        private IEnumerable<SelectListItem> GetIndustryGroupDropdown(string igs)
        {
            Guard.AgainstNullArgument(nameof(igs), igs);

            var industryGroupIds = igs.Split(',').Where(i => !string.IsNullOrEmpty(i));

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllIndustries().ToList();
                var items = all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => industryGroupIds.Contains(i.Id.ToString())).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting all industries.");
        }

        private IEnumerable<SelectListItem> GetHomeCountriesDropdown(string cts)
        {
            Guard.AgainstNullArgument(nameof(cts), cts);

            var countries = cts.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllCountries().ToList();
                var items = all.Where(d => d.Name != null);  //all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => countries.Contains(i.Alpha3Code)).Select(i => i.Id);

                return new MultiSelectList(items, "Alpha3Code", "Name", selected);
            }, $"Error when getting all countries.");
        }

        public ActionResult Dashboard()
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);
            var model = new DashboardViewModel()
            {
                Portfolios = GetPortfolios(orgId, individualId),
                Indices = GetIndices(orgId, individualId)
            };

            // Pre-load functions
            this.SafeExecute(() => PreloadAutocomplete(orgId), $"Exception when pre-loading the auto-complete value of organization({orgId})");

            ViewBag.IndividualId = individualId;
            ViewBag.OrgId = orgId;
            return View(model);
        }

        #region Preload
        private async Task PreloadAutocomplete(long orgId)
        {
            await Task.Run(() =>
            {
                _companiesService.GetAutoCompleteCompanyIssueName(orgId, "", 20, true, true, true);
            });
        }

        #endregion

        #region Data
        [HttpPost]
        [OutputCache(CacheProfile = "Dashboard12h")]
        [Route("DashboardGetLatestNews/{uniqueKey?}")]
        public ActionResult GetLatestNews(string type, List<long> selectedPortfoliosOrIndices, string uniqueKey, string homepageType = "")
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            if (homepageType == "HOMEPAGE")
            {
                selectedPortfoliosOrIndices = new List<long>();
                switch (type)
                {
                    case "portfolios":
                        var portfolios = GetPortfolioIndices(orgId, individualId);
                        if (portfolios != null && portfolios.Any())
                        {
                            foreach (var porfolio in portfolios)
                            {
                                selectedPortfoliosOrIndices.Add(long.Parse(porfolio.Value));
                            }
                        }
                        break;
                    case "indices":
                        var indices = GetPortfolioIndices(orgId, individualId);
                        if (indices != null && indices.Any())
                        {
                            foreach (var indice in indices)
                            {
                                selectedPortfoliosOrIndices.Add(long.Parse(indice.Value));
                            }
                        }
                        break;
                }
            }

            var newPortfolioSetting = selectedPortfoliosOrIndices != null && selectedPortfoliosOrIndices.Any() && !selectedPortfoliosOrIndices.Contains(-1) ? string.Join(",", selectedPortfoliosOrIndices) : string.Empty;

            this.SafeExecute(() =>
            {
                switch (type)
                {
                    case "portfolios":
                        _personalSettingsService.UpdatePersonalSettingValue(individualId, 1, newPortfolioSetting);
                        break;
                    case "indices":
                        _personalSettingsService.UpdatePersonalSettingValue(individualId, 2, newPortfolioSetting);
                        break;
                }
            }, $"Error when update the personal setting value ({newPortfolioSetting})");

            return PartialView("_LatestNews", _dashboardService.GetGesLastestNews(orgId, individualId, type, selectedPortfoliosOrIndices));
        }

        [HttpPost]
        [Route("DashboardGetLatestMilestones/{uniqueKey?}")]
        public ActionResult GetLatestMilestones(string type, List<long> selectedPortfoliosOrIndices, string uniqueKey)
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            return PartialView("_LatestMilestones", _dashboardService.GetMilestones(orgId, individualId, type, selectedPortfoliosOrIndices));
        }

        [HttpPost]
        [Route("DashboardGetCalendarEvents/{uniqueKey?}")]
        public JsonResult GetCalendarEvents(string type, List<long> selectedPortfoliosOrIndices, string uniqueKey, string homepageType = "")
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            if (homepageType == "HOMEPAGE")
            {
                selectedPortfoliosOrIndices = new List<long>();
                switch (type)
                {
                    case "portfolios":
                        var portfolios = GetPortfolioIndices(orgId, individualId);
                        if (portfolios != null && portfolios.Any())
                        {
                            foreach (var porfolio in portfolios)
                            {
                                selectedPortfoliosOrIndices.Add(long.Parse(porfolio.Value));
                            }
                        }
                        break;
                    case "indices":
                        var indices = GetPortfolioIndices(orgId, individualId);
                        if (indices != null && indices.Any())
                        {
                            foreach (var indice in indices)
                            {
                                selectedPortfoliosOrIndices.Add(long.Parse(indice.Value));
                            }
                        }
                        break;
                }
            }

            return Json(
                new
                {
                    events = _dashboardService.GetCalendarEvents(orgId, individualId, type, selectedPortfoliosOrIndices)
                });
        }

        [HttpPost]
        [OutputCache(CacheProfile = "Dashboard1h")]
        [Route("DashboardGetDoughnutChartData/{uniqueKey?}")]
        public JsonResult GetDoughnutChartData(string chartType, string tab, List<long> selectedPortfoliosOrIndices, string uniqueKey)
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            var rawData = new List<ChartModel>();
            switch (chartType)
            {
                case "recommendation":
                    rawData = _dashboardService.GetRecomendationChart(orgId, individualId, tab, selectedPortfoliosOrIndices);
                    break;
                case "norm":
                    rawData = _dashboardService.GetNormChart(orgId, individualId, tab, selectedPortfoliosOrIndices);
                    break;
                case "sector":
                    rawData = _dashboardService.GetGICSectorChart(orgId, individualId, tab, selectedPortfoliosOrIndices);
                    break;
                case "location":
                    rawData = _dashboardService.GetLocationChart(orgId, individualId, tab, selectedPortfoliosOrIndices);
                    break;
            }

            var rawDataCount = rawData.Count;

            // background colors
            var backgroundColors = new List<string>();
            for (var i = 0; i < rawDataCount; i++)
            {
                backgroundColors.Add(CommonHelper.GetChartColor(i, false));
            }

            // labels
            var labels = rawData.Select(i => i.Heading).ToList();

            // data
            var data = rawData.Select(i => i.NumberCount).ToList();

            // ids
            var ids = rawData.Select(i => i.Id).ToList();

            var datasets = new List<ChartDatasetModel>();
            var dataset = new ChartDatasetModel()
            {
                data = data,
                ids = ids,
                backgroundColor = backgroundColors
            };
            datasets.Add(dataset);

            return Json(
                new
                {
                    labels,
                    datasets
                });
        }

        [HttpPost]
        [OutputCache(CacheProfile = "Dashboard1h")]
        [Route("DashboardGetMapData/{uniqueKey?}")]
        public ActionResult GetMapData(string type, List<long> selectedPortfoliosOrIndices, string uniqueKey)
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            var rawData = _dashboardService.GetCountriesMapData(orgId, individualId, type, selectedPortfoliosOrIndices);

            // prepare data object
            dynamic mapData = new ExpandoObject();
            var mapDataDictionary = (IDictionary<string, object>)mapData;
            foreach (var item in rawData)
            {
                mapDataDictionary.Add(item.Heading, item.NumberCount);
            }

            var jsonResult = JsonConvert.SerializeObject(mapDataDictionary);

            return Content(jsonResult, "application/json");
        }

        [HttpPost]
        [Route("DashboardGetInfoBoxData/{uniqueKey?}")]
        public JsonResult GetInfoBoxData(string type, List<long> selectedPortfoliosOrIndices, string uniqueKey)
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            var data = _dashboardService.GetDasboardInfoBox(orgId, individualId, type, selectedPortfoliosOrIndices);

            return Json(
                new
                {
                    infoBoxData = data
                });
        }

        #endregion

        #region Private methods
        private IEnumerable<SelectListItem> GetPortfolios(long orgId, long individualId)
        {
            var types = _companiesService.GetPortfolioIndexes(orgId).Where(x => !x.Name.Contains("[index]"))
                                         .Select(x => new SelectListItem
                                         {
                                             Value = x.Id.ToString(),
                                             Text = x.Name
                                         });

            var list = new MultiSelectList(types, "Value", "Text", GetSettingValue(individualId, 1));

            return list;
        }

        private IEnumerable<SelectListItem> GetIndices(long orgId, long individualId)
        {
            var indexStr = "[index]";

            var xtl = _companiesService.GetPortfolioIndexes(orgId).ToList();

            var types = _companiesService.GetPortfolioIndexes(orgId).Where(x => x.Name.Contains(indexStr))
                                         .Select(x => new SelectListItem
                                         {
                                             Value = x.Id.ToString(),
                                             Text = x.Name.Replace(indexStr, "").Trim()
                                         });

            return new MultiSelectList(types, "Value", "Text", GetSettingValue(individualId, 2));
        }

        private IEnumerable<SelectListItem> GetPortfolioIndices(long orgId, long individualId)
        {
            var types = _companiesService.GetPortfolioIndexes(orgId)
                                        .Select(x => new SelectListItem
                                        {
                                            Value = x.Id.ToString(),
                                            Text = x.Name
                                        });

            var list = new MultiSelectList(types, "Value", "Text", GetSettingValue(individualId, 1));

            return list;
        }

        private void GetIndividualInfo(ref long individualId, ref long orgId)
        {
            if (Session["OrgId"] == null || Session["IndividualId"] == null)
            {
                var usermager = HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(User.Identity.GetUserId());
                long oldId = currentUser?.OldUserId ?? -1;

                var individual = _gIndividualsService.GetIndividualByUserId(oldId);

                Session["IndividualId"] = individualId = individual?.G_Individuals_Id ?? -1;
                Session["OrgId"] = orgId = individual?.G_Organizations_Id ?? -1;
            }
            else
            {
                individualId = (long)Session["IndividualId"];
                orgId = (long)Session["OrgId"];
            }
        }

        private List<long> GetSettingValue(long individualId, long categoryId)
        {
            var result = new List<long>();
            var settingValue = _personalSettingsService.GetPersonalSettingValue(individualId, categoryId);

            if (!string.IsNullOrEmpty(settingValue))
            {
                result = settingValue.Split(',').Select(long.Parse).ToList();
            }

            return result;
        }
        #endregion
    }
}