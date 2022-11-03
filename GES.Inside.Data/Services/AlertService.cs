using System;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class AlertService : EntityService<GesEntities, I_NaArticles>, IAlertService
    {
        private readonly GesEntities _dbContext;
        private readonly II_NaArticlesRepository _naArticlesReporsitory;

        public AlertService(IUnitOfWork<GesEntities> unitOfWork, II_NaArticlesRepository naArticlesReporsitory, IGesLogger logger) : base(unitOfWork, logger, naArticlesReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _naArticlesReporsitory = naArticlesReporsitory;
        }

        public IEnumerable<AlertListViewModel> GetAlertsByCompanyId(long companyId)
        {
            return this.SafeExecute<IEnumerable<AlertListViewModel>>(() =>
            {
                return (from x in _dbContext.I_NaArticles
                        join ns in (from nna in _dbContext.I_NasI_NaArticles
                                    join na in _dbContext.I_Nas on nna.I_Nas_Id equals na.I_Nas_Id
                                    group nna by nna.I_NaArticles_Id into g
                                    select g.OrderByDescending(d => d.I_Nas.DateSent).FirstOrDefault()) on x.I_NaArticles_Id equals ns.I_NaArticles_Id into nsg
                        from ns in nsg.DefaultIfEmpty()
                        where x.I_Companies_Id == companyId
                        select new AlertListViewModel
                        {
                            Id = x.I_NaArticles_Id,
                            CompanyId = x.I_Companies_Id,
                            Heading = x.Heading,
                            Date = x.SourceDate,
                            LastModified = (ns != null && ns.I_Nas != null)? ns.I_Nas.DateSent: null,
                            Summary = x.Text,
                            Location = x.Countries != null ? x.Countries.Name : "",
                            Norm = x.I_NormAreas != null ? x.I_NormAreas.Name : "",
                            Source = x.Source,
                            IsExtended = x.IsExtended,
                            NaTypeId = x.I_NaTypes_Id
                        }).FromCache().ToList();
            });
        }

        public AlertListViewModel GetAlertById(long id)
        {

            var alert = (from x in _dbContext.I_NaArticles
                join ns in (from nna in _dbContext.I_NasI_NaArticles
                    join na in _dbContext.I_Nas on nna.I_Nas_Id equals na.I_Nas_Id
                    group nna by nna.I_NaArticles_Id
                    into g
                    select g.OrderByDescending(d => d.I_Nas.DateSent).FirstOrDefault()) on x.I_NaArticles_Id equals ns
                    .I_NaArticles_Id into nsg
                from ns in nsg.DefaultIfEmpty()
                where x.I_NaArticles_Id == id
                select new AlertListViewModel
                {
                    Id = x.I_NaArticles_Id,
                    CompanyId = x.I_Companies_Id,
                    Heading = x.Heading,
                    Date = x.SourceDate,
                    LastModified = (ns != null && ns.I_Nas != null) ? ns.I_Nas.DateSent : null,
                    Summary = x.Text,
                    Location = x.Countries != null ? x.Countries.Name : "",
                    Norm = x.I_NormAreas != null ? x.I_NormAreas.Name : "",
                    NormId = x.I_NormAreas_Id,
                    Source = x.Source,
                    SourceDate = x.SourceDate,
                    IsExtended = x.IsExtended,
                    NaTypeId = x.I_NaTypes_Id,
                    CompanyViewModel = new CompanyViewModel()
                    {
                        Id = (long) x.I_Companies_Id,
                        Name = x.I_Companies.MsciName,
                        Isin = x.I_Companies.Isin,
                        Location = x.I_Companies.Countries.Name,
                        CountryCode = x.I_Companies.Countries.Alpha3Code,
                        Industry = x.I_Companies.SubPeerGroups.Name,
                        WebsiteUrl = x.I_Companies.Website
                        
                    }
                }).FromCache().FirstOrDefault();

            return alert;

        }
    }
}