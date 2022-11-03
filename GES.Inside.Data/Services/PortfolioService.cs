using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using com.sun.xml.@internal.bind.v2;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services
{
    public class PortfolioService: EntityService<GesEntities, I_Portfolios>, IPortfolioService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfolioRepository _portfolioRepository;

        public PortfolioService(IUnitOfWork<GesEntities> unitOfWork, II_PortfolioRepository portfolioRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _portfolioRepository = portfolioRepository;
        }

        public PaginatedResults<PortfolioViewModel> GetGesPortfolios(JqGridViewModel jqGridParams, long orgId)
        {
            var query = from o in _dbContext.I_PortfoliosG_Organizations
                        join p in _dbContext.I_Portfolios on o.I_Portfolios_Id equals p.I_Portfolios_Id
                        join t in _dbContext.I_PortfolioTypes on p.I_PortfolioTypes_Id equals t.I_PortfolioTypes_Id
                        select new PortfolioViewModel()
                        {
                            PortfolioOrganizationId = o.I_PortfoliosG_Organizations_Id,
                            PortfolioId = o.I_Portfolios_Id,
                            Name = p.Name,
                            Type = t.Name,
                            OrganizationId = o.G_Organizations_Id,
                            IncludeInAlerts = o.IncludeInAlerts,
                            ShowInCSC = p.ShowInCSC,
                            StandardPortfolio = p.StandardPortfolio,
                            Created = p.Created,
                            Companies = (from c in _dbContext.I_PortfoliosI_Companies where c.I_Portfolios_Id == p.I_Portfolios_Id select c.I_PortfoliosI_Companies_Id).Count(),
                            ControversialActivities = (from ca in _dbContext.I_PortfoliosG_OrganizationsI_ControversialActivites where ca.I_PortfoliosG_Organizations_Id == o.I_PortfoliosG_Organizations_Id select ca.I_PortfoliosG_OrganizationsI_ControversialActivites_Id).Count(),
                            GEServices = o.I_PortfoliosG_OrganizationsG_Services.Count(),
                            GEServiceIds = o.I_PortfoliosG_OrganizationsG_Services.Select(d => d.G_Services_Id),
                            Clients = 0,
                            GESStandardUniverse = p.GESStandardUniverse
                        };

            if (orgId <= 0)
            {
                query = from p in _dbContext.I_Portfolios
                        join g in _dbContext.G_Organizations on p.G_Organizations_Id equals g.G_Organizations_Id
                        join t in _dbContext.I_PortfolioTypes on p.I_PortfolioTypes_Id equals t.I_PortfolioTypes_Id
                        select new PortfolioViewModel()
                        {
                            PortfolioId = p.I_Portfolios_Id,
                            Name = p.Name,
                            Type = t.Name,
                            OrganizationId = g.G_Organizations_Id,
                            ShowInCSC = p.ShowInCSC,
                            StandardPortfolio = p.StandardPortfolio,
                            Created = p.Created,
                            Companies = (from c in _dbContext.I_PortfoliosI_Companies where c.I_Portfolios_Id == p.I_Portfolios_Id select c.I_PortfoliosI_Companies_Id).Count(),
                            Clients = (from po in _dbContext.I_PortfoliosG_Organizations where p.I_Portfolios_Id == po.I_Portfolios_Id select po.I_PortfoliosG_Organizations_Id).Count(),
                            GESStandardUniverse = p.GESStandardUniverse
                        };
            }

            if (orgId > 0)
                query = query.Where(x => x.OrganizationId == orgId);

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                        break;
                    case "type":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Type)
                            : query.OrderByDescending(x => x.Type);
                        break;
                    case "includeinalerts":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.IncludeInAlerts)
                            : query.OrderByDescending(x => x.IncludeInAlerts);
                        break;
                    case "showincsc":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ShowInCSC)
                            : query.OrderByDescending(x => x.ShowInCSC);
                        break;
                    case "standardportfolio":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.StandardPortfolio)
                            : query.OrderByDescending(x => x.StandardPortfolio);
                        break;
                    case "created":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Created)
                            : query.OrderByDescending(x => x.Created);
                        break;
                    case "companies":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Companies)
                            : query.OrderByDescending(x => x.Companies);
                        break;
                    case "controversialactivities":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.PortfolioId)
                            : query.OrderByDescending(x => x.GEServices);
                        break;
                    case "geservices":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.GEServices)
                            : query.OrderByDescending(x => x.PortfolioId);
                        break;
                    case "portfolioid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.PortfolioId)
                            : query.OrderByDescending(x => x.PortfolioId);
                        break;
                    case "clients":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Clients)
                            : query.OrderByDescending(x => x.Clients);
                        break;
                    default:
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<PortfolioViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public PaginatedResults<PortfolioControActivPresetListViewModel> GetControActivPresets(JqGridViewModel jqGridParams)
        {
            var query = from p in _dbContext.I_ControversialActivitesPresets
                        select new PortfolioControActivPresetListViewModel()
                        {
                            ControActivPresetId = p.I_ControversialActivitesPresets_Id,
                            ControActivPresetName = p.Name,
                            Created = p.Created,
                            ControversialActivities = (from c in _dbContext.I_ControversialActivitesPresetsItems where c.I_ControversialActivitesPresets_Id == p.I_ControversialActivitesPresets_Id select c.I_ControversialActivitesPresetsItems_Id).Count()
                        };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "controactivid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ControActivPresetId)
                            : query.OrderByDescending(x => x.ControActivPresetId);
                        break;
                    case "controactivname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ControActivPresetName)
                            : query.OrderByDescending(x => x.ControActivPresetName);
                        break;
                    case "created":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Created)
                            : query.OrderByDescending(x => x.Created);
                        break;
                    case "controversialactivities":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ControversialActivities).ThenBy(x => x.ControActivPresetName)
                            : query.OrderByDescending(x => x.ControversialActivities).ThenBy(x => x.ControActivPresetName);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<PortfolioControActivPresetListViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public PaginatedResults<CompanyPortfolioViewModel> GetCompanyListForPortfolioDetails(JqGridViewModel jqGridParams,
            long id)
        {
            var query = from c in _dbContext.I_Companies
                        join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                        into pce
                        from pc in (from pc in pce where pc.I_Portfolios_Id == id select pc).DefaultIfEmpty()
                        select new CompanyPortfolioViewModel()
                        {
                            CompanyId = c.I_Companies_Id,
                            SustainalyticsID = c.Id,
                            CompanyName = c.Name,
                            IsMasterCompany = c.MasterI_Companies_Id == null,
                            Isin = c.Isin,
                            Sedol = c.Sedol,
                            IsInThisPortfolio = pc != null,
                            ShowInClient = c.ShowInClient
                        };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "companyname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.CompanyName)
                            : query.OrderByDescending(x => x.CompanyName);
                        break;
                    case "isin":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Isin)
                            : query.OrderByDescending(x => x.Isin);
                        break;
                    case "sedol":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Sedol)
                            : query.OrderByDescending(x => x.Sedol);
                        break;
                    case "isinthisportfolio":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.IsInThisPortfolio)
                            : query.OrderByDescending(x => x.IsInThisPortfolio);
                        break;
                    case "companyid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.CompanyId)
                            : query.OrderByDescending(x => x.CompanyId);
                        break;
                    case "ismastercompany":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.IsMasterCompany)
                            : query.OrderByDescending(x => x.IsMasterCompany);
                        break;
                    case "showinclient":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ShowInClient)
                            : query.OrderByDescending(x => x.ShowInClient);
                        break;
                    case "sustainalyticsid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.SustainalyticsID)
                            : query.OrderByDescending(x => x.SustainalyticsID);
                        break;
                    default:
                        query = query.OrderBy(x => x.CompanyName);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CompanyPortfolioViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }


        public PaginatedResults<PendingCompanyViewModel> GetPendingCompanies(JqGridViewModel jqGridParams,
            long portfolioId)
        {
            var query = from pci in _dbContext.I_PortfolioCompaniesImport
                        where pci.I_Portfolios_Id == portfolioId
                        select new PendingCompanyViewModel()
                        {
                            Id = pci.I_PortfolioCompaniesImport_Id,
                            Name = pci.OtherName,
                            Isin = pci.Isin,
                            Sedol = pci.Sedol,
                            MasterCompanyId = pci.MasterI_Companies_Id,
                            Screened = pci.Screened,
                            SelectedToAdd = false,
                            SelectedToDelete = false
                        };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                        break;
                    case "isin":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Isin)
                            : query.OrderByDescending(x => x.Isin);
                        break;
                    case "sedol":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Sedol)
                            : query.OrderByDescending(x => x.Sedol);
                        break;
                    case "mastercompanyid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.MasterCompanyId)
                            : query.OrderByDescending(x => x.MasterCompanyId);
                        break;
                    case "screened":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Screened)
                            : query.OrderByDescending(x => x.Screened);
                        break;
                    case "selectedtoadd":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.SelectedToAdd)
                            : query.OrderByDescending(x => x.SelectedToAdd);
                        break;
                    case "selectedtodelete":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.SelectedToDelete)
                            : query.OrderByDescending(x => x.SelectedToDelete);
                        break;
                    default:
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Id)
                            : query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<PendingCompanyViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public IEnumerable<PendingCompanyViewModel> GetPendingCompanies(long portfolioId)
        {
            var query = from pci in _dbContext.I_PortfolioCompaniesImport
                        where pci.I_Portfolios_Id == portfolioId
                        select new PendingCompanyViewModel()
                        {
                            Id = pci.I_PortfolioCompaniesImport_Id,
                            Name = pci.OtherName,
                            Isin = pci.Isin,
                            Sedol = pci.Sedol,
                            MasterCompanyId = pci.MasterI_Companies_Id,
                            Screened = pci.Screened
                        };
            query = query.OrderBy(x => x.Id);

            return query.Select(item => new PendingCompanyViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Isin = item.Isin,
                Sedol = item.Sedol,
                MasterCompanyId = item.MasterCompanyId,
                Screened = item.Screened
            }).AsEnumerable();
        }

        public List<GesStandardUniverseViewModel> GetGesStandardUniverseCompanies(bool includeChildren = false)
        {
            var query = from c in _dbContext.I_Companies
                        join pc in _dbContext.I_PortfoliosI_Companies on c.I_Companies_Id equals pc.I_Companies_Id
                        join p in _dbContext.I_Portfolios on pc.I_Portfolios_Id equals p.I_Portfolios_Id
                        where p.GESStandardUniverse == true
                        select new GesStandardUniverseViewModel()
                        {
                            CompanyId = c.I_Companies_Id,
                            SustainalyticsID = c.Id,
                            MasterCompanyId = c.MasterI_Companies_Id,
                            CompanyName = c.Name.Trim(),
                            Isin = c.Isin,
                            Sedol = c.Sedol,
                            Country = c.Countries != null ? c.Countries.Name : "",
                            MsciId = c.I_Msci_Id,
                            SortOrder = c.Name.Trim(),
                            PortfolioName = p.Name,
                            ClientName = (from po in _dbContext.I_PortfoliosG_Organizations where p.I_Portfolios_Id == po.I_Portfolios_Id && po.G_Organizations != null select new {Name = po.G_Organizations.Name, Order = po.G_Organizations.Name.Contains("ges")?1:2 }).OrderBy(d=>d.Order).Select(d=>d.Name).FirstOrDefault()
                        };
            var result = this.SafeExecute(query.ToList);

            var lstMaster = result.Where(d => d.MasterCompanyId != null).Select(d => new { MasterCompanyId = d.MasterCompanyId.Value , d.ClientName, d.PortfolioName});
            var lstMasterId = result.Where(d => d.MasterCompanyId != null).Select(d => d.MasterCompanyId).ToList();

            //Get master companies
            if (lstMasterId.Any())
            {
                var lstMasterCompanies = (from c in _dbContext.I_Companies
                                      where lstMasterId.Contains(c.I_Companies_Id)
                                      select new GesStandardUniverseViewModel()
                                      {
                                          CompanyId = c.I_Companies_Id,
                                          SustainalyticsID = c.Id,
                                          MasterCompanyId = c.MasterI_Companies_Id,
                                          CompanyName = c.Name.Trim(),
                                          Isin = c.Isin,
                                          Sedol = c.Sedol,
                                          Country = c.Countries != null ? c.Countries.Name : "",
                                          MsciId = c.I_Msci_Id,
                                          SortOrder = c.Name.Trim()
                                      }).ToList();

                var masterCompanies = from m in lstMasterCompanies
                                      join o in lstMaster on m.CompanyId equals o.MasterCompanyId
                                      select new GesStandardUniverseViewModel()
                                      {
                                          CompanyId = m.CompanyId,
                                          SustainalyticsID = m.SustainalyticsID,
                                          MasterCompanyId = m.MasterCompanyId,
                                          CompanyName = m.CompanyName,
                                          Isin = m.Isin,
                                          Sedol = m.Sedol,
                                          Country = m.Country,
                                          MsciId = m.MsciId,
                                          SortOrder = m.SortOrder,
                                          ClientName = o.ClientName,
                                          PortfolioName = o.PortfolioName
                                      };

                if (includeChildren)
                {
                    //Reset sortorder for children
                    foreach (var master in masterCompanies)
                    {
                        var children =
                            result.Where(d => d.MasterCompanyId != null && d.MasterCompanyId == master.CompanyId)
                                .ToList();

                        foreach (var child in children)
                        {
                            child.SortOrder = master.SortOrder + child.SortOrder;
                        }
                    }
                }
                result.AddRange(masterCompanies);
            }

            //Remove child companies
            if (!includeChildren)
            {
                var childrenCompanies = result.Where(d => d.MasterCompanyId != null).ToList();
                if (childrenCompanies.Any())
                {
                    foreach (var item in childrenCompanies)
                    {
                        result.Remove(item);
                    }
                }
            }

            //remove duplicate companies
            if (includeChildren)
            {
                var rs = (from c in result
                    select new
                    {
                        CompanyId = c.CompanyId,
                        SustainalyticsID = c.SustainalyticsID,
                        MasterCompanyId = c.MasterCompanyId,
                        CompanyName = c.CompanyName,
                        Isin = c.Isin,
                        Sedol = c.Sedol,
                        Country = c.Country,
                        MsciId = c.MsciId,
                        SortOrder = c.SortOrder,
                        c.ClientName,
                        c.PortfolioName
                    }).Distinct();

                result = rs.Select(c => new GesStandardUniverseViewModel()
                {
                    CompanyId = c.CompanyId,
                    SustainalyticsID = c.SustainalyticsID,
                    MasterCompanyId = c.MasterCompanyId,
                    CompanyName = c.CompanyName,
                    Isin = c.Isin,
                    Sedol = c.Sedol,
                    Country = c.Country,
                    MsciId = c.MsciId,
                    SortOrder = c.SortOrder,
                    ClientName = c.ClientName,
                    PortfolioName = c.PortfolioName
                }).ToList();
            }
            else
            {
                var rs = (from c in result
                          select new
                          {
                              CompanyId = c.CompanyId,
                              MasterCompanyId = c.MasterCompanyId,
                              SustainalyticsID = c.SustainalyticsID,
                              CompanyName = c.CompanyName,
                              Isin = c.Isin,
                              Sedol = c.Sedol,
                              Country = c.Country,
                              MsciId = c.MsciId,
                              SortOrder = c.SortOrder
                          }).Distinct();

                result = rs.Select(c => new GesStandardUniverseViewModel()
                {
                    CompanyId = c.CompanyId,
                    SustainalyticsID = c.SustainalyticsID,
                    MasterCompanyId = c.MasterCompanyId,
                    CompanyName = c.CompanyName,
                    Isin = c.Isin,
                    Sedol = c.Sedol,
                    Country = c.Country,
                    MsciId = c.MsciId,
                    SortOrder = c.SortOrder
                }).ToList();
            }

            //TODO:MENDO
            //Get MSCI infor
            //var listAllMsci = _dbContext.SubPeerGroups.ToList();
            //var listAddedMsci = new List<MsciSectorModel>();

            //foreach (var companyItem in result)
            //{
            //    if (companyItem.MsciId != null)
            //    {
            //        var msciItem = listAddedMsci.FirstOrDefault(d => d.MsciId == companyItem.MsciId);

            //        if (msciItem == null)
            //        {
            //            msciItem = CommonProcess.GetMsciSector(listAllMsci, companyItem.MsciId.Value);
            //            listAddedMsci.Add(msciItem);
            //        }
            //        companyItem.Sector = msciItem.Sector;
            //        companyItem.IndustryGroup = msciItem.IndustryGroup;
            //        companyItem.Industry = msciItem.Industry;
            //        companyItem.SectorCode = msciItem.SectorCode;
            //    }
            //}

            return result.OrderBy(d => d.SortOrder).ToList();
        }

        public List<GesStandardUniverseViewModel> GetGesStandardUniversePendingCompanies()
        {
            var query = from c in _dbContext.I_PortfolioCompaniesImport
                join co in _dbContext.I_Companies on c.I_PortfolioCompaniesImport_Id equals co.I_Companies_Id
                join p in _dbContext.I_Portfolios on c.I_Portfolios_Id equals p.I_Portfolios_Id
                where p.GESStandardUniverse == true
                select new GesStandardUniverseViewModel()
                {
                    CompanyId = c.I_PortfolioCompaniesImport_Id,
                    SustainalyticsID = co.Id,
                    MasterCompanyId = c.MasterI_Companies_Id,
                    CompanyName = c.OtherName.Trim(),
                    Isin = c.Isin,
                    PortfolioName = p.Name,
                    Screened = c.Screened?"Yes":"No",
                    ClientName = (from po in _dbContext.I_PortfoliosG_Organizations
                        where p.I_Portfolios_Id == po.I_Portfolios_Id && po.G_Organizations != null
                        select
                            new
                            {
                                Name = po.G_Organizations.Name,
                                Order = po.G_Organizations.Name.Contains("ges") ? 1 : 2
                            }).OrderBy(d => d.Order)
                        .Select(d => d.Name)
                        .FirstOrDefault()
                };

            return query.OrderBy(d=>d.CompanyName).ToList();
        }

        public IEnumerable<PortfolioViewModel> GetGesPortfoliosByClientId(long orgId)
        {
            var query = from o in _dbContext.I_PortfoliosG_Organizations
                join p in _dbContext.I_Portfolios on o.I_Portfolios_Id equals p.I_Portfolios_Id
                join t in _dbContext.I_PortfolioTypes on p.I_PortfolioTypes_Id equals t.I_PortfolioTypes_Id
                select new PortfolioViewModel()
                {
                    PortfolioOrganizationId = o.I_PortfoliosG_Organizations_Id,
                    PortfolioId = o.I_Portfolios_Id,
                    Name = p.Name,
                    Type = t.Name,
                    OrganizationId = o.G_Organizations_Id,
                    IncludeInAlerts = o.IncludeInAlerts,
                    ShowInCSC = p.ShowInCSC,
                    StandardPortfolio = p.StandardPortfolio,
                    Created = p.Created,
                    Companies = (from c in _dbContext.I_PortfoliosI_Companies where c.I_Portfolios_Id == p.I_Portfolios_Id select c.I_PortfoliosI_Companies_Id).Count(),
                    ControversialActivities = (from ca in _dbContext.I_PortfoliosG_OrganizationsI_ControversialActivites where ca.I_PortfoliosG_Organizations_Id == o.I_PortfoliosG_Organizations_Id select ca.I_PortfoliosG_OrganizationsI_ControversialActivites_Id).Count(),
                    GEServices = o.I_PortfoliosG_OrganizationsG_Services.Count(),
                    GEServiceIds = o.I_PortfoliosG_OrganizationsG_Services.Select(d => d.G_Services_Id),
                    Clients = 0,
                    GESStandardUniverse = p.GESStandardUniverse
                };
            
            var results = query.Where(x => x.OrganizationId == orgId);

            return results;
        }

        public PaginatedResults<PortfolioControActivityViewModel> GetControActivities(JqGridViewModel jqGridParams, long portfolioOrgId)
        {
            var query = from c in _dbContext.I_ControversialActivites
                        join poc in _dbContext.I_PortfoliosG_OrganizationsI_ControversialActivites on c.I_ControversialActivites_Id equals poc.I_ControversialActivites_Id
                        into co
                        from poc in (from poc in co where poc.I_PortfoliosG_Organizations_Id == portfolioOrgId select poc).DefaultIfEmpty()
                        join po in _dbContext.I_PortfoliosG_Organizations on poc.I_PortfoliosG_Organizations_Id equals po.I_PortfoliosG_Organizations_Id
                        into pog
                        from po in (from po in pog where po.I_PortfoliosG_Organizations_Id == portfolioOrgId select po).DefaultIfEmpty()
                        select new PortfolioControActivityViewModel()
                        {
                            ControActivId = c.I_ControversialActivites_Id,
                            ControActivName = c.Name,
                            Threshold = poc.MinimumInvolvment
                        };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "controactivid":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ControActivId)
                            : query.OrderByDescending(x => x.ControActivId);
                        break;
                    case "controactivname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ControActivName)
                            : query.OrderByDescending(x => x.ControActivName);
                        break;
                    case "threshold":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Threshold).ThenBy(x => x.ControActivId)
                            : query.Where(i => i.Threshold != null).OrderBy(x => x.ControActivId).Concat(query.Where(i => i.Threshold == null).OrderBy(x => x.ControActivId));
                        break;
                }
            }

            return query.ToPagedList(Logger, 1, jqGridParams.rows);
        }

        public String[] GetPortfolioTypes()
        {
            return this.SafeExecute(() => _dbContext.I_PortfolioTypes.Select(x => x.Name).ToArray());
        }

        public IEnumerable<IdNameModel> GetPortfolioTypeItems()
        {
            return this.SafeExecute(() => _dbContext.I_PortfolioTypes.Select(x => new IdNameModel()
            {
                Id = x.I_PortfolioTypes_Id,
                Name = x.Name
            }).ToArray());
        }

        public I_Portfolios GetById(long id)
        {
            return _portfolioRepository.GetById(id);
        }

        public List<IdNameModel> GetPortfoliosWithTerm(string term, int limit, bool onlyStandardPortfolios = false)
        {
            var query = from o in _dbContext.I_Portfolios
                        where o.Name.Contains(term)
                        select new IdNameModel
                        {
                            Id = o.I_Portfolios_Id,
                            Name = "#" + o.I_Portfolios_Id + ". " + o.Name
                        };

            // standard?
            if (onlyStandardPortfolios)
            {
                query = from o in _dbContext.I_Portfolios
                        where o.Name.Contains(term) && o.StandardPortfolio
                        select new IdNameModel
                        {
                            Id = o.I_Portfolios_Id,
                            Name = "#" + o.I_Portfolios_Id + ". " + o.Name
                        };
            }

            return this.SafeExecute(() => query.OrderBy(x => x.Name).Take(limit).ToList());
        }

        public IEnumerable<I_Portfolios> GetStandardPortfolios()
        {
            return this.SafeExecute(() => _portfolioRepository.FindBy(x => x.StandardPortfolio).OrderByDescending(x => x.Created));
        }
    }
}
